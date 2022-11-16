using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffTileChk : MonoBehaviour
{
    public bool debuffTile_chk = false;

    //1.Player에겐 디버프 2.Enemy에게 버프
    public int who;
    public float Dot;
    public float lifeTime;
    private int p_debuffcount = 0; //플레이어 디버프 카운트 중첩이 안되게
    private int b_debuffcount = 0; //보스 디버프 카운트 중첩이 안되게
    private float time;
    private float dotTime;
    public CharacterClass EnemyClass; //보스 
    private CharacterClass PlayerClass; //캐릭터 접근 가능

    void Start()
    {

    }

    void Update()
    {

        time += Time.deltaTime;
        dotTime += Time.deltaTime;

        if (debuffTile_chk)
        {
            if(dotTime>=Dot)
            {
                dotTime = 0f;
                Collider[] cols = Physics.OverlapSphere(transform.position, 12f);
                if (cols.Length > 0)
                {
                    for (int i = 0; i < cols.Length; i++)
                    {
                        //선택적으로 버프와 디버프를 줄때
                        switch (who)
                        {
                            case 1:
                                if (cols[i].tag == "Boss")
                                {
                                    EnemyClass = cols[i].GetComponent<CharacterClass>();
                                    if (cols[i].tag == "Boss" && b_debuffcount == 0)
                                    {
                                        b_debuffcount++;
                                        EnemyClass.m_BossStatData.Atk += 10;
                                    }
                                }
                                break;
                            case 2:
                                if (cols[i].tag == "Player")
                                {
                                    PlayerClass = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<CharacterClass>();

                                    if (cols[i].tag == "Player" && p_debuffcount == 0)
                                    {
                                        p_debuffcount++;
                                        PlayerClass.m_CharacterStat.Atk -= 10;
                                    }
                                }
                                break;
                        }
                    }
                }
            }
        }

        if (time >= lifeTime)
            Destroy(this.gameObject);
    }
}
