using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using EMainProtocol = Net.Protocol.EMainProtocol;
using EProtocolType = Net.Protocol.EProtocolType;
using ESubProtocol = _GameManager.ESubProtocol;
using EDetailProtocol = _GameManager.EDetailProtocol;
public class MapManager : Singleton_Ver2.Singleton<MapManager>
{
    [SerializeField]
    LineRenderer lineprefeb;
    [SerializeField]
    Transform parent;
    Vector3 startpos = new Vector3(-30, 0, 0);
    List<LineRenderer> lines;
    List<Hex> hexs;
    float radius = 17;
    float offset_1 = 0f;
    float offset_2 = 17f;
    #region Init
    private void CreateMap()
    {
        for (int i = 0; i < 5; i++)
        {
            int number = 8;
            for (int j = 0; j < number; j++)
            {
                float offset = i % 2 == 0 ? offset_1 : offset_2;
                Vector3 temppos = new Vector3(startpos.x + (j * radius * 1.8f) - offset, 0f, startpos.z - (i * radius * 1.6f));
                Hex hex = new Hex(temppos, radius);
                hex.CreateVertex();
                hexs.Add(hex);
            }
        }
    }
    #endregion
    #region send func
    public void InitRequest()
    {
        Net.Protocol protocol = new Net.Protocol();
        protocol.SetProtocol((uint)EMainProtocol.INIT, EProtocolType.Main);
        protocol.SetProtocol((uint)ESubProtocol.Object, EProtocolType.Sub);
        protocol.SetProtocol((uint)EDetailProtocol.Tile, EProtocolType.Detail);

        int count = hexs.Count;
        int size = 0;
        Net.NetVector netvector = new Net.NetVector();

        Net.SendPacket sendpacket = new Net.SendPacket();
        sendpacket.__Initialize();
        size += sendpacket.Write(count);
        size += sendpacket.Write(Hex.Radius);
        foreach (var hex in hexs)
        {
            netvector.Vector = hex.SenterPos;
            size += sendpacket.Write(netvector);
        }
        sendpacket.WriteProtocol(protocol.GetProtocol());
        sendpacket.WriteTotalSize(size);
        Net.NetWorkManager.Instance.Send(sendpacket);
    }
    #endregion
    #region recv func
    public void TestTileResult(Net.RecvPacket _recvpacket)
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
        int suc = 0;
        int count = startpos_s.Count;
        for (int i = 0; i < count; i++)
        {
            Vector3 pos;
            pos.x= startpos_s[i].x;
            pos.y = startpos_s[i].y;
            pos.z = startpos_s[i].z;
         
            foreach (var tile in hexs)
            {
                if (tile.SenterPos == pos)
                {
                    DrawLine(tile);
                    ChageColor_Hex(suc++, Color.red);
                    break;
                }
              
            }
        }

    }
    public void InitResult(Net.RecvPacket _recvpacket)
    {

    }
    #endregion

    #region Draw Tile
    public void DrawLine(Hex _hex)
    {
        Vector3[] vertexs = _hex.GetVertex;
        LineRenderer item = GameObject.Instantiate<LineRenderer>(lineprefeb, parent);
        int linecount = 0;
        for (int i = 0; i < vertexs.Length; i++)
        {
            AddLine(ref linecount, ref item);
            if (i == vertexs.Length - 1)
            {
                item.SetPosition(linecount - 1, vertexs[i]);
                AddLine(ref linecount, ref item);
                item.SetPosition(linecount - 1, vertexs[0]);
                break;
            }
            item.SetPosition(linecount - 1, vertexs[i]);
            AddLine(ref linecount, ref item);
            item.SetPosition(linecount - 1, vertexs[i + 1]);
        }
        lines.Add(item);
    }
    private void AddLine(ref int _linecount, ref LineRenderer _line)
    {
        _linecount++;
        _line.positionCount = _linecount;
    }
    private void ChageColor_Hex(int _index, Color _color)
    {
        lines[_index].startColor = _color;
        lines[_index].endColor = _color;
    }
    public void InObjectCheck(Vector3 _objpos)
    {
        bool check = false;
        for (int i = 0; i < hexs.Count; i++)
        {
            check = hexs[i].InHex(_objpos);
            if (check == true)
            {
                ChageColor_Hex(i, Color.red);
            }
            else
            {
                ChageColor_Hex(i, Color.green);
            }
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        lines = new List<LineRenderer>();
        hexs = new List<Hex>();
        CreateMap();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
