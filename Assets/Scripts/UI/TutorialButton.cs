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
        //dondestoryonload가 무한으로 증식되는것을 막기위한 싱글톤 형식
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
        //게임에 들어가면 다시 게임을 시작할때를 위하여 튜토리얼 다시 false로 전환
        if (SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "Stage2")
            TutorialTrigger = false;
    }

    public void BtnPress()
    {
        TutorialTrigger = true;
        SceneManager.LoadScene("UIScene");
    }

}
