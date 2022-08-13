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
		//���� �÷��̾ ����ϴ� ������ ã�´�.
		foreach (var spirit in DataTableManager.Instance.GetDataTable<Spirit_TableExcelLoader>().DataList)
		{
			if (spirit.Code == index)
			{
				m_SpiritClass.m_SpiritData = spirit;
				break;
			}
		}

		//���� ������ ����ϴ� ��ų�� ã�´�.
		foreach (var skill in DataTableManager.Instance.GetDataTable<SpiritSkill_TableExcelLoader>().DataList)
		{
			if (skill.SpiritSkillIndex == m_SpiritClass.m_SpiritData.Skill1Code)
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
					Debug.Log("�ش� Ÿ�Ͽ��� �̹� ������ ��ȯ�Ǿ� �־ Ÿ���� ������ ��ȯ�� �� ����.");
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

		//������ ��ȯ�Ѵ�.(�ڷ�ƾ���� ������ ��ȯ)
		GameObject tempSpirit = Instantiate(SpiritPrefab);

		//��ų����
		//PlayerManager.Instance.m_SpiritSkill.SkillUse(skillInfo, Row, Column);
		m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, m_PlayerRow, m_PlayerColumn);



		float spirit_time = 0f;
		float attack_time = 0f;

		MainManager.Instance.GetStageManager().m_MapInfo[Row, Column].Spirit = true;
		tempSpirit.transform.position = MainManager.Instance.GetStageManager().m_MapInfo[Row, Column].MapPos + new Vector3(0,5f,0);
		


		while (true)
		{
			//���ӽð� üũ
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//���� ���ӽð��� ����� 
			if (spirit_time >= spiritInfo.Duration)
			{
				//���� �ı��� �ڷ�ƾ ����
				Object.Destroy(tempSpirit);
				MainManager.Instance.GetStageManager().m_MapInfo[Row, Column].Spirit = false;
				yield break;
			}

			if(attack_time> spiritInfo.Atk_Speed)
			{
				//���ݼӵ��� �Ѿ�� ��ų�� �ѹ��� �߻�.
				attack_time = 0f;
				//��ų�� �߻��ϴ� �ڵ� -> ���ɼ�ȯ�ϰ� �ٷ� ��ų�߻� �� ���� ���ݼӵ��� �Ѿ�� �ߵ�.
				//m_SpiritSkill.SkillUse(skillInfo, Row, Column);
			}

			yield return null;
		}

	}

	IEnumerator NonTileSpiritByNotMove(Spirit_TableExcel spiritInfo, SpiritSkill_TableExcel skillInfo)
	{
		//������ ��ȯ�Ѵ�.(�ڷ�ƾ���� ������ ��ȯ)
		GameObject tempSpirit = Instantiate(SpiritPrefab);

		tempSpirit.transform.position = this.gameObject.transform.position + new Vector3(0,0,-7f);
		//PlayerManager.Instance.m_SpiritSkill.SkillUse(skillInfo, tempSpirit);
		m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, tempSpirit);

		float spirit_time = 0f;
		float attack_time = 0f;

		while (true)
		{
			//���ӽð� üũ
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//���� ���ӽð��� ����� 
			if (spirit_time >= spiritInfo.Duration)
			{
				//���� �ı��� �ڷ�ƾ ����
				Object.Destroy(tempSpirit);
				yield break;
			}

			if (attack_time > spiritInfo.Atk_Speed)
			{
				//���ݼӵ��� �Ѿ�� ��ų�� �ѹ��� �߻�.
				attack_time = 0f;
				//��ų�� �߻��ϴ� �ڵ� -> ���ɼ�ȯ�ϰ� �ٷ� ��ų�߻� �� ���� ���ݼӵ��� �Ѿ�� �ߵ�.
			}
			yield return null;
		}

	}

	IEnumerator NonTileSpiritByMove(Spirit_TableExcel spiritInfo, SpiritSkill_TableExcel skillInfo)
	{
		//������ ��ȯ�Ѵ�.(�ڷ�ƾ���� ������ ��ȯ)
		GameObject tempSpirit = Instantiate(SpiritPrefab);
		tempSpirit.transform.position = this.gameObject.transform.position + new Vector3(0, 0, -7f);
		m_SpiritClass.m_SkillMgr.m_SpiritSkill.SkillUse(m_SpiritClass.m_SpiritSkillData, tempSpirit);
		//PlayerManager.Instance.m_SpiritSkill.SkillUse(skillInfo, tempSpirit);


		float spirit_time = 0f;
		float attack_time = 0f;

		while (true)
		{
			//���ӽð� üũ
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//���� ���ӽð��� ����� 
			if (spirit_time >= spiritInfo.Duration)
			{
				//���� �ı��� �ڷ�ƾ ����
				Object.Destroy(tempSpirit);
				yield break;
			}

			if (attack_time > spiritInfo.Atk_Speed)
			{
				//���ݼӵ��� �Ѿ�� ��ų�� �ѹ��� �߻�.
				attack_time = 0f;
				//��ų�� �߻��ϴ� �ڵ� -> ���ɼ�ȯ�ϰ� �ٷ� ��ų�߻� �� ���� ���ݼӵ��� �Ѿ�� �ߵ�.
			}


			tempSpirit.transform.position = Vector3.MoveTowards(tempSpirit.transform.position, this.gameObject.transform.position, spiritInfo.Move_Speed);

			yield return null;
		}
	}




	private void Start()
	{
		//�ӽ� �������� ������.
		//SpiritPrefab = Resources.Load("Prefabs/Spirit") as GameObject;

		//m_SpiritSkill = PlayerManager.Instance.m_SpiritSkill;

		m_SpiritSkill = this.GetComponent<SpiritSkill>();
		m_SpiritClass = this.GetComponent<CharacterClass>();
		m_SpiritClass.m_SkillMgr.m_SpiritSkill = m_SpiritSkill;

	}







	


}
