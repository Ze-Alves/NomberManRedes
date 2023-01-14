using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set;}

    [HideInInspector]
    public List<Vector2> BomPoses = new List<Vector2>();
    public int size;
    public Vector2 origin;
    public GameObject player;
    public List<GameObject> Items;
    public GameObject Boxes;
    public int PlayerConnected=0;

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

    void Update()
    {
    }



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
        Debug.Log("GRAU");
        Instantiate(Items[r], pos, Quaternion.identity);
    }

    public void CreateHost()
    {
        Debug.Log("BOP");
        NetworkManager.Singleton.StartHost();
    }

    public void Client()
    {
       
        NetworkManager.Singleton.StartClient();
    }

    
    public void RestartMatch()
    {
        if (IsServer)
            RestartMatchServerRpc();
    }

    [ServerRpc]
    void RestartMatchServerRpc()
    {
        RestartMatchClientRpc();
    }

    [ClientRpc]
    void RestartMatchClientRpc()
    {
        for (int i = 0; i < Boxes.transform.childCount; i++)
        {
            Boxes.transform.GetChild(i).gameObject.SetActive(true);
        }
        


        foreach(Item item in Object.FindObjectsOfType<Item>())
        {
            Destroy(item.gameObject);
        }

        foreach (Player player in FindObjectsOfType<Player>(true))
        {
            player.gameObject.SetActive(true);
            player.ResetSats();
        }
    }

}
