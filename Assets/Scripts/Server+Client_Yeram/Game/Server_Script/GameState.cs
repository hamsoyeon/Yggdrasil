using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
using System;
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
            uint protocol = 0;
            _recvpacket.Read(out protocol);
            Protocol protocol_manager = new Protocol(Convert.ToUInt32(protocol));
            switch ((EMainProtocol)protocol_manager.GetProtocol(EProtocolType.Main))
            {
                case EMainProtocol.INIT:
                    _GameManager.Instance.InItResult(_recvpacket,protocol);
                    break;
                case EMainProtocol.GAME:
                    break;
                case EMainProtocol.TEST:
                    _GameManager.Instance.TestResult(_recvpacket);
                    break;
            }
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
