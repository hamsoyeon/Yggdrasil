using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace test_client_unity
{
    public class Player : MonoBehaviour
    {
        public Vector3 m_pos = new Vector3();

        public static int m_t = 0;

        void Start()
        {
            m_pos = this.transform.position;

            test_async();
        }

        async void test_async()
        {
            t_Eve eve = new t_Eve();
            eve.eveList = new List<int>();
            eve.buf = null;

            while (true)
            {
                Move(ref eve);

                await Task.Delay(200);

                if (eve.eveList.Count > 0)
                {
                    uint protocol = 0;

                    ProtocolMgr.Instance.AddSubProtocol(ref protocol, (uint)InGameMgr.SUB_PROTOCOL.PLAYER);
                    ProtocolMgr.Instance.AddDetailProtocol(ref protocol, (uint)InGameMgr.DETAIL_PROTOCOL.MOVE);
                    eve.buf_size = InGameMgr.Instance.PackPacket(ref eve.buf, (int)protocol, eve.eveList.ToArray());

                    NetMgr.Instance.m_sendQue.Enqueue(eve);
                    Debug.Log(eve.eveList.Count);
                }

                eve.eveList.Clear();
            }
        }

        void Move(ref t_Eve _eve)
        {
            if (Input.GetKey(KeyCode.UpArrow)
                && !_eve.eveList.Contains((int)InGameState.SEND_EVENT.UP))
            {
                _eve.eveList.Add((int)InGameState.SEND_EVENT.UP);
            }
            if (Input.GetKey(KeyCode.DownArrow)
                && !_eve.eveList.Contains((int)InGameState.SEND_EVENT.DOWN))
            {
                _eve.eveList.Add((int)InGameState.SEND_EVENT.DOWN);
            }
            if (Input.GetKey(KeyCode.RightArrow)
                && !_eve.eveList.Contains((int)InGameState.SEND_EVENT.RIGHT))
            {
                _eve.eveList.Add((int)InGameState.SEND_EVENT.RIGHT);
            }
            if (Input.GetKey(KeyCode.LeftArrow)
                && !_eve.eveList.Contains((int)InGameState.SEND_EVENT.LEFT))
            {
                _eve.eveList.Add((int)InGameState.SEND_EVENT.LEFT);
            }
        }
    }
}