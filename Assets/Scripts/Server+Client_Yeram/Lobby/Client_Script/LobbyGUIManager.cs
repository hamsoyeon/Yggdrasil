using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class LobbyGUIManager : Singleton_Ver2.Singleton<LobbyGUIManager>
{
    #region window object
    [SerializeField]
    GameObject m_window_createroom;
    [SerializeField]
    GameObject m_window_enterroom;
    #endregion
    #region input field object

    #endregion
    #region room button object
    [SerializeField]
    private Button m_btn_room_prefeb;
    [ReadOnly]
    private List<Button> m_room_btns;
    private int m_rooms_count; // 한 페이지당 생성 가능한 방 수.
    #endregion
    #region page button object
    Button m_btn_next;
    Button m_btn_pre;
    #endregion
    #region create room input field object
    TMP_InputField m_input_createname;
    TMP_InputField m_input_pw;
    #endregion
    #region create room button object
    Button m_btn_createroom;
    Button m_btn_ok;
    Button m_btn_cancle;
    #endregion
    #region chat object
    [SerializeField]
    Transform m_Content;
    [SerializeField]
    GameObject m_TextPrefeb;
    [SerializeField]
    GameObject lobbyPanel;
    [SerializeField]
    TMP_InputField m_input_chat;
    #endregion
    #region enter room input field object
    TMP_InputField m_input_enter_pw;
    #endregion
    [SerializeField]
    Canvas m_canvas;

    uint m_enter_roomid;



    #region Initialize
    public void __Initialize(int _btncount)
    {
        __Initialize_Btn(_btncount);
        __Initialize_Input();
    }
    public void __Initialize_Btn(int _btncount)
    {
        m_room_btns = new List<Button>();
        m_rooms_count = _btncount;
        Transform parent = m_canvas.transform.GetChild("Room View").GetChild("Content");
        for (int i = 0; i < m_rooms_count; i++)
            m_room_btns.Add(GameObject.Instantiate<Button>(m_btn_room_prefeb, parent));

        #region buttons init
        //m_btn_createroom = m_canvas.transform.GetChild("CreateRoomBtn").GetComponent<Button>();
        //m_next_btn = m_canvas.transform.GetChild("NextPageBtn").GetComponent<Button>();
        //m_pre_btn = m_canvas.transform.GetChild("BeforPageBtn").GetComponent<Button>();
        //m_btn_ok = m_canvas.transform.GetChild("CreateRoom").GetComponentsInChildren<Button>()[0];
        //m_btn_cancle = m_canvas.transform.GetChild("CreateRoom").GetComponentsInChildren<Button>()[1];
        #endregion
    }
    public void __Initialize_Input()
    {
        TMP_InputField[] inputs = m_canvas.transform.GetChild("CreateRoom")
                                                    .GetComponentsInChildren<TMP_InputField>();
        m_input_createname = inputs[0];
        m_input_pw = inputs[1];

        Transform ChatParent = m_canvas.transform.GetChild("Chatting");
        m_input_chat = ChatParent.GetComponentInChildren<TMP_InputField>();

        //enterpw 찾아서 오브젝트 연결하기.
        Transform EnterParent = m_canvas.transform.GetChild("EnterRoom");
        m_input_enter_pw = EnterParent.GetComponentInChildren<TMP_InputField>();
    }
    #endregion
    #region button click event
    public void OnClick_BeforeMenu()
    {
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Menu, true);
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, false);
        LobbyManager.Instance.LobbyLeaveProcess();
        ClearChat();
        ClearInputField();
    }

    public void OnClick_NextPage()
    {
        LobbyManager.Instance.PageReqProcess(true);
    }
    public void OnClick_PrePage()
    {
        LobbyManager.Instance.PageReqProcess(false);
    }
    public void OnClick_CreateRoom()
    {
        m_window_createroom.SetActive(true);
    }
    public void OnClick_CreateRoomOK()
    {
        LobbyManager.Instance.CreateRoomProcess(m_input_createname.text, m_input_pw.text);
        RoomGUIManager.Instance.is_Leader = true;
        RoomGUIManager.Instance.map_Change_Btn.interactable = true;
        m_window_createroom.SetActive(false);
        ClearInputField();
    }
    public void OnClick_CreateRoomCancle()
    {
        m_window_createroom.SetActive(false);
    }
    public void OnClick_ChatSend()
    {
        LobbyManager.Instance.ChattingProcess(m_input_chat.text);
    }
    public void OnClick_Room(uint _roomid)
    {
        ClearInputField();
        m_enter_roomid = _roomid;
        m_window_enterroom.SetActive(true);
    }
    public void OnClick_EnterRoomOK()
    {
        //방 정보 전송 후 방 room 입장.
        RoomManager.Instance.EnterRoomProcess(m_enter_roomid,m_input_enter_pw.text);
        m_window_enterroom.SetActive(false);
    }
    public void OnClick_EnterRommCancle()
    {
        m_window_enterroom.SetActive(false);
    }
    #endregion
    #region page update func
    public void RoomInfoSetting(int _btn_index, RoomOutInfo _room_info)
    {
        m_room_btns[_btn_index].GetComponent<RoomInfoBtn>().ChageInfo(_room_info);

        for (int i = 0; i < m_rooms_count; i++)
        {
            m_room_btns[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <= _btn_index; i++)
        {
            m_room_btns[i].gameObject.SetActive(true);
        }
    }
    #endregion
    #region chat update func
    public void UpdateChat(string _text)
    {
        if (m_input_chat.text.Equals(""))
        {
            return;
        }
        GameObject clone = Instantiate(m_TextPrefeb, m_Content);
        clone.GetComponent<TextMeshProUGUI>().text = _text;
        m_input_chat.text = "";

        if (m_input_chat.isFocused == false)
        {
            m_input_chat.ActivateInputField();
        }
    }
    public void ClearInputField()
    {
        m_input_chat.text = "";
    }

    public void ClearChat()
    {
        Debug.Log("DeleteChat");
        int childCount = m_Content.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            GameObject.DestroyImmediate(m_Content.GetChild(0).gameObject);
        }
    }
    #endregion
}
