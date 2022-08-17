using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_client_unity
{
    public class InGameState : State
    {
        public enum RECV_EVENT
        {

        }

        public enum SEND_EVENT
        {
            UP = 1, DOWN, RIGHT, LEFT,

            SECTOR_INIT,
        }

        public override void Recv(Byte[] _buf, Byte[] _protocol)
        {
            t_Eve teve = new t_Eve();
            teve.buf = new Byte[4096];
            Array.Copy(_buf, teve.buf, _buf.Length);

            int sub_protocol = NetMgr.Instance.m_netWork.GetSubProtocol(_protocol);
            int detail_protocol = NetMgr.Instance.m_netWork.GetDetailProtocol(_protocol);

            int tmp = new int();

            switch (sub_protocol)
            {
                case (int)InGameMgr.SUB_PROTOCOL.PLAYER:
                    {
                        InGameMgr.Instance.Unpackpacket(_buf, ref tmp); // 잘온당!!
                    }
                    break;
            }
        }

        public override void RecvEvent(t_Eve _eve)
        {
            switch (_eve)
            {

            }
        }
    }
}
