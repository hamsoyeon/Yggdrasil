using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EProtocolType = Net.Protocol.EProtocolType;
public class _GameManager : Singleton_Ver2.Singleton<_GameManager>
{
    public enum ESubProtocol
    {
        None,
        Sector,
        Object,
        Max
    }
    public enum EDetailProtocol
    {
        None,
        Tile,
        Player,
        Boss,
        Item,
    }
    #region send func
    public void InitReqest()
    {
        SectorManager.Instance.InitRequest();
    }
    public void TestViewSector(Vector3 _objpos)
    {
        SectorManager.Instance.TestViewSector(_objpos);
    }
    #endregion
    #region recv func
    public void InItResult(Net.RecvPacket _recvpacket, uint _protocol)
    {
        Net.Protocol protocol_manager = new Net.Protocol(Convert.ToUInt32(_protocol));
        uint subprotocol = protocol_manager.GetProtocol(EProtocolType.Sub);
        switch ((ESubProtocol)subprotocol)
        {
            case ESubProtocol.Sector:
                SectorManager.Instance.InitResult(_recvpacket);
                break;
            case ESubProtocol.Object:
                InitObjectResult(_recvpacket, protocol_manager);
                break;
        }
        
    }
    public void TestResult(Net.RecvPacket _recvpacket)
    {
        //SectorManager.Instance.TestViewSectorResult(_recvpacket);
        MapManager.Instance.TestTileResult(_recvpacket);
    }
    
    private void InitObjectResult(Net.RecvPacket _recvpacket,Net.Protocol _protocol)
    {
        uint detailprotocol = _protocol.GetProtocol(EProtocolType.Detail);
        switch((EDetailProtocol)detailprotocol)
        {
            case EDetailProtocol.Tile:
                MapManager.Instance.InitResult(_recvpacket);
                break;
            case EDetailProtocol.Player:
                break;
            case EDetailProtocol.Boss:
                break;
            case EDetailProtocol.Item:
                break;
        }
    }
    #endregion 
}

