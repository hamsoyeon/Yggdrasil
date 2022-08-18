using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RoomInfo : RoomOutInfo
{
    //방장 정보 playerinfo 라는 클래스 만들면 그걸로 저장하기.
    string m_owner;
    //참가자 정보(유저정보,포지션정보)
    List<PlayerInfo> m_playerlist;
  
    #region property
    public string GetOwner
    {
        get => m_owner;
    }
    public List<PlayerInfo> GetPlayersInfo
    {
        get => m_playerlist;
    }
  
    #endregion
    public RoomInfo() : base()
    {
        m_playerlist = new List<PlayerInfo>();
    }
    override public int Deserialize(MemoryStream _stream)
    {
        int size = 0;
        size += base.Deserialize(_stream);
        size += Net.StreamReadWriter.ReadFromStream(_stream, out m_owner);
        for(int i=0;i<GetCurCount;i++)
        {
            PlayerInfo playerInfo = new PlayerInfo();
            size += Net.StreamReadWriter.ReadFromStreamSerialize(_stream,out playerInfo);
            m_playerlist.Add(playerInfo);
        }
        return size;
    }

    override public int Serialize(MemoryStream _stream)
    {
        int size = 0;
        size += base.Serialize(_stream);
        return size;
    }
}


