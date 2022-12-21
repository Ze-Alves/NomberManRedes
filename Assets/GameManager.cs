using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GameManager : MonoBehaviour
{
    public static GameManager Instance { get { return Nested.instance; } }

    private class Nested
    {
        static Nested()
        {

        }

        internal static readonly GameManager instance = new GameManager();
    }

    [HideInInspector]
    public List<Vector2> BomPoses = new List<Vector2>(); 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
