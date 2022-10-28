using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubMonster_TableExcel
{
    public int No;
    public string Name_KR;
    public string Name_EN;
    public int SubMonsetIndex;
    public float HP;
    public float Def;
    public float Atk;
    public float Speed;
    public int Prefab;
    public int BaseSkill;
    public int EpicSkill;
}

[CreateAssetMenu(fileName = "SubMonster_TableLoader", menuName = "Scriptable Object/SubMonster_TableLoader")]
public class SubMonster_Table : ScriptableObject
{
    [SerializeField]
    string filepath = @"Assets\00.Data\Txt\SubMonster_Table.txt";
    public List<SubMonster_TableExcel> DataList;

    private SubMonster_TableExcel Read(string line)
    {
        line = line.TrimStart('\n');

        SubMonster_TableExcel data = new SubMonster_TableExcel();
        int idx = 0;
        string[] strs = line.Split('`');

        data.No = int.Parse(strs[idx++]);
        data.Name_KR = strs[idx++];
        data.Name_EN = strs[idx++];
        data.SubMonsetIndex = int.Parse(strs[idx++]);
        data.HP = float.Parse(strs[idx++]);
        data.Def = float.Parse(strs[idx++]);
        data.Atk = float.Parse(strs[idx++]);
        data.Speed = float.Parse(strs[idx++]);
        data.Prefab = int.Parse(strs[idx++]);
        data.BaseSkill = int.Parse(strs[idx++]);
        data.EpicSkill = int.Parse(strs[idx++]);

        return data;
    }

    [ContextMenu("파일 읽기")]
    public void ReadAllFile()
    {
        DataList = new List<SubMonster_TableExcel>();

        string currentpath = System.IO.Directory.GetCurrentDirectory();
        string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath, filepath));
        string[] strs = allText.Split(';');

        foreach (var item in strs)
        {
            if (item.Length < 2)
                continue;
            SubMonster_TableExcel data = Read(item);
            DataList.Add(data);
        }
    }
}
