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
            MAKE_STAGE,
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

            switch (sub_protocol) // 프로토콜에 따라서 언패킹
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
                case (int)LobbyMgr.SUB_PROTOCOL.ROOM_RESULT: // 메시지를 받으면 채팅창에 올라간다.
                    {
                        switch (detail_prototocol)
                        {
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ROOMLIST_UPDATE_SUCCESS:
                                teve.eve = (int)RECV_EVENT.UPDATE_ROOM;
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ROOMLIST_UPDATE_FAIL:
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.MAKE_ROOM_SUCCESS:
                                // 방주인으로 설정
                                // 룸생성에 성공하면 플레이어는 stage로 이동한다.
                                teve.eve = (int)RECV_EVENT.MAKE_STAGE;
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.MAKE_ROOM_FAIL:
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.OVER_PLAYERS:
                                // 인원초과
                                break;
                            case (int)LobbyMgr.SERVER_DETAIL_PROTOCOL.ENTER_ROOM_SUCCESS:
                                // 방들어가기 성공
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
                        string roomName = null; string passWord = null; int roomNum = new int(); int roomCnt = new int();
                        LobbyMgr.Instance.Unpackpacket(_eve.buf, ref roomName, ref passWord, ref roomNum, ref roomCnt);
                        RoomMgr.Instance.AtiveRoom(roomName, passWord, roomNum, roomCnt);
                    }
                    break;
                case (int)RECV_EVENT.CHAT_FILED:
                    {
                        string msg = null;
                        LobbyMgr.Instance.Unpackpacket(_eve.buf, ref msg);
                        GameObject ui = Change_UI_Mgr.Instance.GetUI(Change_UI_Mgr.UI_TYPE.LOBBY);
                        ui.transform.GetComponentInChildren<ChatPanel>().UpdateField(msg);
                        Debug.Log(msg);
                    }
                    break;
                case (int)RECV_EVENT.ENTER_STAGE:
                    {
                        Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.LOBBY, Change_UI_Mgr.UI_TYPE.STAGE);
                        NetMgr.Instance.m_curState = NetMgr.Instance.m_stageState;
                        StageMgr.Instance.Apply_UpdateStage();
                    }
                    break;
                case (int)RECV_EVENT.MAKE_STAGE:
                    {
                        Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.LOBBY, Change_UI_Mgr.UI_TYPE.STAGE);
                        NetMgr.Instance.m_curState = NetMgr.Instance.m_stageState;
                        StageMgr.Instance.Apply_UpdateStage();
                    }
                    break;
            }
        }
        public override void QuitProgram(t_Eve _eve)
        {
        }
    }
}