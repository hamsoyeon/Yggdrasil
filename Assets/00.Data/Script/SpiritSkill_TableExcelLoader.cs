using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpiritSkill_TableExcel
{
	public int No;
	public string Name_KR;
	public string Name_EN;
	public int SpiritSkillIndex;
	public float Power;
	public float Range;
	public float CoolTime;
	public int SkillType;
	public float BulletSpeed;
	public float TargetNum;
	public float LifeTime;
	public float DoT;
	public int SkillAdded;
	public int BuffAdded;
	public int LunchPrefb;
	public int FirePrefb;
	public int DamPrefb;
}



/*====================================*/

[CreateAssetMenu(fileName="SpiritSkill_TableLoader", menuName= "Scriptable Object/SpiritSkill_TableLoader")]
public class SpiritSkill_TableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\SpiritSkill_Table.txt";
	public List<SpiritSkill_TableExcel> DataList;

	private SpiritSkill_TableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		SpiritSkill_TableExcel data = new SpiritSkill_TableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.No = int.Parse(strs[idx++]);
		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.SpiritSkillIndex = int.Parse(strs[idx++]);
		data.Power = float.Parse(strs[idx++]);
		data.Range = float.Parse(strs[idx++]);
		data.CoolTime = float.Parse(strs[idx++]);
		data.SkillType = int.Parse(strs[idx++]);
		data.BulletSpeed = float.Parse(strs[idx++]);
		data.TargetNum = float.Parse(strs[idx++]);
		data.LifeTime = float.Parse(strs[idx++]);
		data.DoT = float.Parse(strs[idx++]);
		data.SkillAdded = int.Parse(strs[idx++]);
		data.BuffAdded = int.Parse(strs[idx++]);
		data.LunchPrefb = int.Parse(strs[idx++]);
		data.FirePrefb = int.Parse(strs[idx++]);
		data.DamPrefb = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<SpiritSkill_TableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			SpiritSkill_TableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
