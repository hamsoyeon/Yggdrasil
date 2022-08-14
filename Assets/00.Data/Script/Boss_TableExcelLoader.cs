using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Boss_TableExcel
{
	public string Name_KR;
	public string Name_EN;
	public int BossStatIndex;
	public float Atk;
	public float HP;
	public float Def;
	public float SRange;
	public float CRange;
	public float FRange;
	public float MaxStamina;
	public float Speed;
	public float MoveStUsed;
	public int Skill1;
	public int Skill2;
	public int Skill3;
	public int Skill4;
	public int Prefeb;
}



/*====================================*/

[CreateAssetMenu(fileName="Boss_TableLoader", menuName= "Scriptable Object/Boss_TableLoader")]
public class Boss_TableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\Boss_Table.txt";
	public List<Boss_TableExcel> DataList;

	private Boss_TableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		Boss_TableExcel data = new Boss_TableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.BossStatIndex = int.Parse(strs[idx++]);
		data.Atk = float.Parse(strs[idx++]);
		data.HP = float.Parse(strs[idx++]);
		data.Def = float.Parse(strs[idx++]);
		data.SRange = float.Parse(strs[idx++]);
		data.CRange = float.Parse(strs[idx++]);
		data.FRange = float.Parse(strs[idx++]);
		data.MaxStamina = float.Parse(strs[idx++]);
		data.Speed = float.Parse(strs[idx++]);
		data.MoveStUsed = float.Parse(strs[idx++]);
		data.Skill1 = int.Parse(strs[idx++]);
		data.Skill2 = int.Parse(strs[idx++]);
		data.Skill3 = int.Parse(strs[idx++]);
		data.Skill4 = int.Parse(strs[idx++]);
		data.Prefeb = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<Boss_TableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			Boss_TableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
