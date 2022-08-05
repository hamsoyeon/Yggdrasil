using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossFSM : MonoBehaviour
{

    private Yggdrasil.BossManager M_BossInfo;

    private BossStat_TableExcel m_BossStat;
    //private BossStat_TableExcelLoader m_BossStat;


    private float time;
    private int actionPoint;
    //private bool actionCheck=false;
    private bool spCheck = false;

    
    private float currentBossStamina;    
    private float bossStaminaSave;    
    private float maxStamina;

    public float perStamina;

    public GameObject hudDamageText;

    private int BossRandomSkill = 0;

    // Start is called before the first frame update
    void Start()
    {
        maxStamina = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].MaxStamina;
    }

    void StateHit(int damage)
    {
        GameObject hudText = Instantiate(hudDamageText);
        hudText.GetComponent<DamageTxt>().damage = damage;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        
        if (time > 1.0f)
        {
            
            currentBossStamina += DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Speed;
            Debug.Log($"턴미터 회복{maxStamina}/{currentBossStamina}");
            #region 히트판정 임시코드
            perStamina = currentBossStamina / maxStamina;
            if (Input.GetKeyDown(KeyCode.H))
            {
                StateHit(50);                   //매개변수 = 데미지
            }
            #endregion

            if (currentBossStamina > maxStamina)
            {
                Debug.Log("스킬발동");

                int BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Skill1;
                this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);


                //BossRandomSkill = Random.Range(1, 4);

                //switch(BossRandomSkill)
                //{
                //	case 1:
                //		//여기서 보스 스킬에 접근해서 스킬을 사용해야한다.
                //		BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Skill1;
                //		this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);
                //		break;
                //	case 2:
                //		BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Skill2;
                //		this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);
                //		break;
                //	case 3:
                //		BossSkillIndex = DataTableManager.Instance.GetDataTable<BossStat_TableExcelLoader>().DataList[0].Skill3;
                //		this.gameObject.GetComponent<BossSkill>().BossSkillAction(BossSkillIndex);
                //		break;
                //}

                currentBossStamina = 0;

            }




            //if (currentBossStamina > maxStamina)
            //{
            //	bossStaminaSave = currentBossStamina - maxStamina;
            //}

            time = 0;
        }




    }
}
