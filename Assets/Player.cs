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
    public int power, bombs;
    
    void Start()
    {
        //NetworkManager.Singleton.OnClientConnectedCallback += UpdatePos;
        //if (IsOwner)
        //    GameManager.Instance.UpdateCountServerRpc();
        
        Debug.Log("sadasd"+transform.position);
       
        rigidbody = GetComponent<Rigidbody>();

        StartCoroutine(UpdatePos());
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
        Debug.Log("lool"+bombs);
        if (bombs > 0)
        {
            float xPos = Mathf.Ceil(transform.position.x);
            float yPos = Mathf.Ceil(transform.position.y);

            Vector2 BombPos = new Vector2(xPos - .5f, yPos - .5f);

            if (!GameManager.Instance.BomPoses.Contains(BombPos))
            {

                Instantiate(bomb, BombPos, Quaternion.identity).GetComponent<Bomb>().size = power;
                bombs--;
                StartCoroutine(BombExplode(BombPos));

                GameManager.Instance.BomPoses.Add(BombPos);
            }
        }
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


            Vector3 pos = new Vector3(0, 0, 0);
            switch (GameManager.Instance.PlayerConnected)
            {
                case 1:
                    pos = new Vector3(1.5f, 3.5f, 0);
                    break;
                case 2:
                    pos = new Vector3(1.5f, -3.5f, 0);
                    break;

            }
            transform.position = pos;
        }
            GetComponent<SpriteRenderer>().enabled = true;
            //Debug.Log(pos+"UUUUUUUUUU"+transform.position+GameManager.Instance.PlayerConnected);

        
    }
}
