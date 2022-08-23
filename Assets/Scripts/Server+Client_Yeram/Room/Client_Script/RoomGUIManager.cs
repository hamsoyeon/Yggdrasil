using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ECharacterType = CharacterInfo.ECharacterType;

public class RoomGUIManager : Singleton_Ver2.Singleton<RoomGUIManager>
{


    public GameObject MapPannel;

    [SerializeField]
    private GameObject selectMap_Content;

    public bool is_Leader;

    public bool on_Ready;

    [SerializeField]
    private Button start_Btn;
    [SerializeField]
    private Button ready_Btn;

    public Button map_Change_Btn;
    [SerializeField]
    private Button p_MapBtn;
    [SerializeField]
    private int map_Count;
    [SerializeField]
    private Sprite[] map_Sprit;
    [SerializeField]
    private Image map_View;
    [SerializeField]
    private Image room_Map_View;
    [SerializeField]
    private int m_MapNum;


    [SerializeField]
    private List<Button> m_SelectMap_Btn;

    [SerializeField]
    private Button[] m_SelectChar_Btn;

    [SerializeField]
    private List<PlayerSlot> m_player_slots;

    [SerializeField]
    private GameObject parent_render_Char;

    [SerializeField]
    private Canvas m_canvas;
    #region chatting object
    [SerializeField]
    private TMP_InputField m_input_chat;
    [SerializeField]
    GameObject m_TextPrefeb;
    [SerializeField]
    Transform m_content;
    #endregion
    
    private int[] test_slot = new int[3];

    MapSlot m_curmap;

    #region ButtonClickEvent
    public void OnClick_ChatSend()
    {
        RoomManager.Instance.ChattingProcess(m_input_chat.text);
    }
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
        RoomManager.Instance.EnterLobbyProcess();
        LobbyGUIManager.Instance.ClearChat();
    }
    public void OnClick_Ready()
    {
        on_Ready = !on_Ready;

        RoomManager.Instance.ReadyProcess(on_Ready);
    }
    public void OnClick_Start()
    {
        Debug.Log("GameStart");
        //선택된 모드 서버에 전송하기.
        RoomManager.Instance.GameStartProcess(m_curmap.Mode);
        //메인씬으로 전환
    }
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    public void OnClick_Decide_Map()
    {
        room_Map_View.sprite = m_SelectMap_Btn[m_MapNum].transform.GetChild(0).GetComponent<Image>().sprite;
        m_curmap = m_SelectMap_Btn[m_MapNum].GetComponent<MapSlot>();
    }
    #endregion

    private PlayerSlot FindSlot(int _player_id)
    {
        for (int i = 0; i < m_player_slots.Count; i++)
        {
            if (_player_id == m_player_slots[i].ID)
            {
                return m_player_slots[i];
            }
        }
        return null;
    }
    public void RenderCharImage(int _player_id)
    {
        PlayerSlot player = FindSlot(_player_id);
        if (player != null)
            player.Render();
    }
    public void RenderCharImage(int _player_id, ECharacterType _type)
    {
        PlayerSlot player = FindSlot(_player_id);
        if (player != null)
            player.Render(_type);
    }
    public void RenderReady(int _player_id, bool _ready, bool another)
    {
        //레디 텍스트 띄우기 수행.
        EnableReadyText(_player_id, _ready);
        if (another == false)
        {
            EnableReadyBtn(_ready);
        }
        //레디한 사람의 직업을 선택불가 상태로 만들기
        EnableCharBtn(_player_id, _ready);
    }

    public void EnableStartBtn(bool _allready)
    {
        if (_allready)
        {
            start_Btn.gameObject.SetActive(true);
            start_Btn.interactable = true;
        }
    }
    public void EnableReadyText(int _player_id, bool _ready)
    {
        PlayerSlot player = FindSlot(_player_id);
        player.Enable_ReadyText(_ready);
    }
    public void EnableReadyBtn(bool _ready)
    {
        ColorBlock colorBlock = ready_Btn.colors;
        if (_ready)
        {
            colorBlock.selectedColor = new Color(0f, 1f, 0f, 1f);
            colorBlock.normalColor = new Color(0f, 1f, 0f, 1f);
            colorBlock.highlightedColor = new Color(0f, 1f, 0f, 1f);
        }
        else
        {
            colorBlock.selectedColor = new Color(1f, 1f, 1f, 1f);
            colorBlock.normalColor = new Color(1f, 1f, 1f, 1f);
            colorBlock.highlightedColor = new Color(1f, 1f, 1f, 1f);
        }
        ready_Btn.colors = colorBlock;

    }
    public void EnableCharBtn(int _player_id, bool _ready)
    {
        PlayerSlot player = FindSlot(_player_id);
        Button button = null;

        if (player != null)
        {
            switch (player.Player.GetCharacterInfo.CharacterType)
            {
                case ECharacterType.Defense:
                    button = m_SelectChar_Btn[0];
                    break;
                case ECharacterType.Attack:
                    button = m_SelectChar_Btn[1];
                    break;
                case ECharacterType.Support:
                    button = m_SelectChar_Btn[2];
                    break;
            }
            if (button != null)
                button.interactable = !_ready;
        }
    }

    public void UpdateChat(string _txt)
    {

        GameObject clone = Instantiate(m_TextPrefeb, m_content);
        clone.GetComponent<TextMeshProUGUI>().text = _txt;
        m_input_chat.text = "";

        if (m_input_chat.isFocused == false)
        {
            m_input_chat.ActivateInputField();
        }
    }
    public void SettingSlotInfo(PlayerInfo _myplayerinfo, PlayerInfo _another_playerinfo1, PlayerInfo _another_playerinfo2)
    {
        m_player_slots[0].Player = _another_playerinfo1;
        m_player_slots[1].Player = _myplayerinfo;
        m_player_slots[2].Player = _another_playerinfo2;
    }

    public void Select_Map_Btn()
    {
        for (int i = 0; i < map_Count; i++)
        {
            int temp = i;
            m_SelectMap_Btn[temp].onClick.AddListener(() => Map_ViewChange(temp));
        }
    }

    public void Map_ViewChange(int _mapNum)
    {
        map_View.sprite = m_SelectMap_Btn[_mapNum].transform.GetChild(0).GetComponent<Image>().sprite;
        m_MapNum = _mapNum;
    }
    public void Init_Map()
    {
        for (int i = 0; i < map_Count; i++)
        {
            m_SelectMap_Btn.Add(Instantiate(p_MapBtn));
            m_SelectMap_Btn[i].transform.parent = selectMap_Content.transform;
            m_SelectMap_Btn[i].transform.GetChild(0).GetComponent<Image>().sprite = map_Sprit[i];
            m_SelectMap_Btn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = map_Sprit[i].name;
            m_SelectMap_Btn[i].gameObject.AddComponent<MapSlot>().__Initialize(i);
        }

    }

    public void EnableMapBtn(bool _flag)
    {
        map_Change_Btn.interactable = _flag;
    }
    private void Start()
    {
        for (int i = 0; i < m_player_slots.Count; i++)
        {
            m_player_slots[i].__Initialize();
        }

        ColorBlock colorBlock;
        for (int i = 0; i < m_SelectChar_Btn.Length; i++)
        {
            Button button = m_SelectChar_Btn[i];
            colorBlock = button.colors;
            colorBlock.disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            button.colors = colorBlock;
        }


        MapPannel.SetActive(false);
        on_Ready = false;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        start_Btn.interactable = on_Ready;
        map_Change_Btn.interactable = false;                       //방장만 방선택가능
        Init_Map();
        Select_Map_Btn();
    }
}
