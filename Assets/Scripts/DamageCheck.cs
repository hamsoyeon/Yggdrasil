using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCheck : MonoBehaviour
{

	
	private CharacterClass PlayerClass;
	private CharacterClass EnemyClass;

	private int minDamage;
	private int resultDamage;


	//1.Player 2.Enemy
	public int who; 

	private float time;

	public float Dot;


	// Start is called before the first frame update
	void Start()
    {
		Collider[] cols = Physics.OverlapSphere(transform.position, 10f);

		if (cols.Length > 0)
		{

			for (int i = 0; i < cols.Length; i++)
			{
				switch(who)
				{
					case 1: 
						if(cols[i].tag =="Boss" || cols[i].tag == "Mob")
						{
							Debug.Log("적 이펙트 충돌");

							EnemyClass = cols[i].GetComponent<CharacterClass>();
							PlayerClass = GameObject.Find("Player").GetComponent<CharacterClass>();

							if(cols[i].tag =="Boss")
							{
								minDamage = (int)PlayerClass.m_CharacterStat.Atk - (int)EnemyClass.m_BossStatData.Def;

								//최소 데미지 보정.
								if (minDamage <= 0)
									minDamage = 1;

								resultDamage = minDamage * (int)GameObject.Find("Player").GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;


								EnemyClass.m_BossStatData.HP -= resultDamage;
								//해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
								cols[i].GetComponent<BossFSM>().Damage();
							}
							

						

						}

						break;

					case 2:
						if (cols[i].tag == "Player")
						{
							Debug.Log("플레이어 이펙트 충돌");
							PlayerClass = cols[i].GetComponent<CharacterClass>();
							EnemyClass = GameObject.Find("Boss").GetComponent<CharacterClass>();


							minDamage = (int)EnemyClass.m_BossStatData.Atk - (int)PlayerClass.m_CharacterStat.Def;

							//최소 데미지 보정.
							if (minDamage <= 0)
								minDamage = 1;

							resultDamage = minDamage * (int)EnemyClass.m_SkillMgr.m_BossSkill.m_CurrentBossSkill.Power;

							PlayerClass.m_CharacterStat.HP -= resultDamage;
							//해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
							cols[i].GetComponent<PlayerManager>().Damage();
						}
						break;

				}

			}
		}
		else
		{
			Debug.Log("범위내 오브젝트 없음.");
		}
	}

	// Update is called once per frame
	void Update()
    {

		time += Time.deltaTime;

		if(time >= Dot)
		{
			time = 0f;
			Collider[] cols = Physics.OverlapSphere(transform.position, 10f);

			if (cols.Length > 0)
			{
				for (int i = 0; i < cols.Length; i++)
				{

					switch (who)
					{
						case 1:
							if (cols[i].tag == "Boss" || cols[i].tag == "Mob")
							{
								Debug.Log("적 이펙트 충돌");

								EnemyClass = cols[i].GetComponent<CharacterClass>();
								PlayerClass = GameObject.Find("Player").GetComponent<CharacterClass>();

								if (cols[i].tag == "Boss")
								{
									minDamage = (int)PlayerClass.m_CharacterStat.Atk - (int)EnemyClass.m_BossStatData.Def;

									//최소 데미지 보정.
									if (minDamage <= 0)
										minDamage = 1;

									resultDamage = minDamage * (int)GameObject.Find("Player").GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;


									EnemyClass.m_BossStatData.HP -= resultDamage;
									//해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
									cols[i].GetComponent<BossFSM>().Damage();
								}
							}
							break;
						case 2:
							if (cols[i].tag == "Player")
							{
								Debug.Log("플레이어 이펙트 충돌");
								PlayerClass = cols[i].GetComponent<CharacterClass>();
								EnemyClass = GameObject.Find("Boss").GetComponent<CharacterClass>();


								minDamage = (int)EnemyClass.m_BossStatData.Atk - (int)PlayerClass.m_CharacterStat.Def;

								//최소 데미지 보정.
								if (minDamage <= 0)
									minDamage = 1;

								resultDamage = minDamage * (int)EnemyClass.m_SkillMgr.m_BossSkill.m_CurrentBossSkill.Power;

								PlayerClass.m_CharacterStat.HP -= resultDamage;
								//해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
								cols[i].GetComponent<PlayerManager>().Damage();
							}
							break;
					}



				}
			}
			else
			{
				Debug.Log("범위내 오브젝트 없음.");
			}
			
		}
	}

}


