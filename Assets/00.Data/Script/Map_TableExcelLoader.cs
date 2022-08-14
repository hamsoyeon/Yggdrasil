using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Map_TableExcel
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

[CreateAssetMenu(fileName="Map_TableLoader", menuName= "Scriptable Object/Map_TableLoader")]
public class Map_TableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\Map_Table.txt";
	public List<Map_TableExcel> DataList;

	private Map_TableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		Map_TableExcel data = new Map_TableExcel();
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
		DataList=new List<Map_TableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			Map_TableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
