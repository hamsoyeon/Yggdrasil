using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapTableTableExcel
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

[CreateAssetMenu(fileName="MapTableTableLoader", menuName= "Scriptable Object/MapTableTableLoader")]
public class MapTableTableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\MapTableTable.txt";
	public List<MapTableTableExcel> DataList;

	private MapTableTableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		MapTableTableExcel data = new MapTableTableExcel();
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
		DataList=new List<MapTableTableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			MapTableTableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
