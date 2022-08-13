using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossFSM : MonoBehaviour
{
    private int currentBossNumber;
    private BossStat_TableExcel m_BossStat;
    //private BossStat_TableExcelLoader m_BossStat;


    private float time;
    private int actionPoint;
    //private bool actionCheck=false;
    private bool spCheck = false;

    private float currentBossStamina;
    private float bossStaminaSave;
    private float maxStamina;

    private int BossRandomSkill = 0;

    public bool behavior = false;

    private BossSkill m_CurrentBossSkill;
    private CharacterClass m_BossClass;

    // Start is called before the first frame update
    void Start()
    {
        maxStamina = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].MaxStamina;

        m_BossClass = this.gameObject.GetComponent<CharacterClass>();
        m_CurrentBossSkill = this.gameObject.GetComponent<BossSkill>();
        currentBossNumber = 0;   //스테이지에서 해당 스테이지에 맞는 보스 정보를 가져와야됨 지금은 임시로 1로 설정

        m_BossClass.m_BossStatData = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[currentBossNumber];
        m_BossClass.m_SkillMgr.m_BossSkill = m_CurrentBossSkill;

        Debug.Log("현재 보스의 체력:" + m_BossClass.m_BossStatData.HP);
    }

    public void Damage()
    {
        Debug.Log("현재 보스의 체력:" + m_BossClass.m_BossStatData.HP);
    }


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 1.0f && !behavior)
        {

            currentBossStamina += DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Speed;
            Debug.Log($"턴미터 회복{maxStamina}/{currentBossStamina}");


            if (currentBossStamina >= maxStamina)
            {
                Debug.Log("스킬발동");
                currentBossStamina = 0;

                //int BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Skill1;
                //this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);

                BossRandomSkill = Random.Range(1, 4);

                switch (BossRandomSkill)
                {
                    case 1:
                        //스킬인덱스를 참고해 스킬을 사용.
                        Debug.Log("스킬1 사용");
                        m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill1);

                        //BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[BossRandomSkill].Skill1;
                        //this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);
                        break;
                    case 2:
                        Debug.Log("스킬2 사용");
                        m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill2);
                        //BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[BossRandomSkill].Skill2;
                        //this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);
                        break;
                    case 3:
                        Debug.Log("스킬3 사용");
                        m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill3);

                        //BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[BossRandomSkill].Skill3;
                        //this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);
                        break;
                }



            }

            time = 0;
        }




    }
}
