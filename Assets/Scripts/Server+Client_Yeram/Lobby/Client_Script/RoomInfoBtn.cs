using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RoomInfoBtn : MonoBehaviour
{
    private TMP_Text m_roomindex;
    private TMP_Text m_title;
    private TMP_Text m_enterinfo;
    private Image m_image;
    private Button m_btn;
    RoomOutInfo m_roominfo;


    public void ChageInfo(RoomOutInfo _room_info)
    {
        m_roominfo = _room_info;
        //mode -> image chage
        m_roomindex.text = m_roominfo.GetID.ToString();
        m_title.text = m_roominfo.GetTitle;
        m_enterinfo.text = m_roominfo.GetCurCount.ToString() + "/" + m_roominfo.GetMaxEnterCount.ToString();
    }
    public void OnClick_RoomBtn()
    {
        LobbyGUIManager.Instance.OnClick_Room(m_roominfo.GetID);
    }
    private void Start()
    {
        m_roomindex=this.transform.Find("RoomIndex").GetComponent<TMP_Text>();
        m_title = this.transform.Find("Title").GetComponent<TMP_Text>();
        m_enterinfo = this.transform.Find("EnterInfo").GetComponent<TMP_Text>();
        m_btn = this.gameObject.GetComponent<Button>();
        m_btn.onClick.AddListener(OnClick_RoomBtn);
    }

}
