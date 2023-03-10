using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Netcode.Transports.UNET;
using System.Net.Sockets;
using System.Net;
public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set;}

    [HideInInspector]
    public List<Vector2> BomPoses = new List<Vector2>();
    public GameObject player;
    public List<GameObject> Items;
    public GameObject Boxes;
    public int PlayerConnected=0;
    public TextMeshProUGUI IP;
    public GameObject UIStuff,StartB,RestartB;
    public TMP_InputField ipunt;
    UNetTransport Transport;
    [HideInInspector] public NetworkVariable<int> alivePlayers = new NetworkVariable<int>();
    [HideInInspector]
    public NetworkVariable<bool> IsGame_Active = new NetworkVariable<bool>(false);
    public GameObject WinScreen, LoseScreen;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStart;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;

        try
        {
            if (!IsOwnedByServer&&FindObjectOfType<RestartManager>()!=null)
            {
                if (FindObjectOfType<RestartManager>().host)
                {
                    CreateHost();
                }
                else
                    Client();
            }
        }
        catch { }

        Transport = NetworkManager.Singleton.GetComponent<UNetTransport>();
        IPStuff();


        #region trash
        //for (int i = 0; i < size; i++)
        //{


        //    for (int j = 0; j < size; j++)
        //    {
        //        Debug.Log(i+""+j+grid[i, j]);

        //    }
        //}
        #endregion
    }

    void IPStuff()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                IP.text = ip.ToString();
            }
        }
    }

    void Update()
    {
        if (alivePlayers.Value < 2 && IsGame_Active.Value)
        {
            if (IsHost)
            {
                IsGame_Active.Value = false;
                RestartB.SetActive(true);
                EndGamClientRpc();
            }
        }
    }

    [ClientRpc]
    void EndGamClientRpc()
    {
        foreach (Player player in FindObjectsOfType<Player>(true))
        {
            if (player.IsOwner)
                EndScreen(player.alive);
        }
    }

    public void EndScreen(bool screen)
    {
        if (screen)
            WinScreen.SetActive(true);
        else
            LoseScreen.SetActive(true);
    }

    public void StartGame()
    {

        IsGame_Active.Value = true;
        StartB.SetActive(false);
        alivePlayers.Value = NetworkManager.Singleton.ConnectedClients.Count;
    }

    public void RestartMatch()
    {

        if (IsServer)
            RestartMatchServerRpc();
    }


    [ServerRpc]
    void RestartMatchServerRpc()
    {
        StartB.SetActive(true);
        RestartB.SetActive(false);
        RestartMatchClientRpc();
    }

    [ClientRpc]
    void RestartMatchClientRpc()
    {
        for (int i = 0; i < Boxes.transform.childCount; i++)
        {
            Boxes.transform.GetChild(i).gameObject.SetActive(true);
        }

        foreach (Item item in Object.FindObjectsOfType<Item>())
        {
            Destroy(item.gameObject);
        }

        foreach (Player player in FindObjectsOfType<Player>(true))
        {
            player.gameObject.SetActive(true);
            player.ResetSats();
        }
        WinScreen.SetActive(false);
        LoseScreen.SetActive(false);
    }

    #region ServerStuff

    public void HandleServerStart()
    {
        if (IsOwner)
        {
            UpdateCountServerRpc();
        }
    }

    public void HandleClientConnected(ulong cID)
    {
        if(IsOwner)
        UpdateCountServerRpc();
    }

    [ServerRpc]
    public void UpdateCountServerRpc()
    {
        PlayerConnected = NetworkManager.Singleton.ConnectedClients.Count;
        UpdateCountClientRpc(PlayerConnected);
    }


    [ClientRpc]
    public void UpdateCountClientRpc(int count)
    {
        PlayerConnected = count;
    }

    [ServerRpc]
    public void BoxServerRpc(int r,Vector2 pos)
    {
        BoxClientRpc(r,pos);
    }


    [ClientRpc]
    public void BoxClientRpc(int r,Vector2 pos)
    {
        Instantiate(Items[r], pos, Quaternion.identity);
    }

    public void CreateHost()
    {
        GameObject.Find("StartGame").SetActive(false);
        StartB.SetActive(true);
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
        string g= ipunt.text;

        Transport.ConnectAddress = g; 
        IP.text = g;

        GameObject.Find("StartGame").SetActive(false);
        NetworkManager.Singleton.StartClient();
    }

    public void ClientConnectMenu()
    {
        GameObject.Find("Host").SetActive(false);
        GameObject.Find("Client").SetActive(false);
        UIStuff.SetActive(true);
    }
    #endregion

   

}
