using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartManager : MonoBehaviour
{
    // Start is called before the first frame update

    public bool host;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Host()
    {
        host = true;
        Clicked();
    }
    public void Client()
    {
        host = false;
        Clicked();
    }
    public void Clicked()
    {
        SceneManager.LoadScene("Level", LoadSceneMode.Additive);
    }
}
