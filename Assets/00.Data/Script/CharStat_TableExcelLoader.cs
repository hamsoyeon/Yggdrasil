using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharStat_TableExcel
{
	public int No;
	public string Name_KR;
	public string Name_EN;
	public int CharIndex;
	public int position;
	public int Atk;
	public int Def;
	public int HP;
	public int MoveSpeed;
	public int Skill1;
	public int Skill2;
	public int Skill3;
	public int Skill4;
	public int Skill5;
	public int Skill6;
	public int Item1;
	public int Item2;
	public int Sight;
}



/*====================================*/

[CreateAssetMenu(fileName="CharStat_TableLoader", menuName= "Scriptable Object/CharStat_TableLoader")]
public class CharStat_TableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\CharStat_Table.txt";
	public List<CharStat_TableExcel> DataList;

	private CharStat_TableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		CharStat_TableExcel data = new CharStat_TableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.No = int.Parse(strs[idx++]);
		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.CharIndex = int.Parse(strs[idx++]);
		data.position = int.Parse(strs[idx++]);
		data.Atk = int.Parse(strs[idx++]);
		data.Def = int.Parse(strs[idx++]);
		data.HP = int.Parse(strs[idx++]);
		data.MoveSpeed = int.Parse(strs[idx++]);
		data.Skill1 = int.Parse(strs[idx++]);
		data.Skill2 = int.Parse(strs[idx++]);
		data.Skill3 = int.Parse(strs[idx++]);
		data.Skill4 = int.Parse(strs[idx++]);
		data.Skill5 = int.Parse(strs[idx++]);
		data.Skill6 = int.Parse(strs[idx++]);
		data.Item1 = int.Parse(strs[idx++]);
		data.Item2 = int.Parse(strs[idx++]);
		data.Sight = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<CharStat_TableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			CharStat_TableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
