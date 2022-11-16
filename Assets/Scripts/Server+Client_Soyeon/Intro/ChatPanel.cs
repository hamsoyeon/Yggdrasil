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
        public enum INPUTFIELD_TYPE
        {
            CHAT_INPUTFIELD,
        }

        public enum BTN_TYPE
        {
            SENDCHAT_BTN,
        }

        Dictionary<INPUTFIELD_TYPE, InputField> m_inputFieldDic = new Dictionary<INPUTFIELD_TYPE, InputField>();
        Dictionary<BTN_TYPE, Button> m_btnDic = new Dictionary<BTN_TYPE, Button>();

        public Text m_chatfield;

        void Start()
        {
            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            foreach (Transform child in allchildren)
            {
                switch (child.gameObject.name)
                {
                    case "chat button": // 채팅 보내기
                        child.GetComponent<Button>().onClick.AddListener(Click_SendChatMsg);
                        break;
                    case "chat input field":
                        m_inputFieldDic.Add(INPUTFIELD_TYPE.CHAT_INPUTFIELD, child.GetComponent<InputField>());
                        break; 
                    case "chat field panel":
                        break;
                }
            }
        }

        int m_field_cnt = 0;

        public void UpdateField(string _msg)
        {
            m_chatfield.text += _msg + "\r\n";
            m_field_cnt++;
            if (m_field_cnt >= 12)
            {
                m_chatfield.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    m_chatfield.GetComponent<RectTransform>().rect.height + 20);
            }
        }

        public void Click_SendChatMsg()
        {
            uint protocol = 0;

            if (this.transform.parent.name == "stage window")
            {
                ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)StageMgr.SUB_PROTOCOL.CHAT);
                ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)StageMgr.DETAIL_PROTOCOL.ALL_MSG);
            }

            m_inputFieldDic.TryGetValue(
                INPUTFIELD_TYPE.CHAT_INPUTFIELD
                , out InputField chat_inputField);

            if (chat_inputField.text == "")
            {
                return;
            }

            t_Eve eve = new t_Eve();

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                chat_inputField.text
                );

            NetMgr.Instance.m_sendQue.Enqueue(eve);
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

