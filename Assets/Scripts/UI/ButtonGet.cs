using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonGet : MonoBehaviour
{
    GameObject obj;

    void Start()
    {
        obj = GameObject.Find("UIController");
    }

    public void BtnPress()
    {
        obj.GetComponent<TutorialButton>().TutorialTrigger = true;
        SceneManager.LoadScene("UIScene");
    }
}
