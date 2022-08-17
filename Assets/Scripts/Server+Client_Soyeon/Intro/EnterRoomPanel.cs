using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class EnterRoomPanel : MonoBehaviour
    {
        InputField m_passWordField;

        [HideInInspector]
        public int m_selectedRoomNum { get; set; } // 선택한 방의 번호

        [HideInInspector]
        public string m_selectedRoomPassword { get; set; } // 선택한 방의 비밀번호

        void Start()
        {
            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            foreach (Transform child in allchildren)
            {
                switch (child.gameObject.name)
                {
                    case "password input field":
                        m_passWordField = child.GetComponent<InputField>();
                        break;
                    case "ok button":
                        child.GetComponent<Button>().onClick.AddListener(Click_OkBtn);
                        break;
                    case "cancel button":
                        child.GetComponent<Button>().onClick.AddListener(Click_CloseBtn);
                        break;
                }
            }
        }

        public void Click_OkBtn()
        {
            // 비밀번호가 틀리면 아래 코드 무시.
            // 해당 방에 들어가겠다고 서버에 보낸다.

            if(m_passWordField.text != m_selectedRoomPassword)
            {
                Debug.Log("비밀번호가 다름");
                return;
            }

            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LobbyMgr.SUB_PROTOCOL.ROOM);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)LobbyMgr.DETAIL_PROTOCOL.ENTER_ROOM);

            eve.buf_size = LobbyMgr.Instance.Packpacket(
                ref eve.buf,
                (int)protocol,
                m_selectedRoomNum);

            NetMgr.Instance.m_sendQue.Enqueue(eve);

            Debug.Log("방 들어가기");
        }
        void Click_CloseBtn()
        {
            this.gameObject.SetActive(false);
        }
    }
}

