using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Runtime.InteropServices;

namespace Net
{
    public class NetVector : Net.ISerialize
    {
        Vector3 vec;

        public float x
        {
            get => vec.x;
            set => vec.x = value;
        }
        public float y
        {
            get => vec.y;
            set => vec.y = value;
        }
        public float z
        {
            get => vec.z;
            set => vec.z = value;
        }
    
        public int Deserialize(MemoryStream _stream)
        {
            int size = 0;
            size += Net.StreamReadWriter.ReadFromStream(_stream, out vec.x);
            size += Net.StreamReadWriter.ReadFromStream(_stream, out vec.y);
            size += Net.StreamReadWriter.ReadFromStream(_stream, out vec.z);
            return size;
        }

        public int Serialize(MemoryStream _stream)
        {
            int size = 0;
            size += Net.StreamReadWriter.WriteToStream(_stream, vec.x);
            size += Net.StreamReadWriter.WriteToStream(_stream, vec.y);
            size += Net.StreamReadWriter.WriteToStream(_stream, vec.z);
            return size;
        }

    }
}

