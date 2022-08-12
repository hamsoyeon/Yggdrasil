using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharStatTableExcel
{
	public string Name_KR;
	public string Name_EN;
	public int CharStatIndex;
	public int position;
	public float Atk;
	public float Def;
	public float HP;
	public float MoveSpeed;
	public int Skill1;
	public int Skill2;
	public int Skill3;
	public int Skill4;
	public int Skill5;
	public int Skill6;
	public int Item1;
	public int Item2;
	public int Sight;
	public int Prefeb;
}



/*====================================*/

[CreateAssetMenu(fileName="CharStatTableLoader", menuName= "Scriptable Object/CharStatTableLoader")]
public class CharStatTableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\CharStatTable.txt";
	public List<CharStatTableExcel> DataList;

	private CharStatTableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		CharStatTableExcel data = new CharStatTableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.CharStatIndex = int.Parse(strs[idx++]);
		data.position = int.Parse(strs[idx++]);
		data.Atk = float.Parse(strs[idx++]);
		data.Def = float.Parse(strs[idx++]);
		data.HP = float.Parse(strs[idx++]);
		data.MoveSpeed = float.Parse(strs[idx++]);
		data.Skill1 = int.Parse(strs[idx++]);
		data.Skill2 = int.Parse(strs[idx++]);
		data.Skill3 = int.Parse(strs[idx++]);
		data.Skill4 = int.Parse(strs[idx++]);
		data.Skill5 = int.Parse(strs[idx++]);
		data.Skill6 = int.Parse(strs[idx++]);
		data.Item1 = int.Parse(strs[idx++]);
		data.Item2 = int.Parse(strs[idx++]);
		data.Sight = int.Parse(strs[idx++]);
		data.Prefeb = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<CharStatTableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			CharStatTableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
