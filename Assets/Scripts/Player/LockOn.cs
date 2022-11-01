using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    public bool m_lockOn=false;
    public GameObject target;
    public GameObject m_DamPrefab;
    public float moveSpeed;

    private float m_dist;
    private Vector3 m_targetPos;

    // Start is called before the first frame update
    void Start()
    {
        if(m_lockOn)
        {
            this.transform.LookAt(target.transform.position);      // 목표의 방향 설정.
            m_targetPos = target.transform.position;

        }

        Debug.Log("Target LockOn:" + target.gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_lockOn)
        {
            //ballrigid.velocity = transform.forward * ballVelocity;
            //var ballTargetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.8f) - transform.position);
            //ballrigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));

            m_dist = Vector3.Distance(transform.position, target.transform.position);

            //Debug.Log("Dist:" + m_dist);

            if (target == null )
            {
                Destroy(this.gameObject);
            }
            else if(m_dist < 1f)
            {

                //Debug.Log("위치도착 데미지 처리 및 오브젝트 제거");

                if (target.tag == "Boss")
                {

                    CharacterClass EnemyClass = target.GetComponent<CharacterClass>();
                    CharacterClass PlayerClass = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<CharacterClass>();


                    float minDamage = PlayerClass.m_CharacterStat.Atk - EnemyClass.m_BossStatData.Def;

                    //최소 데미지 보정.
                    if (minDamage <= 0)
                        minDamage = 1;

                    float power = (int)GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;

                    float resultDamage = (minDamage * power) * EnemyClass.Invincibility;

                    EnemyClass.m_BossStatData.HP -= resultDamage;
                    //해당 플레이어의 UI에 접근해서 데미지 표시 외적으로 띄어주기.
                    target.GetComponent<BossFSM>().Damage((int)resultDamage);

                    Transform[] allChildren = target.GetComponentsInChildren<Transform>();
                    Transform PosBody = null;
                    foreach (Transform child in allChildren)
                    {
                        if (child.name == "PosBody")
                        {
                            PosBody = child;
                        }
                    }

                    Instantiate(m_DamPrefab, PosBody);

                }
                else if(target.tag == "Mob")
                {
                    CharacterClass EnemyClass = target.GetComponent<CharacterClass>();
                    CharacterClass PlayerClass = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<CharacterClass>();

                    float minDamage = (int)PlayerClass.m_CharacterStat.Atk;

                    //최소 데미지 보정.
                    if (minDamage <= 0)
                        minDamage = 1;

                    float power = (int)GameObject.Find("Player").transform.GetChild(0).GetComponent<PlayerManager>().m_Spirit.m_SpiritClass.m_SpiritSkillData.Power;

                    float resultDamage = minDamage * power;

                    target.transform.GetComponent<Enemy>().TakeDamage((int)resultDamage);

                    Transform[] allChildren = target.GetComponentsInChildren<Transform>();
                    Transform PosBody = null;
                    foreach (Transform child in allChildren)
                    {
                        if (child.name == "PosBody")
                        {
                            PosBody = child;
                        }
                    }

                    Instantiate(m_DamPrefab, PosBody);

                }


                Destroy(this.gameObject);
            }
            else
            {
                this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
            }

           

           

        }
        

    }
}
