using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public int size = 2;
    public float xsize, ysize;
    public float RaySize;

    public GameObject Explosionx, Explosiony;
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Explode() {
    yield return new WaitForSeconds(2);

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

       

        Vector2 RaycastPos;
        for (int w = -1; w < 2; w+=2) {
            RaycastPos = transform.position;
            for (int j = 0; j < size; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast(RaycastPos, new Vector2(w, 0),RaySize);

                RaycastPos.x += w * RaySize;
                if (hit)
                {
                    if (hit.transform.tag == "Wall")
                    {
                        break;
                    }
                    if (hit.transform.tag == "Box")
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
                Instantiate(Explosionx, RaycastPos, Quaternion.identity);
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
                    if (hit.transform.tag == "Box")
                    {
                        Destroy(hit.transform.gameObject);
                    }
                }
                Instantiate(Explosiony, RaycastPos, Quaternion.identity);
            }
        }

        Destroy(gameObject);




    }
}
