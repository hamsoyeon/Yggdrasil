using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PlayerInfo : Net.NetObjectInfo
{
    //id는 상위 클래스 id 인데 그 id는 로그인 정보 id가 아니라
    //방에서 관리할 player에 관한 id 값.
    private string m_nick;
    private CharacterInfo m_character_info;
    private bool is_ready;
    #region property
    public string GetNick
    {
        get => m_nick;
    }
    public CharacterInfo GetCharacterInfo
    {
        get => m_character_info;
    }
    public bool GetReady
    {
        get => is_ready;
    }
    #endregion
    public PlayerInfo() : base(ENetObjectType.Player)
    {
    
    }
    override public int Deserialize(MemoryStream _stream)
    {
        int size = 0;
        size += base.Deserialize(_stream);
        size += Net.StreamReadWriter.ReadFromStream(_stream, out m_nick);
        size += Net.StreamReadWriter.ReadFromStreamSerialize(_stream, out m_character_info);
        size += Net.StreamReadWriter.ReadFromStream(_stream, out is_ready);
       
        return size;
    }

    override public int Serialize(MemoryStream _stream)
    {
        int size = 0;
        size += base.Serialize(_stream);
        return size;
    }
}

