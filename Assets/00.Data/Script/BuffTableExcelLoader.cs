using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuffTableExcel
{
	public int No;
	public string Name_KR;
	public string Name_EN;
	public int BuffsTableIndex;
	public bool CheckBuff;
	public int BuffEffect;
	public bool EffType;
	public float EffAmount;
	public float EffDoT;
	public float EffRate;
	public float EffTime;
	public float EffTurn;
	public int SummonMon;
	public int EffPosApp;
	public int EffAppealPrf;
	public int EffPosDelay;
	public int EffDelayPrf;
	public int EffAdded;
}



/*====================================*/

[CreateAssetMenu(fileName="BuffTableLoader", menuName= "Scriptable Object/BuffTableLoader")]
public class BuffTableExcelLoader :ScriptableObject
{
	[SerializeField] string filepath =@"Assets\00.Data\Txt\BuffTable.txt";
	public List<BuffTableExcel> DataList;

	private BuffTableExcel Read(string line)
	{
		line = line.TrimStart('\n');

		BuffTableExcel data = new BuffTableExcel();
		int idx =0;
		string[] strs= line.Split('`');

		data.No = int.Parse(strs[idx++]);
		data.Name_KR = strs[idx++];
		data.Name_EN = strs[idx++];
		data.BuffsTableIndex = int.Parse(strs[idx++]);
		data.CheckBuff = bool.Parse(strs[idx++]);
		data.BuffEffect = int.Parse(strs[idx++]);
		data.EffType = bool.Parse(strs[idx++]);
		data.EffAmount = float.Parse(strs[idx++]);
		data.EffDoT = float.Parse(strs[idx++]);
		data.EffRate = float.Parse(strs[idx++]);
		data.EffTime = float.Parse(strs[idx++]);
		data.EffTurn = float.Parse(strs[idx++]);
		data.SummonMon = int.Parse(strs[idx++]);
		data.EffPosApp = int.Parse(strs[idx++]);
		data.EffAppealPrf = int.Parse(strs[idx++]);
		data.EffPosDelay = int.Parse(strs[idx++]);
		data.EffDelayPrf = int.Parse(strs[idx++]);
		data.EffAdded = int.Parse(strs[idx++]);

		return data;
	}
	[ContextMenu("파일 읽기")]
	public void ReadAllFile()
	{
		DataList=new List<BuffTableExcel>();

		string currentpath = System.IO.Directory.GetCurrentDirectory();
		string allText = System.IO.File.ReadAllText(System.IO.Path.Combine(currentpath,filepath));
		string[] strs = allText.Split(';');

		foreach (var item in strs)
		{
			if(item.Length<2)
				continue;
			BuffTableExcel data = Read(item);
			DataList.Add(data);
		}
	}
}
