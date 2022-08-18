using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        }
        public void SendComplete()
        {

        }

        public void OnChaged()
        {
            
        }
    }
}