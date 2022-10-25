using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewMenuManager : Singleton_Ver2.Singleton<NewMenuManager>
{
    [SerializeField]
    GameObject m_MainMenu;
    [SerializeField]
    GameObject m_Room;
    [SerializeField]
    GameObject m_Option;

    [SerializeField]
    bool m_IsTutorialClear = false;
    GameObject ui;

    void Start()
    {
        m_MainMenu.SetActive(true);
        m_Option.SetActive(false);
        m_Room.SetActive(false);
        ui = GameObject.Find("UIController");
        m_IsTutorialClear = ui.GetComponent<TutorialButton>().IsTutorialClear;
    }

    public void OnClickGameStart()
    {
        m_MainMenu.SetActive(false);
        if(!m_IsTutorialClear)
        {
            SceneManager.LoadScene("TutorialScene");
        }
        if(m_IsTutorialClear)
        {
            m_Room.SetActive(true);
        }

    }
    public void OnClickOption()
    {
        m_Option.SetActive(true);
    }
    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickOptionOut()
    {
        m_Option.SetActive(false);
    }
    public void OnClickRoomOut()
    {
        m_Room.SetActive(false);
        m_MainMenu.SetActive(true);
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
