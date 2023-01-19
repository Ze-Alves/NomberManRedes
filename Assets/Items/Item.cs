using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Power,Bombs,Speed
    }

    public ItemType type;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().ItemPick(type);
            Destroy(gameObject);
        }
            
    }
}
