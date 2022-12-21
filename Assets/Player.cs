using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    float xInput, yInput;
    new Rigidbody rigidbody;
    public  GameObject bomb;
    public int power, bombs;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.E))
            PlaceBomb();
    }

    private void FixedUpdate()
    {
        //rigidbody.velocity = Vector3.zero;

        //rigidbody.AddForce(xInput * moveSpeed, 0, yInput * moveSpeed);
        transform.position += new Vector3(xInput * moveSpeed * Time.deltaTime, yInput * moveSpeed * Time.deltaTime);
    }


    void PlaceBomb()
    {
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
}
