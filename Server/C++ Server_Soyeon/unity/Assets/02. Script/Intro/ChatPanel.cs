using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class ChatPanel : MonoBehaviour
    {
        public InputField m_chatInputField;
        public Button m_sendChatBtn;
        public Text m_chatfield;

        void Start()
        {
            m_sendChatBtn.onClick.AddListener(OnClick_SendChatMsg);
        }

        int m_field_cnt = 0;

        public void UpdateField(string _msg)
        {
            switch (this.transform.parent.name)
            {
                case "Lobby":
                    LobbyMgr.Instance.UpdateField(_msg, m_chatfield, ref m_field_cnt);
                    break;
                case "Stage":
                    StageMgr.Instance.UpdateField(_msg, m_chatfield, ref m_field_cnt);
                    break;
            }
        }

        public void OnClick_SendChatMsg()
        {
            switch(this.transform.parent.name)
            {
                case "Lobby":
                    LobbyMgr.Instance.Send_ChatMsg(m_chatInputField);
                    break;
                case "Stage":
                    StageMgr.Instance.Send_ChatMsg(m_chatInputField);
                    break;
            }
        }

        #region packing & unpacking
        public int Packpacket(ref Byte[] _buf, int _protocol, string _msg)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int msg_size = _msg.Length * 2;
            int len = 0;

            BitConverter.GetBytes(msg_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_msg).CopyTo(data_buf, len);
            len = len + msg_size;

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }
        #endregion
    }
}

