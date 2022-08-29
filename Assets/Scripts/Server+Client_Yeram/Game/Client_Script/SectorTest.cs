using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorTest : Singleton_Ver2.Singleton<SectorTest>
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
    private void Start()
    {
        lines = new List<LineRenderer>();
        hexs = new List<Hex>();
        CreateMap();
    }
    private void CreateMap()
    {
        for(int i=0;i<5;i++)
        {
            int number = 8;
            for(int j=0;j<number;j++)
            {
                float offset = i % 2 == 0 ? offset_1 : offset_2;
                Vector3 temppos = new Vector3(startpos.x + (j * radius * 1.8f)-offset, 0f, startpos.z - (i * radius*1.6f));
                Hex hex = new Hex(temppos,radius);
                hex.CreateVertex();
                DrawLine(hex);
                hexs.Add(hex);
            }
        }
    }
  
    public void DrawLine(Hex _hex)
    {
        Vector3[] vertexs = _hex.GetVertex;
        LineRenderer item = GameObject.Instantiate<LineRenderer>(lineprefeb, parent);
        int linecount = 0;
        for (int i=0;i<vertexs.Length;i++)
        {
            AddLine(ref linecount,ref item);
            if (i==vertexs.Length-1)
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
    private void AddLine(ref int _linecount,ref LineRenderer _line)
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
        for(int i=0;i<hexs.Count;i++)
        {
            check = hexs[i].InHex(_objpos);
            if (check == true)
            {
                ChageColor_Hex(i,Color.red);
            }
            else
            {
                ChageColor_Hex(i,Color.green);
            }
        }
    }
}
