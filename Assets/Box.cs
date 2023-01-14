using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class Box : NetworkBehaviour//MonoBehaviour
{
    // Start is called before the first frame update
   
    public List<GameObject> Items;
    public float chance;
    int item;
    bool spawn=false;
    bool dead = false;
    // Update is called once per frame
    private void Start()
    {
        item = Random.Range(0, Items.Count);
        int prob = Random.Range(0, 100);
        if (prob < chance)
        {

            spawn = true;

        }
        NetworkManager.Singleton.OnServerStarted += Spawn ;
      
    }

    void Spawn()
    {
        GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.LocalClientId);
    }
    void Update()
    {
        //Debug.Log(IsLocalPlayer + "" + IsSpawned + "" + IsOwnedByServer + "" + IsServer + "" + IsHost);

        if (dead)
        {
            Instantiate(Items[item], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    public void Exploded()
    {
        //dead = true;
        int r = 2;
        if(IsOwner)
        BoxDestroyServerRpc(r);
    }


    [ServerRpc]
    void BoxDestroyServerRpc(int r)
    {
        //BoxDestroyClientRpc(r);
        GetComponent<NetworkObject>().Despawn();
        //Destroy(gameObject);
    }

    [ClientRpc]
    void BoxDestroyClientRpc(int r)
    {
       

        //Instantiate(Items[r],transform.position, Quaternion.identity);
    }



    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //    Destroy(gameObject);
    //}

    private void OnDestroy()
    {
        if (spawn&&IsOwner)
            GameManager.Instance.BoxServerRpc(item, transform.position);
    }


}
