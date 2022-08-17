using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace test_client_unity
{
    public class InGameMgr : Singleton<InGameMgr>
    {
        public enum SUB_PROTOCOL
        {
            INGAME,
            PLAYER,

            INGAME_RESULT,
        };
        public enum DETAIL_PROTOCOL
        {
            MOVE = 1,
            INIT,
        };
        public enum SERVER_DETAIL_PROTOCOL
        {
            STAGE_ENTER_SUCCESS = 1,
        };

        public void Click_InGameBtn()
        {
            // 초기화
            Debug.Log("ChangeState -> InGameState");
            NetMgr.Instance.m_curState = NetMgr.Instance.m_ingameState;

            t_Eve eve = new t_Eve();
            eve.eveList = new List<int>();
            eve.buf = null;

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)InGameMgr.SUB_PROTOCOL.INGAME);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)InGameMgr.DETAIL_PROTOCOL.INIT);
            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(ref eve.buf, (int)protocol, null, 0);
            eve.eveList.Add((int)InGameState.SEND_EVENT.SECTOR_INIT);
            NetMgr.Instance.m_sendQue.Enqueue(eve);
        }

        #region packing & unpacking
        public int PackPacket(ref Byte[] _buf, string _data)
        {
            int len = 0;
            int msg_size = _data.Length * 2;

            BitConverter.GetBytes(msg_size).CopyTo(_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_data).CopyTo(_buf, len);
            len = len + msg_size;

            return len;
        }
        public int PackPacket(ref Byte[] _buf, int _protocol, int[] _eve)
        {
            Byte[] data_buf = new Byte[4096];
            int len = 0;

            Byte[] eveCnt = BitConverter.GetBytes(_eve.Length);
            Array.Copy(eveCnt, 0, data_buf, len, sizeof(int));
            len = len + sizeof(int);

            foreach (int item in _eve)
            {
                Byte[] tmp_bytes = BitConverter.GetBytes(item);
                Array.Copy(tmp_bytes, 0, data_buf, len, sizeof(int));
                len = len + sizeof(int);
            }

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }
        public void Unpackpacket(Byte[] _buf, ref int _data)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int); // 전체 데이터 사이즈 / 패킷넘버 / 데이터 사이즈
            Byte[] data = new byte[4];

            Array.Copy(_buf, len, data, 0, sizeof(int));
            _data = BitConverter.ToInt32(data);
        }
        #endregion
    }
}
