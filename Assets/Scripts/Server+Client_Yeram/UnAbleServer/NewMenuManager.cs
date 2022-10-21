using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMenuManager : Singleton_Ver2.Singleton<NewMenuManager>
{
    [SerializeField]
    GameObject m_MainMenu;
    [SerializeField]
    GameObject m_Room;
    [SerializeField]
    GameObject m_Option;
    // Start is called before the first frame update
    void Start()
    {
        m_MainMenu.SetActive(true);
        m_Option.SetActive(false);
        m_Room.SetActive(false);
    }

    // Update is called once per frame
    public void OnClickGameStart()
    {
        m_MainMenu.SetActive(false);
        m_Room.SetActive(true);
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
}
