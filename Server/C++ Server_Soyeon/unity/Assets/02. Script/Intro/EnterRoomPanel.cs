using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class EnterRoomPanel : MonoBehaviour
    {
        [SerializeField] InputField PassWordField;
        [SerializeField] Button Ok_Btn;
        [SerializeField] Button Cancel_Btn;

        [HideInInspector]
        public int m_selectedRoomNum { get; set; } // 선택한 방의 번호

        [HideInInspector]
        public string m_selectedRoomPassword { get; set; } // 선택한 방의 비밀번호

        void Start()
        {
            Ok_Btn.onClick.AddListener(OnClick_Ok);
            Cancel_Btn.onClick.AddListener(OnClick_Close);
        }

        public void OnClick_Ok()
        {
            // 비밀번호가 틀리면 아래 코드 무시.
            // 해당 방에 들어가겠다고 서버에 보낸다.

            if(PassWordField.text != m_selectedRoomPassword)
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
        void OnClick_Close()
        {
            this.gameObject.SetActive(false);
        }
    }
}

