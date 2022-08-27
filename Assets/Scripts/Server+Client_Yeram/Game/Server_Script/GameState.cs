using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;

namespace Net
{
    public class GameState : Net.IState
    {
        private Net.NetSession m_client;
        private IState.State m_state;
        public NetSession Client 
        {
            get => m_client;
            set => m_client = value;
        }

        public IState.State ClientState =>
            m_state;

        public GameState(Net.NetSession _session)
        {
            m_client = _session;
            m_state = IState.State.Game;
        }
        public void OnChaged()
        {
            
        }

        public void RecvComplete(RecvPacket _recvpacket)
        {
           
        }

        public void SendComplete()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
