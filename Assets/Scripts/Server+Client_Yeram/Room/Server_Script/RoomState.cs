using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
namespace Net
{
    public class RoomState : IState
    {
        private Net.NetSession m_client;
        private IState.State m_state;
        public NetSession Client 
        {
            get => m_client;
            set => m_client = value; 
        }

        public IState.State ClientState => m_state;
        public RoomState(Net.NetSession _session)
        {
            m_client = _session;
            m_state = IState.State.Room;
        }
        public void RecvComplete(RecvPacket _recvpacket)
        {
            uint protocol;
            _recvpacket.Read(out protocol);
            Protocol protocol_manager = new Protocol(Convert.ToUInt32(protocol));
            EMainProtocol main_protocol = (EMainProtocol)protocol_manager.GetProtocol(EProtocolType.Main);
            switch (main_protocol)
            {
                case EMainProtocol.ROOM:
                    RoomManager.Instance.RecvProcess(_recvpacket, protocol_manager);
                    break;
                case EMainProtocol.LOBBY:
                    MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, false);
                    MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, true);
                    m_client.SetState(m_client.m_Lobbystate);
                    break;
                case EMainProtocol.GAME:
                    MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, false);
                    //게임 창 띄우기
                    m_client.SetState(m_client.m_Gamestate);
                    break;
            }
        }
        public void SendComplete()
        {

        }

        public void OnChaged()
        {
            
        }
    }
}