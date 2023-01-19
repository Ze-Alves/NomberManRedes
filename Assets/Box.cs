using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Box : NetworkBehaviour
{
    public List<GameObject> Items;
    public float chance;
    int item;
    bool spawn=false;
    bool dead = false;
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
        if (dead)
        {
            Instantiate(Items[item], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

    public void Exploded()
    {
        if(IsOwner)
        BoxDestroyServerRpc();
    }


    [ServerRpc]
    void BoxDestroyServerRpc()
    {
        BoxDestroyClientRpc();
    }

    [ClientRpc]
    void BoxDestroyClientRpc()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (spawn&&IsOwner)
            GameManager.Instance.BoxServerRpc(item, transform.position);
    }

    private void OnDisable()
    {
        if (spawn && IsOwner)
            GameManager.Instance.BoxServerRpc(item, transform.position);
    }

}
