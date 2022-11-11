using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundDestory : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.Find("MainSound"))
            Destroy(GameObject.Find("MainSound"));

        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            SEManager.instance.LoopPlaySE("Stage1");
        }
        if(SceneManager.GetActiveScene().name == "Stage2")
        {

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("return"))
            SEManager.instance.StopSE("Stage1");
    }
}
