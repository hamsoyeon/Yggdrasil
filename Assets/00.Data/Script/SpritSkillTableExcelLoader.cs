using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpritSkillTableExcel
{
	public string Name_KR;
	public string Name_EN;
	public int SpritSkillIndex;
	public int Target;
	public float Power;
	public float SkillRange;
	public int SkillType;
	public float BulletSpeed;
	public float TargetNum;
	public float LifeTime;
	public float DoT;
	public int Shapeform;
	public float Cshape1;
	public float Cshape2;
	public int SkillAdded;
	public int BuffAdded;
	public int LunchPrefb;
	public int FirePrefb;
	public int DamPrefb;
}



/*====================================*/

[CreateAssetMenu(fileName="SpritSkillTableLoader", menuName= "Scriptable Object/SpritSkillTableLoader")]
public class SpritSkillTableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\SpritSkillTable.txt";
	public List<SpritSkillTableExcel> DataList;

	private SpritSkillTableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		SpritSkillTableExcel data = new SpritSkillTableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.SpritSkillIndex = int.Parse(strs[idx++]);
		data.Target = int.Parse(strs[idx++]);
		data.Power = float.Parse(strs[idx++]);
		data.SkillRange = float.Parse(strs[idx++]);
		data.SkillType = int.Parse(strs[idx++]);
		data.BulletSpeed = float.Parse(strs[idx++]);
		data.TargetNum = float.Parse(strs[idx++]);
		data.LifeTime = float.Parse(strs[idx++]);
		data.DoT = float.Parse(strs[idx++]);
		data.Shapeform = int.Parse(strs[idx++]);
		data.Cshape1 = float.Parse(strs[idx++]);
		data.Cshape2 = float.Parse(strs[idx++]);
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
		DataList=new List<SpritSkillTableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			SpritSkillTableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
