using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class MakeRoomPanel : MonoBehaviour
    {
        InputField m_roomNameField;
        InputField m_passwordField;

        void Start()
        {
            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            foreach (Transform child in allchildren)
            {
                switch (child.gameObject.name)
                {
                    case "room name input field":
                        m_roomNameField = child.GetComponent<InputField>();
                        break;
                    case "room password input field":
                        m_passwordField = child.GetComponent<InputField>();
                        break;
                    case "make button":
                        child.GetComponent<Button>().onClick.AddListener(Click_MakeRoomBtn);
                        break;
                    case "close button":
                        child.GetComponent<Button>().onClick.AddListener(Click_CloseRoomBtn);
                        break;
                }
            }
        }
        public void Click_MakeRoomBtn()
        {
            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LobbyMgr.SUB_PROTOCOL.ROOM);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)LobbyMgr.DETAIL_PROTOCOL.MAKE_ROOM);

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                m_roomNameField.text,
                m_passwordField.text);

            NetMgr.Instance.m_sendQue.Enqueue(eve);

            Debug.Log("방 생성 및 들어가기");

            // 이때 방의 이름을 같이 보내주면 될듯.. 흐음..
        }

        public void Click_CloseRoomBtn()
        {
            this.gameObject.SetActive(false);
        }

        #region 패킹 및 언팩
        public int Packpacket(ref Byte[] _buf, int _protocol, string _str1, string _str2)
        {
            _buf = new Byte[4096];
            Byte[] data_buf = new Byte[4096];
            int len = 0;

            int str_size = _str1.Length * 2;
            BitConverter.GetBytes(str_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_str1).CopyTo(data_buf, len);
            len = len + str_size;

            str_size = _str2.Length * 2;
            BitConverter.GetBytes(str_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_str2).CopyTo(data_buf, len);
            len = len + str_size;

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }
        #endregion
    }
}

