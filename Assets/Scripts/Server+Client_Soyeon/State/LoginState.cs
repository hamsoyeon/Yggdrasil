using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_client_unity
{
    public class LoginState : State
    {
        public enum EVENT
        {
            LOGIN_SUCESS,
            ENTER_LOBBY,
        }

        public override void Recv(Byte[] _buf, Byte[] _protocol)
        {
            int result = new int();
            string msg = null;

            t_Eve teve = new t_Eve();
            teve.buf = new Byte[4096];
            Array.Copy(_buf, teve.buf, _buf.Length);

            int sub_protocol = NetMgr.Instance.m_netWork.GetSubProtocol(_protocol);

            switch (sub_protocol) // 프로토콜에 따라서 언패킹
            {
                case (int)LoginMgr.SUB_PROTOCOL.LOGIN_RESULT:

                    LoginMgr.Instance.Unpackpacket(_buf, ref result, ref msg);
                    Debug.Log(msg);

                    teve.eve = (int)EVENT.LOGIN_SUCESS;

                    // 여기선 null을 던져서 안된다.. 이유가 뭘까..
                    // 로그인 성공시 메뉴로 들어간다.
                    if (result == 1)
                    {
                        NetMgr.Instance.m_recvQue.Enqueue(teve);
                    }

                    break;
                case (int)LoginMgr.SUB_PROTOCOL.JOIN_RESULT:
                    LoginMgr.Instance.Unpackpacket(_buf, ref result, ref msg);
                    Debug.Log(msg);
                    break;
                case (int)LoginMgr.SUB_PROTOCOL.LOGOUT_RESULT:
                    LoginMgr.Instance.Unpackpacket(_buf, ref msg);
                    Debug.Log(msg);
                    break;
                case (int)LoginMgr.SUB_PROTOCOL.LOBBY_RESULT: // 로비에 입장한다는 패킷을 받고...
                    LoginMgr.Instance.Unpackpacket(_buf, ref msg);

                    // 로비에 입장한다는 이벤트를 켜준다.
                    teve.eve = (int)EVENT.ENTER_LOBBY;
                    NetMgr.Instance.m_recvQue.Enqueue(teve);

                    Debug.Log(msg);
                    break;
            }
        }

        public override void RecvEvent(t_Eve _teve)
        {
            switch (_teve.eve)
            {
                case (int)EVENT.LOGIN_SUCESS:
                    LoginMgr.Instance.EnterMainMenu();
                    break;
                case (int)EVENT.ENTER_LOBBY:
                    LoginMgr.Instance.EnterLobby();
                    break;
            }
        }
    }
}