using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace test_client_unity
{
    public class StageMgr : Singleton<StageMgr>
    {
        #region protocol
        public enum SUB_PROTOCOL
        {
            STAGE,

            STAGE_RESULTL,
        }
        public enum DETAIL_PROTOCOL
        {
            STAGE_UPDATE = 1,
        }

        // 1,2,4,8,16,32,64,128
        public enum SERVER_DETAIL_PROTOCOL
        {
            STAGE_UPDATE_RESULT = 1,
        };
        #endregion

        public void UpdateStage() // stageupdate 요청
        {
            // 방에 있는 모든 클라이언트의 정보를 받는다.

            t_Eve eve = new t_Eve();

            uint protocol = 0;
            ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)StageMgr.SUB_PROTOCOL.STAGE);
            ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)StageMgr.DETAIL_PROTOCOL.STAGE_UPDATE);

            eve.buf_size = NetMgr.Instance.m_netWork.PackPacket(
                ref eve.buf,
                (int)protocol,
                null,
                0);

            NetMgr.Instance.m_sendQue.Enqueue(eve);

            Debug.Log("UpdateStage");
        }

        #region packing, unpacking
        public int Packpacket(ref Byte[] _buf, int _protocol, string _str)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int msg_size = _str.Length * 2;
            int len = 0;

            BitConverter.GetBytes(msg_size).CopyTo(data_buf, len);
            len = len + sizeof(int);

            Encoding.Unicode.GetBytes(_str).CopyTo(data_buf, len);
            len = len + msg_size;

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }

        public void Unpackpacket(Byte[] _buf, ref string _str)
        {
            int len = sizeof(int) + sizeof(int) + sizeof(int); // 전체 데이터 사이즈 / 패킷넘버 / 데이터 사이즈
            Byte[] size = new byte[4];

            Array.Copy(_buf, len, size, 0, sizeof(int));
            len = len + sizeof(int);

            _str = Encoding.Unicode.GetString(_buf, len, BitConverter.ToInt32(size));
        }
        #endregion
    }
}

