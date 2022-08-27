using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CharacterInfo : Net.ISerialize
{
    public enum ECharacterType
    {
        None,
        Defense,
        Attack,
        Support,
        Max
    }
    ECharacterType m_character_type;
    #region property
    public ECharacterType CharacterType
    {
        get => m_character_type;
        set => m_character_type = value;
    }
    #endregion
    // 그 외의 캐릭터가 받아야 할 정보들은 나중에 기획서와 클라 보면서 추가.
    public int Deserialize(MemoryStream _stream)
    {
        int type;
        int size = Net.StreamReadWriter.ReadFromStream(_stream, out type);
        m_character_type = (ECharacterType)type;
        return size;
    }

    public int Serialize(MemoryStream _stream)
    {
        int size = 0;
        return size;
    }
}

