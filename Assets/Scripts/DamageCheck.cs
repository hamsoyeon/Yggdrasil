using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCheck : MonoBehaviour
{
    public CharacterClass EnemyClass;
    public bool dmg_check = false;
    public GameObject m_DamageEffect;  //해당 이펙트의 피격된 적의 PosBody에 생성.

    private CharacterClass PlayerClass;

    private Buff BuffClass;

    private Transform PosBody;

	private float minDamage;
	private float resultDamage;
    public float power;

	//1.Player 2.Enemy
	public int who; 

	private float time;

	public float Dot;

    public int buffIndex;         //Dot시간이 될때마다 Buff.cs파일의 Buff()혹은 DeBuff()를 실행시켜서 버프/디버프의 추가 or 지속시간 갱신을 시킨다.

    void Start()
    {
        if (dmg_check)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 15.5f);
            if (cols.Length > 0)
            {

                for (int i = 0; i < cols.Length; i++)
                {
                    switch (who)
                    {

                        case 1:
                            if (cols[i].tag == "Boss" || cols[i].tag == "Mob")
                            {
                                EnemyClass = cols[i].GetComponent<CharacterClass>();
                                //PlayerClass = GameObject.Find("Player").GetComponent<CharacterClass>();
                                PlayerClass = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<CharacterClass>();

                                if (cols[i].tag == "Boss")
                                {
                                    minDamage = PlayerClass.m_CharacterStat.Atk - EnemyClass.m_BossStatData.Def;

                                    //최소 데미지 보정.
                                    if (minDamage <= 0)
                                        minDamage = 1;

                                    //power = (int)GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;

                                    resultDamage = (minDamage * power) * EnemyClass.Invincibility;

                                    EnemyClass.m_BossStatData.HP -= resultDamage;
                                    //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                                    cols[i].GetComponent<BossFSM>().Damage((int)resultDamage);

                                    Transform[] allChildren = cols[i].GetComponentsInChildren<Transform>();
                                    foreach (Transform child in allChildren)
                                    {
                                        if (child.name == "PosBody")
                                        {
                                            PosBody = child;
                                        }
                                    }
                                    Instantiate(m_DamageEffect, PosBody);

                                }
                                if (cols[i].CompareTag("Mob"))
                                {
                                    minDamage = (int)PlayerClass.m_CharacterStat.Atk - EnemyClass.m_SubMonsterData.Defense;

                                    //최소 데미지 보정.
                                    if (minDamage <= 0)
                                        minDamage = 1;

                                    power = (int)GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;

                                    resultDamage = minDamage * power;

                                    cols[i].transform.GetComponent<Enemy>().TakeDamage((int)resultDamage);
                                }
                            }
                            break;

                        case 2:
                            if (cols[i].tag == "Player")
                            {
                                PlayerClass = cols[i].GetComponent<CharacterClass>();
                                EnemyClass = GameObject.FindWithTag("Boss").GetComponent<CharacterClass>();

                                minDamage = EnemyClass.m_BossStatData.Atk - PlayerClass.m_CharacterStat.Def;

                                //최소 데미지 보정.
                                if (minDamage <= 0)
                                    minDamage = 1;

                                power = EnemyClass.m_SkillMgr.m_BossSkill.m_CurrentBossSkill.Power;

                                resultDamage = (minDamage * power) * PlayerClass.Invincibility;

                                PlayerClass.m_CharacterStat.HP -= resultDamage;

                                //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                                cols[i].GetComponent<PlayerManager>().Damage((int)resultDamage);
                                //피해 이미지 띄위기 위한 함수
                                cols[i].GetComponent<PlayerManager>().BloodImage();

                                Transform[] allChildren = cols[i].GetComponentsInChildren<Transform>();
                                foreach (Transform child in allChildren)
                                {
                                    if (child.name == "PosBody")
                                    {
                                        PosBody = child;
                                    }
                                }
                                Instantiate(m_DamageEffect, PosBody);


                            }
                            break;

                    }

                }
            }
            else
            {
            }
        }
	}

	void Update()
    {
        if (dmg_check)
        {
            time += Time.deltaTime;
            if (time >= Dot)
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

                                    if (cols[i].tag == "Boss")
                                    {
                                        minDamage = PlayerClass.m_CharacterStat.Atk - EnemyClass.m_BossStatData.Def;

                                        //최소 데미지 보정.
                                        if (minDamage <= 0)
                                            minDamage = 1;

                                        resultDamage = (minDamage * power) * EnemyClass.Invincibility;

                                        EnemyClass.m_BossStatData.HP -= resultDamage;
                                        //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                                        cols[i].GetComponent<BossFSM>().Damage((int)resultDamage);
                                        //피해 이미지 띄위기 위한 함수
                                        cols[i].GetComponent<PlayerManager>().BloodImage();

                                        Transform[] allChildren = cols[i].GetComponentsInChildren<Transform>();
                                        foreach (Transform child in allChildren)
                                        {
                                            if (child.name == "PosBody")
                                            {
                                                PosBody = child;
                                            }
                                        }
                                        Instantiate(m_DamageEffect, PosBody);

                                    }
                                    if (cols[i].CompareTag("Mob"))
                                    {
                                        minDamage = (int)PlayerClass.m_CharacterStat.Atk - EnemyClass.m_SubMonsterData.Defense;

                                        //최소 데미지 보정.
                                        if (minDamage <= 0)
                                            minDamage = 1;

                                        power = (int)GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;

                                        resultDamage = minDamage * power;

                                        cols[i].GetComponent<Enemy>().TakeDamage((int)resultDamage);
                                    }
                                }
                                break;
                            case 2:
                                if (cols[i].tag == "Player")
                                {
                                    minDamage = EnemyClass.m_BossStatData.Atk - PlayerClass.m_CharacterStat.Def;

                                    //최소 데미지 보정.
                                    if (minDamage <= 0)
                                        minDamage = 1;

                                    resultDamage = (minDamage * power) * PlayerClass.Invincibility;

                                    PlayerClass.m_CharacterStat.HP -= resultDamage;
                                    //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                                    cols[i].GetComponent<PlayerManager>().Damage((int)resultDamage);

                                    Transform[] allChildren = cols[i].GetComponentsInChildren<Transform>();
                                    foreach (Transform child in allChildren)
                                    {
                                        if (child.name == "PosBody")
                                        {
                                            PosBody = child;
                                        }
                                    }
                                    Instantiate(m_DamageEffect, PosBody);
                                }
                                break;
                        }
                    }
                }
                else
                {
                }
            }
        }
	}
}


