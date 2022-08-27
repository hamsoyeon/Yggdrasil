using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class LobbyMgr : Singleton<LobbyMgr>
    {
        #region protocol
        public enum SUB_PROTOCOL
        {
            LOBBY,
            CHAT,
            ROOM,

            LOBBY_RESULTL,
            CHAT_RESULT,
            ROOM_RESULT,
        }
        public enum DETAIL_PROTOCOL
        {
            // LOBBY
            MULTI = 1,
            SINGLE = 2,
            ENTER_LOBBY = 4,
            EXIT_LOBBY = 8,

            // CHAT
            ALL_MSG = 1,
            NOTICE_MSG = 2, 

            // ROOM
            ALL_ROOM = 1,
            PAGE_ROOM = 2,
            MAKE_ROOM = 4,
            ROOMLIST_UPDATE = 8,

            ENTER_ROOM = 16,
        }

        // 1,2,4,8,16,32,64,128
        public enum SERVER_DETAIL_PROTOCOL
        {
            // ROOM_RESULT
            ROOMLIST_UPDATE_SUCCESS = 1,
            ROOMLIST_UPDATE_FAIL = 2,
            MAKE_ROOM_SUCCESS = 4,
            MAKE_ROOM_FAIL = 8,

            OVER_PLAYERS = 16,
            ENTER_ROOM_SUCCESS = 32,

            // CHAT_RESULT
            ALL_MSG_SUCCESS = 1,
            ALL_MSG_FAIL = 2,
        };
        #endregion
   
        public void Enter_MainMenu()
        {
            NetMgr.Instance.m_curState = NetMgr.Instance.m_loginState;

            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LobbyMgr.SUB_PROTOCOL.LOBBY);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)LobbyMgr.DETAIL_PROTOCOL.EXIT_LOBBY);

            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(
                ref eve.buf,
                (int)protocol,
                null,
                0);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        public void UpdateField(string _msg, Text _chatField, ref int _fieldCnt)
        {
            _chatField.GetComponent<Text>().text += _msg + "\r\n";
            _fieldCnt++;
            if (_fieldCnt >= 4)
            {
                _chatField.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    _chatField.GetComponent<RectTransform>().rect.height + 40);
            }
        }

        public void Send_ChatMsg(InputField _inputField)
        {
            if (_inputField.text == "")
            {
                return;
            }

            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)LobbyMgr.SUB_PROTOCOL.CHAT);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)LobbyMgr.DETAIL_PROTOCOL.ALL_MSG);

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                _inputField.text);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        #region packing, unpacking
        public int Packpacket(ref Byte[] _buf, int _protocol, int _data)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int len = 0;

            BitConverter.GetBytes(_data).CopyTo(data_buf, len);
            len = len + sizeof(int);

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }

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

        public void Unpackpacket(Byte[] _buf, ref string _msg)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] size = new byte[4];

            Array.Copy(_buf, len, size, 0, sizeof(int));
            len = len + sizeof(int);

            _msg = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(size));
        }

        public void Unpackpacket(Byte[] _buf, ref string _roomName, ref string _passWord, ref int _roomNum, ref int _roomCnt)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] roomName_size = new Byte[4];
            Byte[] passWord_size = new Byte[4];
            Byte[] roomNum = new Byte[4];
            Byte[] roomCnt = new Byte[4];

            Array.Copy(_buf, len, roomName_size, 0, sizeof(int));
            len = len + sizeof(int);

            _roomName = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(roomName_size));
            len = len + BitConverter.ToInt32(roomName_size);

            Array.Copy(_buf, len, passWord_size, 0, sizeof(int));
            len = len + sizeof(int);

            _passWord = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(passWord_size));
            len = len + BitConverter.ToInt32(passWord_size);

            Array.Copy(_buf, len, roomNum, 0, sizeof(int));
            _roomNum = BitConverter.ToInt32(roomNum);
            len = len + sizeof(int);

            Array.Copy(_buf, len, roomCnt, 0, sizeof(int));
            _roomCnt = BitConverter.ToInt32(roomCnt);
        }

        public void Unpackpacket(Byte[] _buf, ref string _roomName, ref int _roomNum, ref int _roomCnt)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] roomName_size = new Byte[4];
            Byte[] roomNum = new Byte[4];
            Byte[] roomCnt = new Byte[4];

            Array.Copy(_buf, len, roomName_size, 0, sizeof(int));
            len = len + sizeof(int);

            _roomName = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(roomName_size));
            len = len + BitConverter.ToInt32(roomName_size);

            Array.Copy(_buf, len, roomNum, 0, sizeof(int));
            _roomNum = BitConverter.ToInt32(roomNum);
            len = len + sizeof(int);

            Array.Copy(_buf, len, roomCnt, 0, sizeof(int));
            _roomCnt = BitConverter.ToInt32(roomCnt);
        }
        #endregion
    }
}


