using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Map_Table2Excel
{
	public int MapTableIndex;
	public string StageName;
	public int StgConcept;
	public int StgHorz;
	public int StgVert;
	public int TileType;
	public int TilePrefeb;
	public int BossSummon;
	public int BuffSummon;
}



/*====================================*/

[CreateAssetMenu(fileName="Map_Table2Loader", menuName= "Scriptable Object/Map_Table2Loader")]
public class Map_Table2ExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\Map_Table2.txt";
	public List<Map_Table2Excel> DataList;

	private Map_Table2Excel Read(string line)
	{
		line = line.TrimStart('\n');

        Map_Table2Excel data = new Map_Table2Excel();
		int idx =0;
		string[] strs= line.Split('`');

		data.MapTableIndex = int.Parse(strs[idx++]);
		data.StageName = strs[idx++];
		data.StgConcept = int.Parse(strs[idx++]);
		data.StgHorz = int.Parse(strs[idx++]);
		data.StgVert = int.Parse(strs[idx++]);
		data.TileType = int.Parse(strs[idx++]);
		data.TilePrefeb = int.Parse(strs[idx++]);
		data.BossSummon = int.Parse(strs[idx++]);
		data.BuffSummon = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<Map_Table2Excel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
            Map_Table2Excel data = Read(item);
			DataList.Add(data);
		}
	}
}
