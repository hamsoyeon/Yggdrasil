using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomGUIManager : Singleton_Ver2.Singleton<RoomGUIManager>
{
    public enum Player_Type
    {
        ATTACK = 0,
        DEFENCE,
        SUPPORT
    };

    public GameObject MapPannel;

    public bool is_Leader;

    public bool on_Ready;

    [SerializeField]
    private Button start_Btn;
    [SerializeField]
    private Button ready_Btn;

    [SerializeField]
    private Player_Type select_Type;

    [SerializeField]
    private Image[] render_Char;

    #region ButtonClickEvent
    public void OnClick_Attack()
    {
        select_Type = Player_Type.ATTACK;
        RenderCharImage();
    }
    public void OnClick_Defence()
    {
        select_Type = Player_Type.DEFENCE;
        RenderCharImage();
    }
    public void OnClick_Support()
    {
        select_Type = Player_Type.SUPPORT;
        RenderCharImage();
    }
    public void OnClick_Map()
    {
        MapPannel.SetActive(true);
    }
    public void OnClick_Lobby()
    {
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, true);
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, false);
        LobbyGUIManager.Instance.ClearChat();
    }
    public void OnClick_Ready()
    {
        on_Ready = !on_Ready;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        if (is_Leader)
        {
            start_Btn.interactable = on_Ready;
        }
        
    }
    public void OnClick_Start()
    {
        if (on_Ready)
        {
            Debug.Log("GameStart");
            //전부다 Ready일때 시작, 3명의 캐릭터가 안겹칠때 시작?(이건 기획분께 물어보기)
            //메인씬으로 전환
        }
    }
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    #endregion

    private void Start()
    {
        MapPannel.SetActive(false);
        on_Ready = false;
        select_Type = Player_Type.ATTACK;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        start_Btn.interactable = on_Ready;
    }

    private void RenderCharImage()
    {
        switch (select_Type)
        {

            case Player_Type.ATTACK:
                render_Char[1].sprite = GameObject.Find("Attack_Button").GetComponent<Image>().sprite;
                break;

            case Player_Type.DEFENCE:
                render_Char[1].sprite = GameObject.Find("Defence_Button").GetComponent<Image>().sprite;
                break;

            case Player_Type.SUPPORT:
                render_Char[1].sprite = GameObject.Find("support_Button").GetComponent<Image>().sprite;
                break;
        }

    }
}
