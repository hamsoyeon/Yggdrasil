using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ItemTableTableExcel
{
	public string Name_KR;
	public string Name_EN;
	public int ItemTableIndex;
	public int Type;
	public int CallBuff;
	public int MaxStack;
	public int EffectArea;
	public float SetTime;
	public float PlaceTime;
	public int EffectTarget;
	public int EffectPrefeb;
	public int HitPrefeb;
	public int ItemPrefeb;
}



/*====================================*/

[CreateAssetMenu(fileName="ItemTableTableLoader", menuName= "Scriptable Object/ItemTableTableLoader")]
public class ItemTableTableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\ItemTableTable.txt";
	public List<ItemTableTableExcel> DataList;

	private ItemTableTableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		ItemTableTableExcel data = new ItemTableTableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.ItemTableIndex = int.Parse(strs[idx++]);
		data.Type = int.Parse(strs[idx++]);
		data.CallBuff = int.Parse(strs[idx++]);
		data.MaxStack = int.Parse(strs[idx++]);
		data.EffectArea = int.Parse(strs[idx++]);
		data.SetTime = float.Parse(strs[idx++]);
		data.PlaceTime = float.Parse(strs[idx++]);
		data.EffectTarget = int.Parse(strs[idx++]);
		data.EffectPrefeb = int.Parse(strs[idx++]);
		data.HitPrefeb = int.Parse(strs[idx++]);
		data.ItemPrefeb = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<ItemTableTableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			ItemTableTableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
