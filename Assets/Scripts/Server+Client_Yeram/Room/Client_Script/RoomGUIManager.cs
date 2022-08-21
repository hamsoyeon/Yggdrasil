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
    private Button[] m_SelectChar_Btn;

    [SerializeField]
    private List<PlayerSlot> m_player_slots;

    [SerializeField]
    private GameObject parent_render_Char;

    [SerializeField]
    private Canvas m_canvas;

    private int[] test_slot = new int[3];

    #region ButtonClickEvent
    public void OnClick_Defence()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Defense);
    }
    public void OnClick_Attack()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Attack);
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
        if (is_Leader)
        {
            start_Btn.interactable = on_Ready;
        }

        //ready_Btn.gameObject.SetActive(!on_Ready);
        RoomManager.Instance.ReadyProcess(on_Ready);
        if (on_Ready)
        {
            for(int i=0;i< m_SelectChar_Btn.Length; i++)
            {
                m_SelectChar_Btn[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < m_SelectChar_Btn.Length; i++)
            {
                m_SelectChar_Btn[i].interactable = true;
            }
        }
    }
    public void OnClick_Start()
    {
        Debug.Log("GameStart");
        //메인씬으로 전환
    }
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    #endregion

    public void RenderCharImage(int _player_id)
    {
        for (int i = 0; i < m_player_slots.Count; i++)
        {
            if (_player_id == m_player_slots[i].ID)
            {
                m_player_slots[i].Render();
            }
        }
    }
    public void RenderCharImage(int _player_id, ECharacterType _type)
    {
        for (int i = 0; i < m_player_slots.Count; i++)
        {
            if (_player_id == m_player_slots[i].ID)
            {
                m_player_slots[i].Render(_type);
            }
        }
    }
    public void RenderReady(int _player_id,bool _ready,bool another)
    {
        //본인 레디에 대한 결과일 때만 버튼 활성화 비활성화 수행.
        if(another==false)
        EnalbleReadyBtn(_ready);
    }
    public void EnableStartBtn(bool _allready)
    {
        if (_allready)
        {
            start_Btn.gameObject.SetActive(true);
            ready_Btn.gameObject.SetActive(false);
            start_Btn.interactable = true;
        }
    }
    public void EnalbleReadyBtn(bool _ready)
    {
       
    }
    public void SettingSlotInfo(PlayerInfo _myplayerinfo, PlayerInfo _another_playerinfo1, PlayerInfo _another_playerinfo2)
    {
        m_player_slots[0].Player = _another_playerinfo1;
        m_player_slots[1].Player = _myplayerinfo;
        m_player_slots[2].Player = _another_playerinfo2;
    }
    
   
   
    public void Controll_CharBtn(int _player_id)
    {
        for(int i = 0; i < m_player_slots.Count; i++)
        {
            if (m_player_slots[i].ID==_player_id)                                          
            {
                switch (m_player_slots[i].Player.GetCharacterInfo.CharacterType)                        
                {
                    case ECharacterType.Defense:
                        m_SelectChar_Btn[0].interactable = false;
                        break;

                    case ECharacterType.Attack:
                        m_SelectChar_Btn[1].interactable = false;
                        break;

                    case ECharacterType.Support:
                        m_SelectChar_Btn[2].interactable = false;
                        break;
                }

            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < m_player_slots.Count; i++)
        {
            m_player_slots[i].__Initialize();
        }
        MapPannel.SetActive(false);
        on_Ready = false;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        start_Btn.interactable = on_Ready;
    }
}
