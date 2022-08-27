using Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
namespace Net
{
    public class LobbyState : Net.IState
    {
        private Net.NetSession m_client;
        private IState.State m_state;
        public NetSession Client 
        {
            get => m_client;
            set => m_client = value;
        }

        public IState.State ClientState => m_state;

        public LobbyState(Net.NetSession _session)
        {
            m_client = _session;
            m_state = IState.State.Lobby;
        }
        public void RecvComplete(RecvPacket _recvpacket)
        {
            uint protocol;
            _recvpacket.Read(out protocol);
            Protocol protocol_manager = new Protocol(Convert.ToUInt32(protocol));
            EMainProtocol main_protocol = (EMainProtocol)protocol_manager.GetProtocol(EProtocolType.Main);
            switch (main_protocol)
            {
                case EMainProtocol.LOBBY:
                    LobbyManager.Instance.RecvProcess(_recvpacket, protocol_manager);
                    break;
                case EMainProtocol.LOGIN:
                    m_client.SetState(m_client.m_Loginstate);
                    break;
                case EMainProtocol.ROOM:
                    // 결과 리턴해주는 함수 연결하기
                    bool result=RoomManager.Instance.RecvProcess(_recvpacket, protocol_manager);
                    if(result)
                    {
                        m_client.SetState(m_client.m_Roomstate);
                    }
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

