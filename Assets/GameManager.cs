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
    public GameObject Level;
    
    //public string[,] grid;



    public int PlayerConnected=0;

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        //grid = new string[size, size];

        //Vector3 RaycastPos;
        //RaycastHit hit;


        //for(int i = -size / 2; i < size / 2; i++)
        //{
        //    RaycastPos = new Vector3(origin.x, i + .5f, -1f);


        //    for(int j = -size / 2; j < size / 2; j++)
        //    {
        //        if(Physics.Raycast(RaycastPos,new Vector3(0,0,1),out hit))
        //        {
        //            //if (hit.transform.tag == "Box")
        //            //    grid[i, j] = "Box";

        //            grid[i+size/2, j+size/2] = hit.transform.tag;
        //            Debug.Log("AAA");
        //            //break;
        //        }

        //        RaycastPos.x++;

        //    }
        //}
    }

    void Start()
    {

        NetworkManager.Singleton.OnServerStarted += HandleServerStart;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;


        //Debug.Log("MUUUUUU"+FindObjectOfType<RestartManager>().transform.name);

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

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PlayerConnected + "WWWWW");
    }

    public void HandleServerStart()
    {
        if (IsOwner)
        {
            UpdateCountServerRpc();
            //SceneManager.LoadScene("Level", LoadSceneMode.Additive);
            //GameObject gs = Instantiate(player);
            //player.GetComponent<NetworkObject>();



        }
        //Instantiate(player);
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
        //Debug.Log("AAAAAAAAAA" + PlayerConnected);
        UpdateCountClientRpc(PlayerConnected);
    }


    [ClientRpc]
    public void UpdateCountClientRpc(int count)
    {
        PlayerConnected = count;
        //Debug.Log("AAAAAAAAAABBBBBBBBBBBB" + PlayerConnected);

        //NetworkManager.Singleton.LocalClientId
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
        //Debug.Log("AAAAAAAAAABBBBBBBBBBBB" + PlayerConnected);

        //NetworkManager.Singleton.LocalClientId
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


}
