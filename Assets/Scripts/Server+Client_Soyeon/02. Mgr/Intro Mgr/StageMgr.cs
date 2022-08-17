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
            // - layout_01
            // - layout_02
            // - layout_03

            // - charField
            // - clickBtn
            // - selectedMap

            CHARFIELD,
            CLICKBTN,
            SELECTEDMAP,
            CHAT,

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

        public enum PLAYER_STATE
        {
            OWNER, // 방장
            NORMAL, // 일반
        }

        public enum LAYOUT_TYPE
        {
            LAYOUT_01,
            LAYOUT_02,
            LAYOUT_03,
            LAYOUT_04,
        }

        public enum BTN_TYPE
        {
            // layout_01
            LAYOUT_01_BTN,

            // layout_02
            ATTACK_BTN,
            SHIELD_BTN,
            SUPPORT_BTN,
        }

        [System.Serializable]
        public class Layout_Dic : test_client_unity.SerializableDictionary<LAYOUT_TYPE, GameObject> { }
        public Layout_Dic m_layoutDic = new Layout_Dic();

        public Dictionary<BTN_TYPE, Button> m_btnDic = new Dictionary<BTN_TYPE, Button>();

        int m_selected_charType = new int();
        List<int> m_selected_charTypes = new List<int>();

        void Start()
        {
            // 레이아웃에 존재하는 버튼들의 이벤트를 등록한다.

            // layout_01의 버튼 이벤트 등록.
            m_layoutDic.TryGetValue(LAYOUT_TYPE.LAYOUT_01, out GameObject layout_01);
            layout_01.transform.Find("button").GetComponent<Button>().onClick.AddListener(Click_Layout_01_Btn);
            m_btnDic.Add(BTN_TYPE.LAYOUT_01_BTN, layout_01.transform.Find("button").GetComponent<Button>());

            // layout_02의 버튼 이벤트 등록.
            m_layoutDic.TryGetValue(LAYOUT_TYPE.LAYOUT_02, out GameObject layout_02);
            layout_02.transform.Find("attack type button").GetComponent<Button>()
                .onClick.AddListener(() => Click_CharType((int)BTN_TYPE.ATTACK_BTN));
            m_btnDic.Add(BTN_TYPE.ATTACK_BTN, layout_02.transform.Find("attack type button").GetComponent<Button>());

            layout_02.transform.Find("shield type button").GetComponent<Button>()
                .onClick.AddListener(() => Click_CharType((int)BTN_TYPE.SHIELD_BTN));
            m_btnDic.Add(BTN_TYPE.SHIELD_BTN, layout_02.transform.Find("shield type button").GetComponent<Button>());

            layout_02.transform.Find("support type button").GetComponent<Button>()
                .onClick.AddListener(() => Click_CharType((int)BTN_TYPE.SUPPORT_BTN));
            m_btnDic.Add(BTN_TYPE.SUPPORT_BTN, layout_02.transform.Find("support type button").GetComponent<Button>());
        }

        // 준비버튼
        void Click_Layout_01_Btn()
        {
            m_btnDic.TryGetValue(BTN_TYPE.LAYOUT_01_BTN, out Button layout_01_btn);
            string text = layout_01_btn.transform.Find("text").GetComponent<Text>().text;

            switch (text)
            {
                case "ready":
                    // 선택되지 않은 경우 무시한다.
                    if (m_selected_charType == 0)
                        return;

                    // 준비를 누르면 비활성화 시킨다.
                    layout_01_btn.interactable = false;
                    Send_Ready();
                    break;
                case "start":
                    Send_Start();
                    break;
            }
        }

        public void Selected_Char(int _type)
        {
            Interactable_Btn(_type, false);

            m_selected_charTypes.Add(_type);

            // 어떤 캐릭터가 선택되었다는 신호가 왔을때, 선택한 charType을 NON(0)으로 바꾼다.
            if (m_selected_charType == _type)
            {
                m_selected_charType = 0;
            }
        }

        void Click_CharType(int _type)
        {
            int charType = _type;

            // 해당 캐릭터를 선택
            m_selected_charType = _type;

            m_btnDic.TryGetValue((BTN_TYPE)_type, out Button typeBtn);

            // 해당 charTypeBtn을 비활성화시키고
            // 다른 charTypeBtn을 활성화한다.
            typeBtn.interactable = false;

            switch (_type)
            {
                case (int)BTN_TYPE.ATTACK_BTN:
                    Interactable_Btn((int)BTN_TYPE.SHIELD_BTN, true);
                    Interactable_Btn((int)BTN_TYPE.SUPPORT_BTN, true);
                    break;
                case (int)BTN_TYPE.SHIELD_BTN:
                    Interactable_Btn((int)BTN_TYPE.ATTACK_BTN, true);
                    Interactable_Btn((int)BTN_TYPE.SUPPORT_BTN, true);
                    break;
                case (int)BTN_TYPE.SUPPORT_BTN:
                    Interactable_Btn((int)BTN_TYPE.ATTACK_BTN, true);
                    Interactable_Btn((int)BTN_TYPE.SHIELD_BTN, true);
                    break;
            }

            // 이미 선택된 charType은 비활성화한다.
            foreach (int type in m_selected_charTypes)
            {
                Interactable_Btn(type, false);
            }
        }

        //public void Enabled_Btn(int _type, bool _flag)
        //{
        //    m_btnDic.TryGetValue((BTN_TYPE)_type, out Button btn);
        //    //ColorBlock color = btn.colors; // 색변경
        //    //color.normalColor = Color.gray;
        //    //btn.colors = color;
        //    btn.interactable = false;
        //    btn.enabled = _flag;
        //}

        public void Interactable_Btn(int _type, bool _falg)
        {
            switch (_type)
            {
                case (int)BTN_TYPE.LAYOUT_01_BTN: // 준비버튼
                    m_btnDic.TryGetValue(BTN_TYPE.LAYOUT_01_BTN, out Button layout_01_btn);
                    layout_01_btn.enabled = _falg;
                    break;
                case (int)BTN_TYPE.ATTACK_BTN:
                    m_btnDic.TryGetValue(BTN_TYPE.ATTACK_BTN, out Button attack_btn);
                    //ColorBlock color = attack_btn.colors; // 색변경
                    //color.normalColor = Color.gray;
                    //attack_btn.colors = color;
                    attack_btn.interactable = _falg;
                    break;
                case (int)BTN_TYPE.SHIELD_BTN:
                    m_btnDic.TryGetValue(BTN_TYPE.SHIELD_BTN, out Button shield_btn);
                    shield_btn.interactable = _falg;
                    break;
                case (int)BTN_TYPE.SUPPORT_BTN:
                    m_btnDic.TryGetValue(BTN_TYPE.SUPPORT_BTN, out Button support_btn);
                    support_btn.interactable = _falg;
                    break;
            }
        }

        void Send_Ready()
        {
            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)StageMgr.SUB_PROTOCOL.CLICKBTN);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)StageMgr.DETAIL_PROTOCOL.READY);

            t_Eve eve = new t_Eve();

            eve.buf_size = Packpacket(
                ref eve.buf,
                (int)protocol,
                m_selected_charType);

            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        void Send_Start()
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

            Debug.Log("UpdateStage");
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
                if (_type[i] != 0)
                {
                    m_selected_charTypes.Add(_type[i]);
                    Interactable_Btn(_type[i], false);
                }
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

