using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float moveSpeed;
    float xInput, yInput;
    new Rigidbody rigidbody;
    public  GameObject bomb;
    public GameObject PStats;
    GameObject stats;
    public int power, bombs;
    int playerNumber;
    
    void Start()
    {
        //NetworkManager.Singleton.OnClientConnectedCallback += UpdatePos;
        //if (IsOwner)
        //    GameManager.Instance.UpdateCountServerRpc();
        
        //Debug.Log("sadasd"+transform.position);
       
        rigidbody = GetComponent<Rigidbody>();

        if(IsOwner)
        PlayerNumServerRpc();
        StartCoroutine(UpdatePos());
    }

    [ServerRpc]
    void PlayerNumServerRpc()
    {
        playerNumber = NetworkManager.Singleton.ConnectedClients.Count;
    }

    void Update()
    {
        if (IsOwner)
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.E))
                PlaceBomb();
        }
    }

    private void FixedUpdate()
    {
        //rigidbody.velocity = Vector3.zero;

        //rigidbody.AddForce(xInput * moveSpeed, 0, yInput * moveSpeed);
        transform.position += new Vector3(xInput * moveSpeed * Time.deltaTime, yInput * moveSpeed * Time.deltaTime);
    }


    void PlaceBomb()
    {
        //Debug.Log("lool"+bombs);

        if (bombs > 0)
        {
            float xPos = Mathf.Ceil(transform.position.x);
            float yPos = Mathf.Ceil(transform.position.y);
            //StatusClientRpc();
            //ChangeStatusServerRpc();

            Vector2 BombPos = new Vector2(xPos - .5f, yPos - .5f);

            if (!GameManager.Instance.BomPoses.Contains(BombPos))
            {
                PlaceBombServerRpc(BombPos);
                //Instantiate(bomb, BombPos, Quaternion.identity).GetComponent<Bomb>().size = power;

                bombs--;
                StartCoroutine(BombExplode(BombPos));

                GameManager.Instance.BomPoses.Add(BombPos);
            }
        }
    }

    [ServerRpc]
    void PlaceBombServerRpc(Vector2 pos)
    {
        //NetworkObject bombe = Instantiate(bomb, pos, Quaternion.identity);
        //bombe.gameObject.GetComponent<Bomb>().size = power;
        //bombe.Spawn();
        //bombe.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
        //Debug.Log("QQQQQQQ");
        PlaceBombClientRpc(pos);
    }

    [ClientRpc]
    void PlaceBombClientRpc(Vector2 pos)
    {
        //obj.
        //bombe.gameObject.GetComponent<Bomb>().size = power;

        Bomb bombe = Instantiate(bomb, pos, Quaternion.identity).GetComponent<Bomb>();
        bombe.size = power;
        bombe.owner = this;
        //bombe.gameObject.GetComponent<NetworkObject>().Spawn();
        Debug.Log("QQQQQQQFFFFFF");
    }



    IEnumerator BombExplode(Vector2 bombpos)
    {
        yield return new WaitForSeconds(2);
        bombs++;
        GameManager.Instance.BomPoses.Remove(bombpos);
    }

    IEnumerator UpdatePos()
    {
        if (IsOwner)
        {
            yield return new WaitForSeconds(.3f);


            Vector3 Posi = new Vector3(0, 0, 0);
            Vector3 statusPos=PStats.transform.position;
            switch (GameManager.Instance.PlayerConnected)
            {
                case 1:
                    Posi = new Vector3(1.5f, 3.5f, 0);
                    break;
                case 2:
                    Posi = new Vector3(1.5f, -3.5f, 0);
                    statusPos.y -= 3.5f;
                    break;

            }
            transform.position = Posi;

            StatusServerRpc(statusPos);
        }
            GetComponent<SpriteRenderer>().enabled = true;
            //Debug.Log(pos+"UUUUUUUUUU"+transform.position+GameManager.Instance.PlayerConnected);

        
    }

    [ServerRpc]
    void StatusServerRpc(Vector2 pos)
    {
        stats = Instantiate(PStats, pos, Quaternion.identity);

        stats.GetComponent<NetworkObject>().Spawn();

        stats.GetComponent<PlayerStats>().PlayerNum.Value=playerNumber;
        StatusClientRpc(pos);

    }

    [ClientRpc]
    void StatusClientRpc(Vector3 pos)
    {
        Debug.Log("RRRRRRRRR");
        //stats = Instantiate(PStats, pos, Quaternion.identity);
        //stats.GetComponent<PlayerStats>().p++;
        Debug.Log("RAR");

    }

    [ServerRpc]
    void ChangeStatusServerRpc(int type)
    {
        switch (type)
        {
            case 1:
                stats.GetComponent<PlayerStats>().Power.Value++;
                break;
            case 2:
                stats.GetComponent<PlayerStats>().bombCount.Value++;
                break;
            case 3:
                stats.GetComponent<PlayerStats>().Speed.Value++;
                break;

        }
            

        
    }

    public void ItemPick(Item.ItemType type)
    {
        switch (type)
        {
            case Item.ItemType.Power:
                if (IsOwner)
                    ChangeStatusServerRpc(1);
                power++;
                break;
            case Item.ItemType.Bombs:
                if(IsOwner)
                ChangeStatusServerRpc(2);
                bombs++;
                break;
            case Item.ItemType.Speed:
                if (IsOwner)
                    ChangeStatusServerRpc(3);
                moveSpeed++;
                break;
        }
    }
}
