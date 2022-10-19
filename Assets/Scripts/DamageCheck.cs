using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCheck : MonoBehaviour
{

    public CharacterClass EnemyClass;
    public bool dmg_check = false;
    public GameObject DamageEffect;  //해당 이펙트의 피격된 적의 PosBody에 생성.

    private CharacterClass PlayerClass;

    private Buff BuffClass;

	private float minDamage;
	private float resultDamage;
    private float power;

	//1.Player 2.Enemy
	public int who; 

	private float time;

	public float Dot;


    public int buffIndex;         //Dot시간이 될때마다 Buff.cs파일의 Buff()혹은 DeBuff()를 실행시켜서 버프/디버프의 추가 or 지속시간 갱신을 시킨다.

    void Start()
    {
        if(dmg_check)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position, 12f);
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

                                //BuffClass = cols[i].GetComponent<Buff>();
                                //BuffClass.AddBuff(buffIndex);  //버프 추가.

                                EnemyClass = cols[i].GetComponent<CharacterClass>();
                                //PlayerClass = GameObject.Find("Player").GetComponent<CharacterClass>();
                                PlayerClass = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<CharacterClass>();



                                if (cols[i].tag == "Boss")
                                {
                                    minDamage = PlayerClass.m_CharacterStat.Atk - EnemyClass.m_BossStatData.Def;

                                    //최소 데미지 보정.
                                    if (minDamage <= 0)
                                        minDamage = 1;

                                    power = (int)GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;

                                    resultDamage = (minDamage * power) * EnemyClass.Invincibility;

                                    EnemyClass.m_BossStatData.HP -= resultDamage;
                                    //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                                    cols[i].GetComponent<BossFSM>().Damage((int)resultDamage);
                                }
                                if (cols[i].CompareTag("Mob"))
                                {
                                    Debug.Log("아 왜이리 빠른가요");
                                    minDamage = (int)PlayerClass.m_CharacterStat.Atk;

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
                                Debug.Log("플레이어 이펙트 충돌");
                                //BuffClass = cols[i].GetComponent<Buff>(); //맞은 대상의 Buff를 가지고 와서 지금 가지고있는 버프 목록을 리스트에 추가.
                                //BuffClass.AddBuff(buffIndex);  //버프 추가.

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
                                //cols[i].GetComponent<PlayerManager>().PlayerDamageTxt

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
                                    Debug.Log("적 이펙트 충돌");


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
                                    }
                                    if (cols[i].CompareTag("Mob"))
                                    {
                                        Debug.Log("아 왜이리 빠른가요");
                                        minDamage = (int)PlayerClass.m_CharacterStat.Atk;

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
                                    Debug.Log("플레이어 이펙트 충돌");

                                    minDamage = EnemyClass.m_BossStatData.Atk - PlayerClass.m_CharacterStat.Def;

                                    //최소 데미지 보정.
                                    if (minDamage <= 0)
                                        minDamage = 1;

                                    resultDamage = (minDamage * power) * PlayerClass.Invincibility;

                                    PlayerClass.m_CharacterStat.HP -= resultDamage;
                                    //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                                    cols[i].GetComponent<PlayerManager>().Damage((int)resultDamage);
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
}


