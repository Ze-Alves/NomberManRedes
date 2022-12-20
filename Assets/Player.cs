using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    float xInput, yInput;
    new Rigidbody rigidbody;
   public  GameObject bomb;
    

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
        float xPos =Mathf.Ceil( transform.position.x);
        float yPos = Mathf.Ceil(transform.position.y);

        Vector2 BombPos = new Vector2(xPos - .5f, yPos - .5f);
       


        Instantiate(bomb, BombPos,Quaternion.identity);
    }

}
