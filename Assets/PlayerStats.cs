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
    NetworkVariable<Color> color = new NetworkVariable<Color>();
    [SerializeField] SpriteRenderer sprite;


    // Start is called before the first frame update
    void Start()
    {
        if (IsHost)
        PlayerColoresServerRpc();
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

    [ServerRpc]
    void PlayerColoresServerRpc()
    {
        switch (GameManager.Instance.PlayerConnected)
        {
            case 1:
                color.Value = Color.red;
                break;
            case 2:
                color.Value = Color.green;
                break;
            case 3:
                color.Value = Color.blue;
                break;
            case 4:
                color.Value = Color.yellow;
                break;
        }
    }


    void Update()
    {
        
        pnum.text = "Player " + PlayerNum.Value.ToString();
        sprite.color = color.Value;
        pnum.color = color.Value;
    }
}
