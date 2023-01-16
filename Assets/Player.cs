using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public int moveSpeed;
    float xInput, yInput;
    new Rigidbody rigidbody;
    public  GameObject bomb;
    public GameObject PStats;
    GameObject stats;
    public int power, bombCount;
    int playerNumber;
    Animator anim;
    
    void Start()
    {
        //NetworkManager.Singleton.OnClientConnectedCallback += UpdatePos;
        //if (IsOwner)
        //    GameManager.Instance.UpdateCountServerRpc();
        
        //Debug.Log("sadasd"+transform.position);
       
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

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

            setAnimation();

            if (Input.GetKeyDown(KeyCode.E))
                PlaceBomb();
        }
    }
    void setAnimation()
    {
        anim.SetFloat("X", xInput);
        anim.SetFloat("Y", yInput);

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

        if (bombCount > 0)
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

                bombCount--;
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
        bombCount++;
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
                stats.GetComponent<PlayerStats>().Power.Value=power;
                stats.GetComponent<PlayerStats>().bombCount.Value=bombCount;
                stats.GetComponent<PlayerStats>().Speed.Value=moveSpeed;
    }

    public void ResetSats()
    {
        power = 1;
        moveSpeed = 7;
        bombCount = 1;
        if(IsOwner)
        ChangeStatusServerRpc(0);

        if (IsOwner)
        {
            Vector3 Posi = new Vector3(0, 0, 0);
            switch (playerNumber)
            {
                case 1:
                    Posi = new Vector3(1.5f, 3.5f, 0);
                    break;
                case 2:
                    Posi = new Vector3(1.5f, -3.5f, 0);
                    break;

            }
            transform.position = Posi;
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
                bombCount++;
                break;
            case Item.ItemType.Speed:
                if (IsOwner)
                    ChangeStatusServerRpc(3);
                moveSpeed++;
                break;
        }
    }
}
