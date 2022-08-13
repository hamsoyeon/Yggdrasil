using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ��ų�� ����� ���� -> 

public class SpiritSkill : MonoBehaviour
{ 
	public GameObject EffectPrefab;

	private StageManager m_StageMgr;

	public void SkillUse(SpiritSkill_TableExcel skillInfo,GameObject Spirit)  //��Ÿ����
	{

		GameObject tempSpirit = Spirit;
		EffectPrefab.GetComponent<DamageCheck>().Dot = skillInfo.DoT;
		EffectPrefab.GetComponent<DamageCheck>().who = 1;

		//GameObject tempSkillEffect = Instantiate(EffectPrefab);
		switch (skillInfo.SpiritSkillIndex)
		{
			case 10100:  //������
				StartCoroutine(PoisonCloud(skillInfo, tempSpirit));
				break;
			case 10300:  //����
				StartCoroutine(Invincibility(skillInfo, tempSpirit));
				break;
			case 10400:  //�ż�����
				StartCoroutine(Sanctity(skillInfo, tempSpirit));
				break;
			case 10500:  //��
				StartCoroutine(Heal(skillInfo, tempSpirit));
				break;

		}

	}

	public void SkillUse(SpiritSkill_TableExcel skillInfo,int row, int column)  //Ÿ����
	{
		//GameObject tempSkillEffect = Instantiate(EffectPrefab);
		StartCoroutine(SkillWideAction(skillInfo, row, column));
		EffectPrefab.GetComponent<DamageCheck>().Dot = skillInfo.DoT;
		EffectPrefab.GetComponent<DamageCheck>().who = 1;
	}

	IEnumerator Heal(SpiritSkill_TableExcel skill, GameObject spirit)
	{
		GameObject tempEffect = Instantiate(EffectPrefab);
		tempEffect.transform.position = spirit.transform.position;
		Collider[] colls = null;

		float spirit_time = 0f;
		float buff_Time = 0f;

		while (true)
		{
			//���ӽð� üũ
			spirit_time += Time.deltaTime;
			buff_Time += Time.deltaTime;

			//���� ���ӽð��� ����� 
			if (spirit_time >= skill.LifeTime)
			{
				//���� �ı��� �ڷ�ƾ ����
				Object.Destroy(tempEffect);
				yield break;
			}

			if(buff_Time >= skill.DoT)
			{
				buff_Time = 0f;


				colls = Physics.OverlapSphere(spirit.transform.position, skill.Range, 1 << 8);  //9��° ���̾� = Enemy
				foreach (var rangeCollider in colls)
				{
					//�÷��̾� ȸ����Ű�� �ڵ�.

					Debug.Log("�÷��̾� ȸ����");
				}

			}


			yield return null;
		}

	}

	IEnumerator Sanctity(SpiritSkill_TableExcel skill, GameObject spirit)
	{
		GameObject tempEffect = Instantiate(EffectPrefab);
		tempEffect.transform.position = spirit.transform.position;
		Collider[] colls = null;

		float spirit_time = 0f;

		Debug.Log(spirit.transform.position);

		while(true)
		{
			//���ӽð� üũ
			spirit_time += Time.deltaTime;

			//���� ���ӽð��� ����� 
			if (spirit_time >= skill.LifeTime)
			{
				//���� �ı��� �ڷ�ƾ ����
				Object.Destroy(tempEffect);
				yield break;
			}


			colls = Physics.OverlapSphere(spirit.transform.position, skill.Range, 1 << 9);  //9��° ���̾� = Enemy

			if(colls != null)
			{
				foreach (var rangeCollider in colls)
				{

					Debug.Log(rangeCollider);
					//�и��� ��ü�� �и��� ������ ������ �Ÿ�(��)�� ���ϰ�

					//���ɰ� �и��� ��ü������ ������ ���ؼ�
					var heading = rangeCollider.transform.position - spirit.transform.position;
					heading.y = 0f;
					heading *= skill.Range;

					//������ �ƴ϶��
					if (rangeCollider.gameObject.name != "Boss")
					{
						rangeCollider.gameObject.transform.position = Vector3.MoveTowards(rangeCollider.gameObject.transform.position, heading, 5f);
						Debug.Log($"{rangeCollider.ToString()}�� ���� ������ �о���ϴ�.");
					}
				}
			}

		

			yield return null;
		}

		

	}

	IEnumerator Invincibility(SpiritSkill_TableExcel skill, GameObject spirit)
	{

		GameObject tempEffect = Instantiate(EffectPrefab);
		tempEffect.transform.position = spirit.transform.position;


		//All 
		Collider[] colls = null;

		colls = Physics.OverlapSphere(spirit.transform.position, skill.Range, 1 << 8);  
		//8��° ���̾� = Player
		//�������� 


		yield return new WaitForSeconds(skill.LifeTime);

		//���� ���� ���� �ڵ�

		//���� �ı��� �ڷ�ƾ ����
		Object.Destroy(tempEffect);
		yield break;

	}

	
	IEnumerator PoisonCloud(SpiritSkill_TableExcel skill,GameObject spirit)
	{
		
		GameObject nearEnemy = FindNearbyEnemy(spirit, skill.Range);
		GameObject tempEffect = null;


		if (nearEnemy  != null)
		{
			tempEffect = Instantiate(EffectPrefab);
			tempEffect.transform.position = nearEnemy.transform.position + new Vector3(0, 5f, 0);



		}


		float spirit_time = 0f;
		float attack_time = 0f;

		while (true)
		{
			//���ӽð� üũ
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//���� ���ӽð��� ����� 
			if (spirit_time >= skill.LifeTime)
			{
				//���� �ı��� �ڷ�ƾ ����

				if(nearEnemy != null)
				{
					nearEnemy = null;
				}

				if (nearEnemy != null)
					Object.Destroy(tempEffect);

				yield break;
			}

			if (attack_time > skill.DoT)
			{
				//��Ʈ �ð��� ������ ������ ����.
			}

			yield return null;
		}

	}


	IEnumerator SkillWideAction(SpiritSkill_TableExcel skillInfo, int row, int column)
	{
		int Row = row;
		int Column = column;


		Debug.Log("���̵� ��ų ����");
		float range = skillInfo.Range - 1.0f;
		float xRange = skillInfo.Range + range;

		//���� ��ų������ ǥ���� �ִ� �κ�.
		if (range > 0)  //range�� -1���Ѱ� ������ 2���� ���� ���´�. ������ 1�ϰ��� �ش� Ÿ�Ͽ� ����ϸ� �ȴ�.
		{
			int saveRow = Row;
			int saveColumn = 0;

			int checkRow_P;   //���� ���� �Ʒ��ʿ� �ִ� Column��
			int checkRow_M;   //���� ���� ���ʿ� �ִ� Column��
			int checkColumn;  //���� ���� �ٲ� Ÿ���� Column��

			for (float i = 0; i < skillInfo.Range; i += 1.0f)
			{

				checkRow_P = Row + (int)i;
				checkRow_M = Row - (int)i;


				if (checkRow_P % 2 == 0) //024(135����)
					saveColumn++;

				for (float j = 0; j < xRange; j += 1.0f)
				{

					if (i != 0) //������ �ִ� ������ +-1���ξ� �׸�.
					{

						checkColumn = saveColumn + (int)j;

						if (checkColumn < 0 || checkColumn > 5)
							continue;



						if (checkRow_P < 5)
						{
							m_StageMgr.m_MapInfo[checkRow_P, checkColumn].MapObject.GetComponent<MeshRenderer>().material.color = Color.blue;
							m_StageMgr.m_MapInfo[checkRow_P, checkColumn].SpiritEffect = true;
							
						}

						if (checkRow_M >= 0)
						{
							m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.GetComponent<MeshRenderer>().material.color = Color.blue;
							m_StageMgr.m_MapInfo[checkRow_M, checkColumn].SpiritEffect = true;
							
						}

					}
					else  //������ �ִ� ������ �߱׸�.
					{

						checkColumn = Column - (int)range + (int)j;

						if (j == 0)
							saveColumn = checkColumn;   //�������ο��� ù��° Ÿ�� ����ȯ��ġ ����.

						if (checkColumn < 0 || checkColumn > 5)
							continue;

						
						m_StageMgr.m_MapInfo[Row, checkColumn].MapObject.GetComponent<MeshRenderer>().material.color = Color.blue;
						m_StageMgr.m_MapInfo[Row, checkColumn].SpiritEffect = true;
					}


				}

				xRange -= 1.0f;
			}
		}
		else  //range�� 0���ϸ� ��Ÿ��� 1 �ڱ��ڽ��� Ÿ�ϸ� �ش�
		{
			m_StageMgr.m_MapInfo[Row, Column].MapObject.GetComponent<MeshRenderer>().material.color = Color.red;
			m_StageMgr.m_MapInfo[Row, Column].SpiritEffect = true;
			
		}

		//���ð�(��罺ų 0.5f�ʷ� ����)��  ����ϸ� �������� �ٽ� ��������� ������ �� ������ ����Ʈ �����ϰ� ������ ���� ó��.
		yield return new WaitForSeconds(2f);

		for (int i = 0; i < m_StageMgr.mapZ; i++)
		{
			for (int j = 0; j < m_StageMgr.mapX; j++)
			{

				if (m_StageMgr.m_MapInfo[i, j].SpiritEffect)
				{
					m_StageMgr.m_MapInfo[i, j].MapObject.GetComponent<MeshRenderer>().material.color = Color.white;
					GameObject effect = Instantiate(EffectPrefab);
					effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
					m_StageMgr.m_MapInfo[i, j].SpiritEffectObject = effect;
				}
			}
		}

		Debug.Log("����Ʈ ��ȯ");
		//���� ���� ������ ó��.
		yield return new WaitForSeconds(skillInfo.LifeTime);  //�����ð��� ������ ����Ʈ �����

		for (int i = 0; i < m_StageMgr.mapZ; i++)
		{
			for (int j = 0; j < m_StageMgr.mapX; j++)
			{

				if (m_StageMgr.m_MapInfo[i, j].SpiritEffect)
				{
					m_StageMgr.m_MapInfo[i, j].SpiritEffect = false;
					Object.Destroy(m_StageMgr.m_MapInfo[i, j].SpiritEffectObject);
				}
			}
		}


		Debug.Log("���̵� ��ų ����");

		//���轺ų�ִ��� Ȯ���� �ٽ� ��ų����.
		if (skillInfo.SkillAdded != 0)
		{

		}

		yield break;
	}


	GameObject FindNearbyPlayer(GameObject findStartObject, float distance)
	{
		float Dist = 0f;
		float near = 0f;
		GameObject nearEnemy = null;

		//���� ���� ���� ã�´�.
		Collider[] colls = Physics.OverlapSphere(findStartObject.transform.position, distance, 1 << 8);  //8��° ���̾� = Player
		if (colls.Length == 0)
		{
			Debug.Log("������ �÷��̾ �����ϴ�.");
			DestroyObject(findStartObject);
			return null;
		}
		else
		{
			//���� �ִٸ� �� ���� �߿�
			for (int i = 0; i < colls.Length; i++)
			{

				//���ɰ��� �Ÿ��� �����
				Dist = Vector3.Distance(findStartObject.transform.position, colls[i].transform.position);

				if (i == 0)
				{
					near = Dist;
					nearEnemy = colls[i].gameObject;
				}

				//�� �Ÿ��� �۴ٸ� �Ÿ��� �����ϰ� �ش� ������Ʈ�� ����
				if (Dist < near)
				{
					near = Dist;
					nearEnemy = colls[i].gameObject;
				}
			}
			return nearEnemy;
		}

	}


	GameObject FindNearbyEnemy(GameObject findStartObject, float distance)
	{
		float Dist = 0f;
		float near = 0f;
		GameObject nearEnemy = null;

		//���� ���� ���� ã�´�.
		Collider[] colls = Physics.OverlapSphere(findStartObject.transform.position, distance, 1 << 9);  //9��° ���̾� = Enemy


		if (colls.Length == 0)
		{
			Debug.Log("������ ���� �����ϴ�.");
			return null;
		}
		else
		{
			//���� �ִٸ� �� ���� �߿�
			for (int i = 0; i < colls.Length; i++)
			{
				//������ ������� ������ ����
				if(colls[i].gameObject.name == "Boss")
				{
					nearEnemy = colls[i].gameObject;
					break;
				}

				


				//���ɰ��� �Ÿ��� �����
				Dist = Vector3.Distance(findStartObject.transform.position, colls[i].transform.position);

				if (i == 0)
				{
					near = Dist;
					nearEnemy = colls[i].gameObject;
				}

				//�� �Ÿ��� �۴ٸ� �Ÿ��� �����ϰ� �ش� ������Ʈ�� ����
				if (Dist < near)
				{
					near = Dist;
					nearEnemy = colls[i].gameObject;
				}
			}

			return nearEnemy;
		}
	}


	private void Start()
	{

		m_StageMgr = MainManager.Instance.GetStageManager();
	}


}
