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
            switch (type)
            {
                case ItemType.Power:
                    other.GetComponent<Player>().power++;
                    break;
                case ItemType.Bombs:
                    other.GetComponent<Player>().bombs++;
                    break;
                case ItemType.Speed:
                    other.GetComponent<Player>().moveSpeed++;
                    break;
            }

            Destroy(gameObject);
        }
            
    }
}
