using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_client_unity
{
    public class StageState : State
    {
        public enum RECV_EVENT
        {
            STAGE_UPDATE,
        }
        public override void Recv(Byte[] _buf, Byte[] _protocol)
        {
            t_Eve teve = new t_Eve();
            teve.buf = new Byte[4096];
            teve.state_type = (int)STATE_TYPE.STAGE_STATE;

            Array.Copy(_buf, teve.buf, _buf.Length);

            int sub_protocol = NetMgr.Instance.m_netWork.GetSubProtocol(_protocol);
            int detail_prototocol = NetMgr.Instance.m_netWork.GetDetailProtocol(_protocol);

            switch (sub_protocol) // 프로토콜에 따라서 언패킹
            {
                case (int)StageMgr.SUB_PROTOCOL.STAGE_RESULTL:
                    {
                        switch (detail_prototocol)
                        {
                            case (int)StageMgr.SERVER_DETAIL_PROTOCOL.STAGE_UPDATE_RESULT:
                                teve.eve = (int)RECV_EVENT.STAGE_UPDATE;
                                break;
                        }
                    }
                    break;
            }

            NetMgr.Instance.m_recvQue.Enqueue(teve);
        }

        public override void RecvEvent(t_Eve _eve)
        {
            switch (_eve.eve)
            {
                case (int)RECV_EVENT.STAGE_UPDATE:
                    {
                        // 받으면 stage에 현재방에 있는 플레이어의 이름이 뜰 수 있게 한다.
                        string name = null;
                        StageMgr.Instance.Unpackpacket(_eve.buf, ref name);
                        Debug.Log(name);
                    }
                    break;
            }
        }

        public override void Send(Byte[] _buf, int _protocol)
        {
            switch (_protocol)
            {

            }
        }

        public override void SendEvent(t_Eve _eve)
        {
            // 이벤트 발생시 send
            switch (_eve.eve)
            {

            }
        }
    }
}