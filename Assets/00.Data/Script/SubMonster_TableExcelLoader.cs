using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubMonster_TableExcel
{
	public int No;
	public string Name_KR;
	public string Name_EN;
	public int SubMonsterIndex;
	public int HP;
	public int Defense;
	public int Demege;
	public int Speed;
	public int Prefb;
	public int Baseskill;
	public int Epicskill;
}



/*====================================*/

[CreateAssetMenu(fileName="SubMonster_TableLoader", menuName= "Scriptable Object/SubMonster_TableLoader")]
public class SubMonster_TableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\SubMonster_Table.txt";
	public List<SubMonster_TableExcel> DataList;

	private SubMonster_TableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		SubMonster_TableExcel data = new SubMonster_TableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.No = int.Parse(strs[idx++]);
		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.SubMonsterIndex = int.Parse(strs[idx++]);
		data.HP = int.Parse(strs[idx++]);
		data.Defense = int.Parse(strs[idx++]);
		data.Demege = int.Parse(strs[idx++]);
		data.Speed = int.Parse(strs[idx++]);
		data.Prefb = int.Parse(strs[idx++]);
		data.Baseskill = int.Parse(strs[idx++]);
		data.Epicskill = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<SubMonster_TableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			SubMonster_TableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
