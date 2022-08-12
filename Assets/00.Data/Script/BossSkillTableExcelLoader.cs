using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BossSkillTableExcel
{
	public string Name_KR;
	public string Name_EN;
	public int BossSkillIndex;
	public int TargetType;
	public int Target;
	public float Power;
	public float CoolTime;
	public float SkillDistance;
	public float SkillRange;
	public float SkillType;
	public float UseStat;
	public bool Direction1;
	public bool Direction2;
	public bool Direction3;
	public bool Direction4;
	public bool Direction5;
	public bool Direction6;
	public bool 정령;
	public float LifeTime;
	public float DoT;
	public int SkillAdded;
	public int BuffAdded;
	public int SkillAnimation;
	public float SkillDelay;
	public int DelayPrefb;
	public int LunchPrefb;
	public int FirePrefb;
	public int DamPrefb;
}



/*====================================*/

[CreateAssetMenu(fileName="BossSkillTableLoader", menuName= "Scriptable Object/BossSkillTableLoader")]
public class BossSkillTableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\BossSkillTable.txt";
	public List<BossSkillTableExcel> DataList;

	private BossSkillTableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		BossSkillTableExcel data = new BossSkillTableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.BossSkillIndex = int.Parse(strs[idx++]);
		data.TargetType = int.Parse(strs[idx++]);
		data.Target = int.Parse(strs[idx++]);
		data.Power = float.Parse(strs[idx++]);
		data.CoolTime = float.Parse(strs[idx++]);
		data.SkillDistance = float.Parse(strs[idx++]);
		data.SkillRange = float.Parse(strs[idx++]);
		data.SkillType = float.Parse(strs[idx++]);
		data.UseStat = float.Parse(strs[idx++]);
		data.Direction1 = bool.Parse(strs[idx++]);
		data.Direction2 = bool.Parse(strs[idx++]);
		data.Direction3 = bool.Parse(strs[idx++]);
		data.Direction4 = bool.Parse(strs[idx++]);
		data.Direction5 = bool.Parse(strs[idx++]);
		data.Direction6 = bool.Parse(strs[idx++]);
		data.정령 = bool.Parse(strs[idx++]);
		data.LifeTime = float.Parse(strs[idx++]);
		data.DoT = float.Parse(strs[idx++]);
		data.SkillAdded = int.Parse(strs[idx++]);
		data.BuffAdded = int.Parse(strs[idx++]);
		data.SkillAnimation = int.Parse(strs[idx++]);
		data.SkillDelay = float.Parse(strs[idx++]);
		data.DelayPrefb = int.Parse(strs[idx++]);
		data.LunchPrefb = int.Parse(strs[idx++]);
		data.FirePrefb = int.Parse(strs[idx++]);
		data.DamPrefb = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<BossSkillTableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			BossSkillTableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
