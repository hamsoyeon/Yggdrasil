using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Spirit_TableExcel
{
	public int No;
	public string Name_KR;
	public string Name_EN;
	public int SpritTableIndex;
	public int SpritClass;
	public int SpiritType;
	public float Atk;
	public float Atk_Speed;
	public float Move_Speed;
	public float Crit_rate;
	public float Crit_Dmg;
	public int ChaceTarget;
	public float Duration;
	public float CoolTime;
	public int Skill1Code;
	public int Prefab;
}



/*====================================*/

[CreateAssetMenu(fileName="Spirit_TableLoader", menuName= "Scriptable Object/Spirit_TableLoader")]
public class Spirit_TableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\Spirit_Table.txt";
	public List<Spirit_TableExcel> DataList;

	private Spirit_TableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		Spirit_TableExcel data = new Spirit_TableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.No = int.Parse(strs[idx++]);
		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.SpritTableIndex = int.Parse(strs[idx++]);
		data.SpritClass = int.Parse(strs[idx++]);
		data.SpiritType = int.Parse(strs[idx++]);
		data.Atk = float.Parse(strs[idx++]);
		data.Atk_Speed = float.Parse(strs[idx++]);
		data.Move_Speed = float.Parse(strs[idx++]);
		data.Crit_rate = float.Parse(strs[idx++]);
		data.Crit_Dmg = float.Parse(strs[idx++]);
		data.ChaceTarget = int.Parse(strs[idx++]);
		data.Duration = float.Parse(strs[idx++]);
		data.CoolTime = float.Parse(strs[idx++]);
		data.Skill1Code = int.Parse(strs[idx++]);
		data.Prefab = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<Spirit_TableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			Spirit_TableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
