using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Bomb : MonoBehaviour
{
    public List<GameObject> Items;
    
    public int probi;
    public int size = 2;
    float RaySize = 1;

    public GameObject Explosionx, Explosiony,ExplosionEndx,ExplosionEndy,centro;
    public Animator animator;
    public Animation animation;
    public Player owner;
    public NetworkBehaviourReference refer;
    public LayerMask mask;

    void Start()
    {
        animator.Play("BombExplode");
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
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize,mask);
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
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(0, w), RaySize,mask);

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


    void BoxDestroy(GameObject box)
    {

        box.GetComponent<Box>().Exploded();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
