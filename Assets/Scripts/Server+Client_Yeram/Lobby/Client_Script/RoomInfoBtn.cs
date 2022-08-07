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

    public void ChageInfo(int _id, string _title,int mode,int _enter_count, int _enter_limit)
    {
        //mode -> image chage
        m_roomindex.text = _id.ToString();
        m_title.text = _title;
        m_enterinfo.text = _enter_count.ToString() + "/" + _enter_limit.ToString();
    }
    public void OnClick_RoomBtn()
    {
        //방 정보 전송 후 방 room 입장.
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, false);
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, true);
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
