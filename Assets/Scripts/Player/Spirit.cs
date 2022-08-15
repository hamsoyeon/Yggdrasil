using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spirit : MonoBehaviour
{

	enum SpiritType { Tile=1, NonTileNotMove, NonTileMove }

	//public Spirit_TableExcel m_CurrentSpirit;
	public GameObject SpiritPrefab;

	//public SpiritSkill_TableExcel m_CurrentSkill;
	public SpiritSkill m_SpiritSkill;


	private int m_PlayerRow;
	private int m_PlayerColumn;

	public CharacterClass m_SpiritClass;


	public void SpiritSummon(int index)
	{
		//현재 플레이어가 사용하는 정령을 찾는다.
		foreach (var spirit in DataTableManager.Instance.GetDataTable<Spirit_TableExcelLoader>().DataList)
		{


            
			if (spirit.SpritTableIndex == index)
			{
				m_SpiritClass.m_SpiritData = spirit;
				break;
			}
		}

		//현재 정령이 사용하는 스킬을 찾는다.
		foreach (var skill in DataTableManager.Instance.GetDataTable<SpiritSkill_TableExcelLoader>().DataList)
		{
			if (skill.SpritSkillIndex == m_SpiritClass.m_SpiritData.Skill1Code)
			{
				m_SpiritClass.m_SpiritSkillData = skill;
				break;
			}
		}

       

        switch ((SpiritType)m_SpiritClass.m_SpiritData.SpiritType)
		{
			case SpiritType.Tile:
				m_PlayerRow = MainManager.Instance.GetStageManager().m_PlayerRow;
				m_PlayerColumn = MainManager.Instance.GetStageManager().m_PlayerCoulmn;
				if(MainManager.Instance.GetStageManager().m_MapInfo[m_PlayerRow, m_PlayerColumn].Spirit)
				{
					Debug.Log("해당 타일에는 이미 정령이 소환되어 있어서 타일형 정령을 소환할 수 없음.");
				}
				else
				{
					
					StartCoroutine(TileSpirit(m_SpiritClass.m_SpiritData, m_SpiritClass.m_SpiritSkillData, m_PlayerRow,m_PlayerColumn));
				}
				
				break;
			case SpiritType.NonTileNotMove:
				StartCoroutine(NonTileSpiritByNotMove(m_SpiritClass.m_SpiritData, m_SpiritClass.m_SpiritSkillData));
				break;
			case SpiritType.NonTileMove:
				StartCoroutine(NonTileSpiritByMove(m_SpiritClass.m_SpiritData, m_SpiritClass.m_SpiritSkillData));
				break;

		}

	}

	IEnumerator TileSpirit(Spirit_TableExcel spiritInfo,SpiritSkill_TableExcel skillInfo,int Row,int Column)
	{

		//정령을 소환한다.(코루틴으로 정령을 소환)
		GameObject tempSpirit = Instantiate(SpiritPrefab);

		//스킬실행
		//PlayerManager.Instance.m_SpiritSkill.SkillUse(skillInfo, Row, Column);
		//m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, m_PlayerRow, m_PlayerColumn);

        m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, tempSpirit);


        float spirit_time = 0f;
		float attack_time = 0f;

		MainManager.Instance.GetStageManager().m_MapInfo[Row, Column].Spirit = true;
		tempSpirit.transform.position = MainManager.Instance.GetStageManager().m_MapInfo[Row, Column].MapPos + new Vector3(0,5f,0);

       
		while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= spiritInfo.Duration)
			{
				//정령 파괴후 코루틴 종료
				Object.Destroy(tempSpirit);
				MainManager.Instance.GetStageManager().m_MapInfo[Row, Column].Spirit = false;
				yield break;
			}

			if(attack_time> spiritInfo.Atk_Speed)
			{
				//공격속도를 넘어서면 스킬을 한번더 발생.
				attack_time = 0f;
				//스킬을 발생하는 코드 -> 정령소환하고 바로 스킬발생 그 이후 공격속도를 넘어서면 발동.
				//m_SpiritSkill.SkillUse(skillInfo, Row, Column);
			}

			yield return null;
		}

	}

	IEnumerator NonTileSpiritByNotMove(Spirit_TableExcel spiritInfo, SpiritSkill_TableExcel skillInfo)
	{
		//정령을 소환한다.(코루틴으로 정령을 소환)
		GameObject tempSpirit = Instantiate(SpiritPrefab);

		tempSpirit.transform.position = this.gameObject.transform.position + new Vector3(0,0,-7f);
		//PlayerManager.Instance.m_SpiritSkill.SkillUse(skillInfo, tempSpirit);
		m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, tempSpirit);

		float spirit_time = 0f;
		float attack_time = 0f;

		while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= spiritInfo.Duration)
			{
				//정령 파괴후 코루틴 종료
				Object.Destroy(tempSpirit);
				yield break;
			}

			if (attack_time > spiritInfo.Atk_Speed)
			{
				//공격속도를 넘어서면 스킬을 한번더 발생.
				attack_time = 0f;
				//스킬을 발생하는 코드 -> 정령소환하고 바로 스킬발생 그 이후 공격속도를 넘어서면 발동.
			}
			yield return null;
		}

	}

	IEnumerator NonTileSpiritByMove(Spirit_TableExcel spiritInfo, SpiritSkill_TableExcel skillInfo)
	{
		//정령을 소환한다.(코루틴으로 정령을 소환)
		GameObject tempSpirit = Instantiate(SpiritPrefab);
		tempSpirit.transform.position = this.gameObject.transform.position + new Vector3(0, 0, -7f);
		m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, tempSpirit);
		//PlayerManager.Instance.m_SpiritSkill.SkillUse(skillInfo, tempSpirit);


		float spirit_time = 0f;
		float attack_time = 0f;

		while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= spiritInfo.Duration)
			{
				//정령 파괴후 코루틴 종료
				Object.Destroy(tempSpirit);
				yield break;
			}

			if (attack_time > spiritInfo.Atk_Speed)
			{
				//공격속도를 넘어서면 스킬을 한번더 발생.
				attack_time = 0f;
				//스킬을 발생하는 코드 -> 정령소환하고 바로 스킬발생 그 이후 공격속도를 넘어서면 발동.
			}


			tempSpirit.transform.position = Vector3.MoveTowards(tempSpirit.transform.position, this.gameObject.transform.position, spiritInfo.Move_Speed);

			yield return null;
		}
	}




	private void Start()
	{
		//임시 프리팹을 가져옴.
		//SpiritPrefab = Resources.Load("Prefabs/Spirit") as GameObject;

		//m_SpiritSkill = PlayerManager.Instance.m_SpiritSkill;

		m_SpiritSkill = this.GetComponent<SpiritSkill>();
		m_SpiritClass = this.GetComponent<CharacterClass>();
		m_SpiritClass.m_SkillMgr.m_SpiritSkill = m_SpiritSkill;

	}







	


}
