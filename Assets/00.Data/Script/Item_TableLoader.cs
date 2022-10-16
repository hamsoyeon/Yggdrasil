using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Item_TableExcel
{
    public string Name_KR; //한글 이름
    public string Name_EN; //영어 이름
    public int ItemIndex; //아이템 인덱스 값
    public int Type; //아이템 타입
    public int BuffType; //아이템 효과 
    public int MaxStack; //최대 소지
    public int EffectArea; //효과 타일 범위
    public float SetTime; //설치 시간
    public float PlaceTime; //지속 시간
    public int EffectTarget; //효과 대상
    public int EffectPrefab; //효과 프리팹
    public int HitEffect; //피격 프리팹
    public int ItemPrefab; //아이템 프리팹
}

[CreateAssetMenu(fileName ="Item_TableLoader", menuName = "Scriptable Object/Item_TableLoader")]
public class Item_TableLoader : ScriptableObject
{
    [SerializeField]
    string filepath = @"Assets\00.Data\Txt\Item_Table.txt";
    public List<Item_TableExcel> DataList;

    private Item_TableExcel Read(string line)
    {
        line = line.TrimStart('\n');

        Item_TableExcel data = new Item_TableExcel();
        int idx = 0;
        string[] strs = line.Split('`');

        data.Name_KR = strs[idx++];
        data.Name_EN = strs[idx++];
        data.ItemIndex = int.Parse(strs[idx++]);
        data.Type = int.Parse(strs[idx++]);
        data.BuffType = int.Parse(strs[idx++]);
        data.MaxStack = int.Parse(strs[idx++]);
        data.EffectArea = int.Parse(strs[idx++]);
        data.SetTime = float.Parse(strs[idx++]);
        data.PlaceTime = float.Parse(strs[idx++]);
        data.EffectTarget = int.Parse(strs[idx++]);
        data.EffectPrefab = int.Parse(strs[idx++]);
        data.HitEffect = int.Parse(strs[idx++]);
        data.ItemPrefab = int.Parse(strs[idx++]);

        return data;
    }
    [ContextMenu("파일 읽기")]
    public void ReadAllFile()
    {
        DataList = new List<Item_TableExcel>();

        string currentpath = System.IO.Directory.GetCurrentDirectory();
        string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath, filepath));
        string[] strs = allText.Split(';');

        foreach (var item in strs)
        {
            if (item.Length < 2)
                continue;
            Item_TableExcel data = Read(item);
            DataList.Add(data);
        }
    }
}
