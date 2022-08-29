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

    [SerializeField]
    List<Vector3> startpos;
    public void _Initialize()
    {
        //InitRequest();
    }
    #region send func
    public void InitRequest()
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((uint)EMainProtocol.INIT, EProtocolType.Main);
        protocol.SetProtocol((uint)_GameManager.ESubProtocol.Sector, EProtocolType.Sub);
       
        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        sendpacket.WriteProtocol(protocol.GetProtocol());
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    public void TestViewSector(Vector3 _objpos)
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((uint)EMainProtocol.TEST, EProtocolType.Main);
        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        Net.NetVector netvector = new Net.NetVector();
        netvector.Vector = _objpos;
        int size = sendpacket.Write(netvector);
        sendpacket.WriteProtocol(protocol.GetProtocol());
        sendpacket.WriteTotalSize(size);
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    #endregion
    #region recv func
    #region ver1
    //public void InitResult(Net.RecvPacket _recvpacket)
    //{
    //    int datasize = 0;
    //    Net.NetVector startpos = new Net.NetVector();
    //    Net.NetVector endpos = new Net.NetVector();
    //    int sectorcount = 0;
    //    _recvpacket.Read(out datasize);
    //    _recvpacket.ReadSerialize(out startpos);
    //    _recvpacket.ReadSerialize(out endpos);
    //    _recvpacket.Read(out m_h_distance);
    //    _recvpacket.Read(out m_v_distance);
    //    _recvpacket.Read(out sectorcount);
    //    float start = startpos.x;
    //    //vertical
    //    for (int i = 0; i < sectorcount + 1; i++)
    //    {
    //        LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
    //        item.SetPosition(0, new Vector3(start, startpos.y, startpos.z));
    //        item.SetPosition(1, new Vector3(start, startpos.y, endpos.z));
    //        start += m_h_distance;
    //    }
    //    start = startpos.z;
    //    //horizontal
    //    for (int i = 0; i < sectorcount + 1; i++)
    //    {
    //        LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
    //        item.SetPosition(0, new Vector3(startpos.x, startpos.y, start));
    //        item.SetPosition(1, new Vector3(endpos.x, startpos.y, start));
    //        start -= m_v_distance;
    //    }

    //}
    #endregion
    #region test ver2
    public void InitResult(Net.RecvPacket _recvpacket)
    {
        int datasize = 0;
        List<Net.NetVector> startpos_s = new List<Net.NetVector>();

        int sectorcount = 0;
        _recvpacket.Read(out datasize);
        _recvpacket.Read(out sectorcount);
        Net.NetVector net_distance = new Net.NetVector();
        _recvpacket.ReadSerialize(out net_distance);
        for (int i = 0; i < sectorcount; i++)
        {
            Net.NetVector startpos = new Net.NetVector();
            _recvpacket.ReadSerialize(out startpos);
            startpos_s.Add(startpos);
        }
        startpos = new List<Vector3>();
        //vertical
        for (int i = 0; i < sectorcount; i++)
        {
            Vector3 pos = new Vector3(startpos_s[i].x, startpos_s[i].y, startpos_s[i].z);
            Vector3 distance = new Vector3(net_distance.x, net_distance.y, net_distance.z);
            startpos.Add(pos);
            LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
            int line_count = 0;

            AddLine(ref line_count, ref item);
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.x += distance.x*2;
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.z -= distance.z*2;
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.x -= distance.x*2;
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.z += distance.z*2;
            item.SetPosition(line_count - 1, pos);
        }
    }
    #endregion

    public void TestViewSectorResult(Net.RecvPacket _recvpacket)
    {
        int datasize = 0;
        List<Net.NetVector> startpos_s = new List<Net.NetVector>();

        int sectorcount = 0;
        _recvpacket.Read(out datasize);
        _recvpacket.Read(out sectorcount);
        Net.NetVector net_distance = new Net.NetVector();
        _recvpacket.ReadSerialize(out net_distance);
        for (int i = 0; i < sectorcount; i++)
        {
            Net.NetVector startpos = new Net.NetVector();
            _recvpacket.ReadSerialize(out startpos);
            startpos_s.Add(startpos);
        }
        startpos = new List<Vector3>();
        //vertical
        for (int i = 0; i < sectorcount; i++)
        {
            Vector3 pos = new Vector3(startpos_s[i].x, startpos_s[i].y, startpos_s[i].z);
            Vector3 distance = new Vector3(net_distance.x, net_distance.y, net_distance.z);
            startpos.Add(pos);
            LineRenderer item = GameObject.Instantiate<LineRenderer>(m_line_prefeb, m_parent);
            int line_count = 0;

            item.startColor = Color.red;
            item.endColor = Color.red;
            AddLine(ref line_count, ref item);
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.x += distance.x * 2;
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.z -= distance.z * 2;
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.x -= distance.x * 2;
            item.SetPosition(line_count - 1, pos);
            AddLine(ref line_count, ref item);
            pos.z += distance.z * 2;
            item.SetPosition(line_count - 1, pos);
        }
    }
    #endregion
    private void AddLine(ref int _linecount, ref LineRenderer _line)
    {
        _linecount++;
        _line.positionCount = _linecount;
    }
    void Start()
    {

    }


    void Update()
    {

    }
}
