using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerStats : NetworkBehaviour
{
    public TextMeshPro pnum,bombs,power,speed;
    public NetworkVariable<int> PlayerNum = new NetworkVariable<int>();
    public NetworkVariable<int> Power = new NetworkVariable<int>();
    public NetworkVariable<int> bombCount = new NetworkVariable<int>();
    public NetworkVariable<int> Speed = new NetworkVariable<int>();
    [SerializeField] SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        PlayerColores();
        Power.OnValueChanged += UpdateStats;
        bombCount.OnValueChanged += UpdateStats;
        Speed.OnValueChanged += UpdateStats;
    }

    // Update is called once per frame
    void UpdateStats(int old,int neu)
    {
        bombs.text = "Bombs:" + bombCount.Value.ToString();
        power.text = "Power:" + Power.Value.ToString();
        speed.text = "Speed:" + Speed.Value.ToString();




    }

    void PlayerColores()
    {
        switch (GameManager.Instance.PlayerConnected)
        {

            case 1:
                sprite.color = Color.red;
                pnum.color = Color.red;
                break;
            case 2:
                sprite.color = Color.green;
                pnum.color = Color.green;
                break;
            case 3:
                sprite.color = Color.blue;
                pnum.color = Color.blue;
                break;
            case 4:
                sprite.color = Color.yellow;
                pnum.color = Color.yellow;
                break;
        }
    }

    void Update()
    {
        pnum.text = "Player " + PlayerNum.Value.ToString();
    }
}
