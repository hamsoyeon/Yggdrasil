using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGet : MonoBehaviour
{
    GameObject obj;

    void Start()
    {
        //시작하자 마자 dondestoryonload오브젝트를 찾아서 보관
        obj = GameObject.Find("UIController");
    }

    public void BtnPress()
    {
        //버튼눌러서 보내는 함수
        obj.GetComponent<TutorialButton>().TutorialTrigger = true;
        SceneManager.LoadScene("UIScene");
    }
}
