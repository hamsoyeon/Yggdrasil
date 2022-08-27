using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class StageMgr : Singleton<StageMgr>
    {
        #region protocol
        public enum SUB_PROTOCOL
        {
            CHARFIELD,
            CLICKBTN,
            SELECTEDMAP,
            CHAT,
            STAGE,

            CHARFIELD_RESULTL,
            CLICKBTN_RESULT,
            SELECTEDMAP_RESULT,
            STAGE_RESULT,
            CHAT_RESULT,
        }
        public enum DETAIL_PROTOCOL
        {
            // STAGE
            CHARFIELD_UPDATE = 1,
            OWNER_CHARFILED = 2,
            STAGE_EXIT = 4,

            // CLICK_BTN
            READY = 1,
            GAME_START = 2,
            SELECT_CHAR = 4,

            // CHAT
            ALL_MSG = 1,
        }

        // 1, 2, 4, 8, 16, 32, 64, 128
        public enum SERVER_DETAIL_PROTOCOL
        {
            // STAGE_RESULTL
            CHARFIELD_UPDATE_RESULT = 1,
            STAGE_OWNER = 2,
            STAGE_EXIT_SUCCESS = 4,

            // CLICKBTN_RESULT
            //START_ACTIVE = 1, //
            SELECTED_CHARTYPE = 2, // 캐릭터 선택을 알림.

            // STAGE_RESULT
            ENTER_INGAME = 1,

            // CHAT_RESULT
            ALL_MSG_SUCCESS = 1,
            ALL_MSG_FAIL = 2,
        };
        #endregion

        #region 채팅
        public void UpdateField(string _msg, Text _chatField, ref int _fieldCnt)
        {
            _chatField.text += _msg + "\r\n";
            _fieldCnt++;
            if (_fieldCnt >= 12)
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
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)SUB_PROTOCOL.CHAT);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)DETAIL_PROTOCOL.ALL_MSG);

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                _inputField.text);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }
        #endregion

        public void Selected_Char(int _type, bool flag)
        {
            StageGUIMgr.Instance.Interactable_Btn(_type, false);

            StageGUIMgr.Instance.m_selected_charTypes.Add(_type);

            // 어떤 캐릭터가 선택되었다는 신호가 왔을때, 선택한 charType을 NON(-1)으로 바꾼다.
            if (StageGUIMgr.Instance.m_selected_charType == _type)
            {
                StageGUIMgr.Instance.m_selected_charType = (int)StageGUIMgr.CHAR_TYPE.NON;
            }

            if (flag)
            {
                Button StartAndReady = StageGUIMgr.Instance.m_StartAndReady_Btn;
                StartAndReady.transform.Find("text").GetComponent<Text>().text = "start";
                StartAndReady.GetComponent<Button>().interactable = true;
            }
        }

        public void Send_Ready()
        {
            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)StageMgr.SUB_PROTOCOL.CLICKBTN);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)StageMgr.DETAIL_PROTOCOL.READY);

            t_Eve eve = new t_Eve();

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                 StageGUIMgr.Instance.m_selected_charType);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        public void Send_Start()
        {
            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)StageMgr.SUB_PROTOCOL.CLICKBTN);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)StageMgr.DETAIL_PROTOCOL.GAME_START);

            t_Eve eve = new t_Eve();

            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(
                ref eve.buf,
                (int)protocol,
                null,
                0);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        public void Apply_UpdateStage()
        {
            // 방에 있는 모든 클라이언트의 정보를 받는다.
            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)StageMgr.SUB_PROTOCOL.CHARFIELD);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)StageMgr.DETAIL_PROTOCOL.CHARFIELD_UPDATE);

            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(
                ref eve.buf,
                (int)protocol,
                null,
                0);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        public void UpdateStage(int _arrNum, string[] _nick, int[] _type)
        {
            for (int i = 0; i < _arrNum; i++)
            {
                CharField[] charFileds = FindObjectsOfType<CharField>();
                charFileds[i].Init(_nick[i]);
            }

            for (int i = 0; i < _type.Length; i++)
            {
                if (_type[i] != (int)StageGUIMgr.CHAR_TYPE.NON)
                {
                    StageGUIMgr.Instance.m_selected_charTypes.Add(_type[i]);
                    StageGUIMgr.Instance.Interactable_Btn(_type[i], false);
                }
            }
        }

        public void ReSetStage(int _arrNum, string[] _nick, int[] _type)
        {
            StageGUIMgr.Instance.m_selected_charTypes.Clear();
            StageGUIMgr.Instance.m_selected_charType = (int)StageGUIMgr.CHAR_TYPE.NON;

            Button StartAndReady = StageGUIMgr.Instance.m_StartAndReady_Btn;
            StartAndReady.transform.Find("text").GetComponent<Text>().text = "ready";
            StartAndReady.interactable = true;

            for (int i = 0; i < _type.Length; i++)
            {
                StageGUIMgr.Instance.Interactable_Btn(i, true);
                CharField[] charFileds = FindObjectsOfType<CharField>();
                charFileds[i].Init(_nick[i]);
            }
        }

        #region packing, unpacking
        public int Packpacket(ref Byte[] _buf, int _protocol, string _str)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int str_size = _str.Length * 2;
            int len = 0;

            BitConverter.GetBytes(str_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_str).CopyTo(data_buf, len);
            len = len + str_size;

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }
        public int Packpacket(ref Byte[] _buf, int _protocol, int _data)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int len = 0;

            BitConverter.GetBytes(_data).CopyTo(data_buf, len);
            len = len + sizeof(int);

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }
        public void Unpackpacket(Byte[] _buf, ref int _data)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] bt = new byte[4];

            Array.Copy(_buf, len, bt, 0, sizeof(int));
            _data = BitConverter.ToInt32(bt);
        }
        public void Unpackpacket(Byte[] _buf, ref bool _flag)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] bt = new byte[4];

            Array.Copy(_buf, len, bt, 0, sizeof(bool));
            _flag = BitConverter.ToBoolean(bt);
        }
        public void Unpackpacket(Byte[] _buf, ref int _data, ref bool _flag)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] bt = new byte[4];

            Array.Copy(_buf, len, bt, 0, sizeof(int));
            _data = BitConverter.ToInt32(bt);
            len = len + sizeof(int);

            Array.Copy(_buf, len, bt, 0, sizeof(bool));
            _flag = BitConverter.ToBoolean(bt);
        }
        public void Unpackpacket(Byte[] _buf, ref string _str)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int); // 전체 데이터 사이즈 / 패킷넘버 / 데이터 사이즈
            Byte[] size = new byte[4];

            Array.Copy(_buf, len, size, 0, sizeof(int));
            len = len + sizeof(int);

            _str = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(size));
        }
        public void Unpackpacket(Byte[] _buf, ref int _arrNum, ref string[] _str, ref int[] _data)
        {
            // 전체 데이터 사이즈 / 패킷넘버 / 데이터 사이즈
            int len = sizeof(int) + sizeof(int) + sizeof(int);
            Byte[] bt = new Byte[4];

            Array.Copy(_buf, len, bt, 0, sizeof(int));
            len = len + sizeof(int);
            _arrNum = BitConverter.ToInt32(bt);

            for (int i = 0; i < _arrNum; i++)
            {
                Array.Copy(_buf, len, bt, 0, sizeof(int));
                len = len + sizeof(int);

                _str[i] = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(bt));
                len = len + BitConverter.ToInt32(bt);

                Array.Copy(_buf, len, bt, 0, sizeof(int));
                _data[i] = BitConverter.ToInt32(bt);
                len = len + sizeof(int);
            }
        }
        #endregion
    }
}

