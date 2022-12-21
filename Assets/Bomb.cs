using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public List<GameObject> Items;
    public int probi;
    public int size = 2;
    //public float xsize, ysize;
    float RaySize = 1;

    public GameObject Explosionx, Explosiony;
    void Start()
    {
        StartCoroutine(Explode());
    }

    // Update is called once per frame
    void Update()
    {

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
                            Destroy(hit.transform.gameObject);
                        RaycastHit2D hit2 = Physics2D.Raycast(RaycastPos, new Vector2(w, 0), RaySize);
                        if (hit2 && hit2.transform.tag == "Box")
                            BoxDestroy(hit2.transform.gameObject);

                    }
                    //Debug.Log(hit.transform.position);
                }
                Instantiate(Explosionx, RaycastPos, Quaternion.identity);
                //Debug.Log(j + "" + RaycastPos);
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
                Instantiate(Explosiony, RaycastPos, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }

    void BoxDestroy(GameObject box)
    {

        int r = Random.Range(0, Items.Count);
        int prob = Random.Range(0, 100);
        if (prob < probi)
        {
            Instantiate(Items[r], box.transform.position, Quaternion.identity);
        }

        Destroy(box);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = false;
    }
}
