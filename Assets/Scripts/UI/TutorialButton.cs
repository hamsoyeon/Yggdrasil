using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialButton : MonoBehaviour
{
    public bool TutorialTrigger = false;
    public static TutorialButton instance = null;

    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "Stage2")
            TutorialTrigger = false;
    }

    public void BtnPress()
    {
        TutorialTrigger = true;
        SceneManager.LoadScene("UIScene");
    }

}
