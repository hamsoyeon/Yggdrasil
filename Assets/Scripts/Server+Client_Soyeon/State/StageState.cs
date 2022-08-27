using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class StageState : State
    {
        public enum RECV_EVENT
        {
            UPDATE_STAGE,
            STAGE_OWNER,
            ENTER_INGAME,

            SELECTED_CHAR,

            CHAT_FILED,
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
                case (int)StageMgr.SUB_PROTOCOL.CHAT_RESULT:
                    {
                        switch (detail_prototocol)
                        {
                            case (int)StageMgr.SERVER_DETAIL_PROTOCOL.ALL_MSG_SUCCESS:
                                teve.eve = (int)RECV_EVENT.CHAT_FILED;
                                break;
                            case (int)StageMgr.SERVER_DETAIL_PROTOCOL.ALL_MSG_FAIL:
                                break;
                        }
                    }
                    break;
                case (int)StageMgr.SUB_PROTOCOL.CHARFIELD_RESULTL:
                    {
                        switch (detail_prototocol)
                        {
                            case (int)StageMgr.SERVER_DETAIL_PROTOCOL.CHARFIELD_UPDATE_RESULT:
                                teve.eve = (int)RECV_EVENT.UPDATE_STAGE;
                                break;
                        }
                    }
                    break;
                case (int)StageMgr.SUB_PROTOCOL.CLICKBTN_RESULT:
                    {
                        switch (detail_prototocol)
                        {
                            case (int)StageMgr.SERVER_DETAIL_PROTOCOL.SELECTED_CHARTYPE:
                                teve.eve = (int)RECV_EVENT.SELECTED_CHAR;
                                break;
                        }
                    }
                    break;
                case (int)StageMgr.SUB_PROTOCOL.STAGE_RESULT:
                    {
                        switch (detail_prototocol)
                        {
                            case (int)StageMgr.SERVER_DETAIL_PROTOCOL.ENTER_INGAME:
                                teve.eve = (int)RECV_EVENT.ENTER_INGAME;
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
                case (int)RECV_EVENT.UPDATE_STAGE:
                    {
                        // 이미 선택된 charType도 받아온다.
                        // 받으면 stage에 현재방에 있는 플레이어의 이름이 뜰 수 있게 한다.
                        string[] name = new string[3];
                        int[] charType = new int[3];
                        int arrNum = new int();
                        // 배열의 개수, 닉네임을 가져옴.
                        StageMgr.Instance.Unpackpacket(_eve.buf, ref arrNum, ref name, ref charType);
                        StageMgr.Instance.UpdateStage(arrNum, name, charType);
                    }
                    break;
                case (int)RECV_EVENT.ENTER_INGAME:
                    {
                        NetMgr.Instance.m_curState = NetMgr.Instance.m_ingameState;
                        // ingameScene으로 변경
                    }
                    break;
                case (int)RECV_EVENT.SELECTED_CHAR:
                    {
                        // flag true일 경우, 게임시작
                        // 모든 플레이어가 준비가 되면.. 시작!
                        // 해당 캐릭터의 선택을 비활성화 시킨다.
                        int charType = new int(); bool flag = new bool();
                        StageMgr.Instance.Unpackpacket(_eve.buf, ref charType, ref flag);
                        StageMgr.Instance.Selected_Char(charType);

                        // 방장주인에게 flag ture가 온다.
                        if (flag)
                        {
                            StageMgr.Instance.m_btnDic.TryGetValue(
                                StageMgr.BTN_TYPE.LAYOUT_01_BTN,
                                out Button layout_01_btn);
                            layout_01_btn.transform.Find("text").GetComponent<Text>().text = "start";
                            layout_01_btn.GetComponent<Button>().interactable = true;
                        }
                    }
                    break;
                case (int)RECV_EVENT.CHAT_FILED:
                    {
                        string msg = null;
                        LobbyMgr.Instance.Unpackpacket(_eve.buf, ref msg);
                        WindowMgr.Instance.m_state_dic.TryGetValue(
                            WindowMgr.WINDOW_TYPE.STAGE, out GameObject stage);
                        stage.transform.GetComponentInChildren<ChatPanel>().UpdateField(msg);
                        Debug.Log(msg);
                    }
                    break;
            }
        }
    }
}