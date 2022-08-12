using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
public class SectorManager : Singleton_Ver2.Singleton<SectorManager>
{
    [SerializeField]
    Transform m_parent;
    [SerializeField]
    LineRenderer m_line_prefeb;

    //private const float m_distance = 15.5f;

    //private List<LineRenderer> m_lines;

    //void Start()
    //{
    //    m_start_x = -45;
    //    m_end_x = 195;
    //    m_start_z = 38;
    //    m_end_z = -142;

    //    float startpos = m_start_x;
    //    //vertical
    //    for (int i = 0; i < 8 * 2; i++)
    //    {
    //        LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
    //        item.SetPosition(0, new Vector3(startpos, m_pos_y, m_start_z));
    //        item.SetPosition(1, new Vector3(startpos, m_pos_y, m_end_z));
    //        startpos += m_distance;
    //    }
    //    startpos = m_start_z;
    //    //horizontal
    //    for (int i = 0; i < 6 * 2; i++)
    //    {
    //        LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
    //        item.SetPosition(0, new Vector3(m_start_x, m_pos_y, startpos));
    //        item.SetPosition(1, new Vector3(m_end_x, m_pos_y, startpos));
    //        startpos -= m_distance;
    //    }

    //}

    private  float m_h_distance;
    private  float m_v_distance;
    private List<LineRenderer> m_lines;

    private void _Initialize()
    {
        InitRequest();
    }
    #region send func
    private void InitRequest()
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((uint)EMainProtocol.TEST, EProtocolType.Main);
        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        sendpacket.WriteProtocol(protocol.GetProtocol());
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    #endregion
    #region recv func
    public void InitResult(Net.RecvPacket _recvpacket)
    {
        int datasize = 0;
        Net.NetVector startpos = new Net.NetVector();
        Net.NetVector endpos = new Net.NetVector();
        int sectorcount = 0;
        _recvpacket.Read(out datasize);
        _recvpacket.ReadSerialize(out startpos);
        _recvpacket.ReadSerialize(out endpos);
        _recvpacket.Read(out m_h_distance);
        _recvpacket.Read(out m_v_distance);
        _recvpacket.Read(out sectorcount);
        float start = startpos.x;
        //vertical
        for (int i = 0; i < sectorcount + 1; i++)
        {
            LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
            item.SetPosition(0, new Vector3(start, startpos.y, startpos.z));
            item.SetPosition(1, new Vector3(start, startpos.y, endpos.z));
            start += m_h_distance;
        }
        start = startpos.z;
        //horizontal
        for (int i = 0; i < sectorcount + 1; i++)
        {
            LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
            item.SetPosition(0, new Vector3(startpos.x, startpos.y, start));
            item.SetPosition(1, new Vector3(endpos.x, startpos.y, start));
            start -= m_v_distance;
        }

    }
    #endregion
    void Start()
    {
        _Initialize();
    }

    
    void Update()
    {
        
    }
}
