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
        ready_Btn.gameObject.SetActive(!on_Ready);
        RoomManager.Instance.ReadyProcess(on_Ready);
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
    public void EnableStartBtn(bool _allready)
    {
        if (_allready)
        {
            start_Btn.interactable = true;
            //레디버튼 비활성화
            //스타트버튼 활성화
        }
        else
        {
            start_Btn.interactable = false;
        }
    }
    public void SettingSlotInfo(PlayerInfo _myplayerinfo, PlayerInfo _another_playerinfo1, PlayerInfo _another_playerinfo2)
    {
        m_player_slots[0].Player = _another_playerinfo1;
        m_player_slots[1].Player = _myplayerinfo;
        m_player_slots[2].Player = _another_playerinfo2;
    }

    private void Start()
    {
        for(int i=0;i<m_player_slots.Count;i++)
        {
            m_player_slots[i].__Initialize();
        }
        MapPannel.SetActive(false);
        on_Ready = false;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        start_Btn.interactable = on_Ready;
    }
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
}
