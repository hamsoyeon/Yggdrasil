using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Singleton_Ver2.Singleton<NewMenuManager>

public class NewMenuManager : MonoBehaviour
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

    NewRoomManager m_roommanager;

    void Start()
    {
        m_roommanager = GetComponent<NewRoomManager>();
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

    // ------------------------------------------- 수정사항 (승렬)

    public void OnClickStart()
    {
        if (m_roommanager.m_MapNum == 0)
            SceneManager.LoadScene("MainScene");

        if (m_roommanager.m_MapNum == 1)
            SceneManager.LoadScene("Stage2");

        //SceneManager.LoadScene("MainScene");
    }
}
