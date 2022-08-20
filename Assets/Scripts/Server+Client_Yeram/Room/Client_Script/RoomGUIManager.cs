using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ECharacterType = CharacterInfo.ECharacterType;

public class RoomGUIManager : Singleton_Ver2.Singleton<RoomGUIManager>
{
  

    public GameObject MapPannel;

    public bool is_Leader;

    public bool on_Ready;

    [SerializeField]
    private Button start_Btn;
    [SerializeField]
    private Button ready_Btn;

    [SerializeField]
    private ECharacterType select_Type;

    [SerializeField]
    private Image[] render_Char;

    [SerializeField]
    private Canvas m_canvas;

    #region ButtonClickEvent
    public void OnClick_Attack()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Attack);
    }
    public void OnClick_Defence()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Defense);
    }
    public void OnClick_Support()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Support);
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
        select_Type = ECharacterType.Attack;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        start_Btn.interactable = on_Ready;
    }

    public void RenderCharImage(int _slotindex,ECharacterType _type)
    {
        switch (_type)
        {

            case ECharacterType.Attack:
                render_Char[_slotindex].sprite = m_canvas.transform.GetChild("Attack_Button").GetComponent<Image>().sprite;// GameObject.Find("Attack_Button").GetComponent<Image>().sprite;
                break;

            case ECharacterType.Defense:
                render_Char[_slotindex].sprite = m_canvas.transform.GetChild("Defence_Button").GetComponent<Image>().sprite;//GameObject.Find("Defence_Button").GetComponent<Image>().sprite;
                break;

            case ECharacterType.Support:
                render_Char[_slotindex].sprite = m_canvas.transform.GetChild("support_Button").GetComponent<Image>().sprite; //GameObject.Find("support_Button").GetComponent<Image>().sprite;
                break;
        }

    }
}
