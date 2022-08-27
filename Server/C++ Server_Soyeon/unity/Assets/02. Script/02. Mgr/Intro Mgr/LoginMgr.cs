using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

// Send하는 부분은 여따가 놓기!!!

namespace test_client_unity
{
    public struct LoginInfo
    {
        [SerializeField] public string m_id;
        [SerializeField] public string m_pw;
        [SerializeField] public string m_nick;

        public LoginInfo(string _id, string _pw, string _nick)
        {
            m_id = _id;
            m_pw = _pw;
            m_nick = _nick;
        }
    };

    // 로그인, 가입, 메뉴, 메인메뉴
    public class LoginMgr : Singleton<LoginMgr> // login, join mgr
    {
        #region protocol
        public enum SUB_PROTOCOL
        {
            NONE,
            LOGIN_INFO,
            LOGIN_RESULT,
            JOIN_INFO,
            JOIN_RESULT,
            LOGOUT_INFO,
            LOGOUT_RESULT,
            LOBBY_ENTER,
            LOBBY_RESULT,
            LOGIN = 1,
            JOIN,
            LOGOUT,
            LOBBY,
            MAX
        };
        #endregion

        public void LoginProcess(LoginInfo _info) // inputField의 text를 받아온다.
        {
            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LoginMgr.SUB_PROTOCOL.LOGIN_INFO);

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                _info.m_id,
                _info.m_pw);

            Debug.Log(_info.m_id + ", " + _info.m_pw + " 로그인 시도");

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }
        public void JoinProcess(LoginInfo _info)
        {
            if (_info.m_id == "" ||
                _info.m_pw == "" ||
                _info.m_nick == "")
            {
                return;
            }
            else
            {
                t_Eve eve = new t_Eve();

                uint protocol = 0;
                ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LoginMgr.SUB_PROTOCOL.JOIN_INFO);

                eve.buf_size = Packpacket(
                    ref eve.buf,
                    (int)protocol,
                    _info.m_id,
                    _info.m_pw,
                    _info.m_nick);

                NetMgr.Instance.m_sendQue.Enqueue(eve);
            }
        }

        /// stage 전환
        /// <summary>
        /// 메뉴 입장 버튼 로그인 성공시 호출된다.
        /// </summary>
        public void EnterMainMenu() // 로그인 성공시 메인메뉴로
        {
            Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.INTRO, Change_UI_Mgr.UI_TYPE.MENU);
            Debug.Log("Enter 메인메뉴화면");
        }

        public void EnterLobby() // 멀티 선택시 로비로 입장.
        {
            NetMgr.Instance.m_curState = NetMgr.Instance.m_lobbyState; // 로비스테이트로 바꾸어준다.

            Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.MENU, Change_UI_Mgr.UI_TYPE.LOBBY);

            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LobbyMgr.SUB_PROTOCOL.LOBBY);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)LobbyMgr.DETAIL_PROTOCOL.MULTI);

            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(
                ref eve.buf,
                (int)protocol,
                null,
                0);

            NetMgr.Instance.m_sendQue.Enqueue(eve);

            Debug.Log("로비입장");
        }

        public void OnClick_Logout() // 로그아웃
        {
            Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.MENU, Change_UI_Mgr.UI_TYPE.INTRO);

            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LoginMgr.SUB_PROTOCOL.LOGOUT_INFO);

            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(
                ref eve.buf,
                (int)protocol,
                null,
                0);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
            Debug.Log("로그아웃");
        }

        #region packing, unpacking
        public int Packpacket(ref Byte[] _buf, int _protocol, string _id, string _pw)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int id_size = _id.Length * 2;
            int pw_size = _pw.Length * 2;

            // 전체 데이터 사이즈 / 프로토콜 / 전체 데이터 사이즈 / ID 사이즈 / ID / PW 사이즈 / PW
            int len = 0;

            BitConverter.GetBytes(id_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_id).CopyTo(data_buf, len);
            len = len + id_size;

            BitConverter.GetBytes(pw_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_pw).CopyTo(data_buf, len);
            len = len + pw_size;

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }

        public int Packpacket(ref Byte[] _buf, int _protocol, string _id, string _pw, string _nick)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int id_size = _id.Length * 2;
            int pw_size = _pw.Length * 2;
            int nick_size = _nick.Length * 2;

            // 전체 데이터 사이즈 / 프로토콜 / 데이터 사이즈 / ID 사이즈 / ID / PW 사이즈 / PW / NICK 사이즈 / NICK
            int len = 0;

            BitConverter.GetBytes(id_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_id).CopyTo(data_buf, len);
            len = len + id_size;

            BitConverter.GetBytes(pw_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_pw).CopyTo(data_buf, len);
            len = len + pw_size;

            BitConverter.GetBytes(nick_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_nick).CopyTo(data_buf, len);
            len = len + nick_size;

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }

        public void Unpackpacket(Byte[] _buf, ref int _result, ref string _msg)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] msg_size = new Byte[4];
            Byte[] result = new Byte[4];

            Array.Copy(_buf, len, result, 0, sizeof(int));
            _result = BitConverter.ToInt32(result);
            len = len + sizeof(int);

            Array.Copy(_buf, len, msg_size, 0, sizeof(int));
            len = len + sizeof(int);

            _msg = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(msg_size));
        }

        public void Unpackpacket(Byte[] _buf, ref string _msg)
        {
            // 패킷넘버 / 프로토콜 / 데이터 사이즈
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] msg_size = new Byte[4];

            Array.Copy(_buf, len, msg_size, 0, sizeof(int));
            len = len + sizeof(int);

            _msg = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(msg_size));
        }

        #endregion
    }
}

