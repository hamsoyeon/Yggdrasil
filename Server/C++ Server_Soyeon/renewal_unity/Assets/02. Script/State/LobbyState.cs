using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class LobbyState : State
    {
        public enum RECV_EVENT
        {
            CREATE_ROOM,
            UPDATE_ROOM,
            CHAT_FILED,
            ENTER_STAGE,
        }

        public override void Recv(Byte[] _buf, Byte[] _protocol)
        {
            int result = new int();
            string msg = null;

            t_Eve teve = new t_Eve();
            teve.buf = new Byte[4096];
            teve.state_type = (int)STATE_TYPE.LOBBY_STATE;

            Array.Copy(_buf, teve.buf, _buf.Length);

            int sub_protocol = NetMgr.Instance.m_netWork.GetSubProtocol(_protocol);
            int detail_prototocol = NetMgr.Instance.m_netWork.GetDetailProtocol(_protocol);

            switch (sub_protocol) // �������ݿ� ���� ����ŷ
            {
                case (int)LobbyMgr.SUB_PROTOCOL.CHAT_RESULT:
                    {
                        switch (detail_prototocol)
                        {
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ALL_MSG_SUCCESS:
                                teve.eve = (int)RECV_EVENT.CHAT_FILED;
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ALL_MSG_FAIL:
                                break;
                        }
                    }
                    break;
                case (int)LobbyMgr.SUB_PROTOCOL.ROOM_RESULT: // �޽����� ������ ä��â�� �ö󰣴�.
                    {
                        switch (detail_prototocol)
                        {
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ROOMLIST_UPDATE_SUCCESS:
                                teve.eve = (int)RECV_EVENT.UPDATE_ROOM;
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ROOMLIST_UPDATE_FAIL:
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.MAKE_ROOM_SUCCESS:
                                /// ������� �����ϸ� �÷��̾�� stage�� �̵��Ѵ�.
                                teve.eve = (int)RECV_EVENT.ENTER_STAGE;
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.MAKE_ROOM_FAIL:
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.OVER_PLAYERS:
                                /// �ο��ʰ�
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ENTER_ROOM_SUCCESS:
                                /// ����� ����
                                teve.eve = (int)RECV_EVENT.ENTER_STAGE;
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
                case (int)RECV_EVENT.UPDATE_ROOM:
                    {
                        string roomName = null; int roomNum = new int(); int roomCnt = new int();
                        LobbyMgr.Instance.Unpackpacket(_eve.buf, ref roomName, ref roomNum, ref roomCnt);
                        RoomMgr.Instance.UpdateRoom(roomName, roomNum, roomCnt);
                    }
                    break;
                case (int)RECV_EVENT.CREATE_ROOM:
                    {
                        // ���ȣ�� �̸�, �ο���
                    }
                    break;
                case (int)RECV_EVENT.CHAT_FILED:
                    {
                        string msg = null;
                        LobbyMgr.Instance.Unpackpacket(_eve.buf, ref msg);
                        LobbyMgr.Instance.UpdateField(msg);
                        Debug.Log(msg);
                    }
                    break;
                case (int)RECV_EVENT.ENTER_STAGE:
                    {
                        WindowMgr.Instance.ChangeState(WindowMgr.WINDOW_TYPE.LOBBY, WindowMgr.WINDOW_TYPE.STAGE);
                        NetMgr.Instance.m_curState = NetMgr.Instance.m_stageState;
                        StageMgr.Instance.UpdateStage();
                        Debug.Log("Stage�� �̵�...");
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
            // �̺�Ʈ �߻��� send
            switch (_eve.eve)
            {

            }
        }
    }
}