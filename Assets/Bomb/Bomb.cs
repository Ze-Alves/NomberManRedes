using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bomb : MonoBehaviour
{
    public List<GameObject> Items;
    
    public int probi;
    public int size = 2;
    //public float xsize, ysize;
    float RaySize = 1;

    public GameObject Explosionx, Explosiony,ExplosionEndx,ExplosionEndy,centro;
    public Animator animator;
    public Animation animation;
    public Player owner;
    public NetworkBehaviourReference refer;

    void Start()
    {
        //StartCoroutine(Explode());
        animator.Play("BombExplode");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Explosion()
    {
        Vector2 RaycastPos;
        Instantiate(centro, transform.position, Quaternion.identity);
        for (int w = -1; w < 2; w += 2)
        {
            RaycastPos = transform.position;
            for (int j = 0; j < size; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize);
                RaycastPos.x += w * RaySize;

                if (hit)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        break;
                    }
                    else
                    if (hit.transform.tag == "Box")
                    {
                        BoxDestroy(hit.transform.gameObject);
                    }
                    else
                    {
                        if (hit.transform.tag == "Player")
                        {
                            hit.transform.gameObject.SetActive(false);
                            GameManager.Instance.alivePlayers--;
                        }
                        RaycastHit2D hit2 = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize);
                        if (hit2 && hit2.transform.tag == "Box")
                            BoxDestroy(hit2.transform.gameObject);
                    }
                }
                if (j == size - 1)
                {
                    if (w == 1)
                        Instantiate(ExplosionEndx, RaycastPos, Explosionx.transform.rotation);
                    else
                        Instantiate(ExplosionEndx, RaycastPos, Quaternion.Euler(0,0,180));
                }else
                Instantiate(Explosionx, RaycastPos, Explosionx.transform.rotation);
            }
        }

        for (int w = -1; w < 2; w += 2)
        {
            RaycastPos = transform.position;
            for (int j = 0; j < size; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(0, w), RaySize);

                RaycastPos.y += w * RaySize;
                if (hit)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        break;
                    }
                    else
                    if (hit.transform.tag == "Box")
                    {
                        BoxDestroy(hit.transform.gameObject);
                    }
                    else
                    {
                        if (hit.transform.tag == "Player")
                            Destroy(hit.transform.gameObject);
                        RaycastHit2D hit2 = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize);
                        if (hit2 && hit2.transform.tag == "Box")
                            BoxDestroy(hit2.transform.gameObject);
                    }
                }
                if (j == size - 1)
                {
                    if (w == 1)
                        Instantiate(ExplosionEndy, RaycastPos, Explosiony.transform.rotation);
                    else
                        Instantiate(ExplosionEndy, RaycastPos, Quaternion.Euler(0, 0, -90));
                }
                else
                    Instantiate(Explosiony, RaycastPos, Explosiony.transform.rotation);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator Explode()
    {
        
        yield return new WaitForSeconds(2);

        #region Trash
        //Collider2D[] collider2s = Physics2D.OverlapBoxAll(transform.position, new Vector2(size*2,.5f), 0);
        //Collider2D[] collider2sy= Physics2D.OverlapBoxAll(transform.position, new Vector2(.5f, size*2), 0);



        //foreach (Collider2D collider in collider2s)
        //    if (collider.transform.tag == "Box")
        //        Destroy(collider.gameObject);

        //foreach (Collider2D collider in collider2sy)
        //    if (collider.transform.tag == "Box")
        //        Destroy(collider.gameObject);


        //for(int i = 1; i <= size; i++)
        //{
        //    Vector2 Expos = transform.position;
        //    Expos.x+=i;
        //    Instantiate(Explosionx, Expos,Quaternion.identity);
        //    Expos.x-=i;
        //    Expos.y+=i;
        //    Instantiate(Explosiony, Expos, Quaternion.identity);
        //    Expos.y-=i;
        //    Expos.x-=i;
        //    Instantiate(Explosionx, Expos, Quaternion.identity);
        //    Expos.x+=i;
        //    Expos.y-=i;
        //    Instantiate(Explosiony, Expos, Quaternion.identity);
        //}
        #endregion


        Vector2 RaycastPos;
        for (int w = -1; w < 2; w += 2)
        {
            RaycastPos = transform.position;
            for (int j = 0; j < size; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize);

                
                if (hit)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        break;
                    }
                    else
                    if (hit.transform.tag == "Box")
                    {
                        BoxDestroy(hit.transform.gameObject);
                    }
                    else
                    {
                        if (hit.transform.tag == "Player")
                            Destroy(hit.transform.gameObject);
                        RaycastHit2D hit2 = Physics2D.Raycast(new Vector2(RaycastPos.x+w*RaySize,RaycastPos.y), new Vector2(w, 0), RaySize);
                        if (hit2 && hit2.transform.tag == "Box")
                            BoxDestroy(hit2.transform.gameObject);

                    }
                    //Debug.Log(hit.transform.position);
                }
                Instantiate(Explosionx, RaycastPos, Explosionx.transform.rotation);
                RaycastPos.x += w * RaySize;
                //Debug.Log(j + "" + RaycastPos);
            }
            Instantiate(ExplosionEndx, RaycastPos, Explosionx.transform.rotation);
        }

        for (int w = -1; w < 2; w += 2)
        {
            RaycastPos = transform.position;
            for (int j = 0; j < size; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(0, w), RaySize);

                RaycastPos.y += w * RaySize;
                if (hit)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        break;
                    }
                    else
                    if (hit.transform.tag == "Box")
                    {
                        BoxDestroy(hit.transform.gameObject);
                    }
                    else
                    {
                        if (hit.transform.tag == "Player")
                            Destroy(hit.transform.gameObject);
                        RaycastHit2D hit2 = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize);
                        if (hit2 && hit2.transform.tag == "Box")
                            BoxDestroy(hit2.transform.gameObject);
                    }
                }
                Instantiate(Explosiony, RaycastPos, Explosiony.transform.rotation);
            }
        }

        Destroy(gameObject);
    }

    void BoxDestroy(GameObject box)
    {

        //int r = Random.Range(0, Items.Count);
        //int prob = Random.Range(0, 100);
        //if (prob < probi)
        //{

        //    //BoxDestroyServerRpc(box, r);
        //    Instantiate(Items[r], box.transform.position, Quaternion.identity);
        //}
        //Debug.Log(IsLocalPlayer + "" + IsSpawned + "" + IsOwnedByServer + "" + IsServer + "" + IsHost);

        //if (IsOwnedByServer)
        //{
        //    Debug.Log("KKKKKKKKKKKKK");
        //    BoxDestroyServerRpc(box.transform.position);
        //}
        //Destroy(box);

        //NetworkBehaviour me;
        //if(refer.TryGet(out me,NetworkManager.Singleton))
        //{
        //    Debug.Log("Meeme");
        //    if (me == this)
        //        Debug.Log("YESSSSSSSSSSSSSS");
        //}
        box.GetComponent<Box>().Exploded();
    }

    [ServerRpc]
    void BoxDestroyServerRpc(Vector3 pos)
    {
        Debug.Log("OOOOOOOOOOOOOOOOOOOOOOO");
        int r = Random.Range(0, Items.Count);
        int prob = Random.Range(0, 100);
        if (prob < probi)
        {

            //BoxDestroyServerRpc(box, r);
            BoxDestroyClientRpc(r,pos);

        }
    }

    [ClientRpc]
    void BoxDestroyClientRpc(int r,Vector3 pos)
    {
        Instantiate(Items[r], pos, Quaternion.identity);
        Debug.Log("EEEEEEEEEEEEEEE");
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
        Debug.Log("Saiu");
    }
}
