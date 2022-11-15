using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public bool IsTutorialClear = false;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "Stage2")
        {
            IsTutorialClear = false;
        }
    }

    public void BtnPress()
    {
        IsTutorialClear = true;
        SceneManager.LoadScene("UIScene");
    }
}
