using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        InintroScene();
    }
    private void InintroScene()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
