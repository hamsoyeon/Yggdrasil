using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossFSM : MonoBehaviour
{
    private int currentBossNumber;
    private Boss_TableExcel m_BossStat;
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


    private Animator anim;

    public Board GameBoard;

    // Start is called before the first frame update
    void Start()
    {
        maxStamina = DataTableManager.Instance.GetDataTable<Boss_TableExcelLoader>().DataList[0].MaxStamina;

        m_BossClass = this.gameObject.GetComponent<CharacterClass>();
        m_CurrentBossSkill = this.gameObject.GetComponent<BossSkill>();
        currentBossNumber = 0;   //스테이지에서 해당 스테이지에 맞는 보스 정보를 가져와야됨 지금은 임시로 1로 설정

        m_BossClass.m_BossStatData = DataTableManager.Instance.GetDataTable<Boss_TableExcelLoader>().DataList[currentBossNumber];
        m_BossClass.m_SkillMgr.m_BossSkill = m_CurrentBossSkill;

        Debug.Log("현재 보스의 체력:" + m_BossClass.m_BossStatData.HP);


        anim = this.transform.GetChild(0).GetComponent<Animator>();

        if (anim == null)
        {
            anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }


        GameBoard = new Board(6, 5);


        for (int i=0;i<5;i++)
        {
            for(int j=0;j<6;j++)
            {
                if(!MainManager.Instance.GetStageManager().m_MapInfo[i,j].IsUnWalkable)
                {
                    GameBoard.SetBlock(j, i,true);
                }
            }
        }

        //Block bossBlock = new Block(MainManager.Instance.GetStageManager().m_BossRow, MainManager.Instance.GetStageManager().m_BossColumn);
        //Block PlayerBlock = new Block(MainManager.Instance.GetStageManager().m_PlayerRow, MainManager.Instance.GetStageManager().m_PlayerCoulmn);
        //var startBlock = AStarNJ.PathFinding(GameBoard, bossBlock, PlayerBlock);

        //if(startBlock != null)
        //{
        //    Debug.Log("asd");
        //}
        
        //if(startBlock.next != null)
        //{
        //    Debug.Log("qwe");
        //}

        //Block tempBlock = null;

        //while (true)
        //{
        //    Debug.Log($"길찾기:{startBlock.x}/{startBlock.y}");

        //    if (startBlock.next != null)
        //    {
        //        tempBlock = startBlock.next;
        //    }
        //    else
        //    {
        //        break;
        //    }

        //    startBlock = tempBlock;
        //}

    }


    //보스가 이동할때.
    public void Move()
    {
       
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

            currentBossStamina += DataTableManager.Instance.GetDataTable<Boss_TableExcelLoader>().DataList[0].Speed;
            Debug.Log($"턴미터 회복{maxStamina}/{currentBossStamina}");


            if (currentBossStamina >= maxStamina)
            {
                //int moveAndSkill = Random.Range(1, 3);

                int moveAndSkill = Random.Range(1, 11);

                if(moveAndSkill > 3)
                {
                    moveAndSkill = 2;
                }
                else
                {
                    moveAndSkill = 1;
                }

                //currentBossStamina = 0f;

                //int moveAndSkill = 1;

                if (moveAndSkill ==1)
                {
                    Debug.Log("상대방 추적");

                    //보스 위치 업데이트 해줘야함(Row,Column)

                    //a*알고리즘 발동.
                    Block bossBlock = new Block(MainManager.Instance.GetStageManager().m_BossRow, MainManager.Instance.GetStageManager().m_BossColumn);
                    Block PlayerBlock = new Block(MainManager.Instance.GetStageManager().m_PlayerRow, MainManager.Instance.GetStageManager().m_PlayerCoulmn);
                    var startBlock = AStarNJ.PathFinding(GameBoard, bossBlock, PlayerBlock);
                    Block tempBlock = null;

                    var copyBlock = startBlock;
                    //플레이어까지의 길 색칠해주기

                    while (true)
                    {
                        Debug.Log($"길찾기:{startBlock.x}/{startBlock.y}");
                        MainManager.Instance.GetStageManager().m_MapInfo[startBlock.x, startBlock.y].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.yellow;

                        if (startBlock.next != null)
                        {
                            tempBlock = startBlock.next;
                        }
                        else
                        {
                            break;
                        }

                        startBlock = tempBlock;
                    }

                    bool moving = false;
                    int i = 0;
                    behavior = true;
                    float checkTime = 0f;


                    while (true)
                    {

                        if (!moving)
                        {
                            //와일문 종료조건  
                            if (copyBlock.next == null || currentBossStamina < m_BossClass.m_BossStatData.MoveStUsed)
                            {
                                MainManager.Instance.GetStageManager().m_BossRow = copyBlock.x;
                                MainManager.Instance.GetStageManager().m_BossColumn = copyBlock.y;
                                behavior = false;
                                //Debug.Log(startBlock.next);
                                break;
                            }

                            if (copyBlock.next != null)
                            {
                                tempBlock = copyBlock.next;
                                currentBossStamina -= m_BossClass.m_BossStatData.MoveStUsed;
                                Vector3 targetPos = MainManager.Instance.GetStageManager().m_MapInfo[tempBlock.x, tempBlock.y].MapPos;    //x=row y=column
                                


                                Debug.Log(targetPos);

                                transform.position = targetPos + new Vector3(0, 0.7f, 0);
                                moving = true;
                                i++;
                            }

                        }
                        else
                        {
                            checkTime += Time.deltaTime;
                            Debug.Log(checkTime);

                            if(checkTime >= 1.0f)
                            {
                                Debug.Log($"{i}번째 움직임");
                                moving = false;
                                copyBlock = tempBlock;
                                checkTime = 0f;
                            }

                          
                        }



                    }




                }
                else if(moveAndSkill == 2)
                {

                    Debug.Log("스킬발동");
                    currentBossStamina = 0;

                    BossRandomSkill = Random.Range(1, 4);
                    switch (BossRandomSkill)
                    {
                        case 1:
                            //스킬인덱스를 참고해 스킬을 사용.
                            Debug.Log("스킬1 사용");
                            m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill1);
                            AnimationManager.GetInstance().PlayAnimation(anim, "Skill01");
                            break;
                        case 2:
                            Debug.Log("스킬2 사용");
                            m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill2);
                            AnimationManager.GetInstance().PlayAnimation(anim, "Skill02");
                            break;
                        case 3:
                            Debug.Log("스킬3 사용");
                            m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill3);
                            AnimationManager.GetInstance().PlayAnimation(anim, "Skill03");
                            break;
                        case 4:
                            Debug.Log("스킬4 사용");
                            m_BossClass.Skill(CharacterClass.Character.BOSS, m_BossClass.m_BossStatData.Skill4);
                            AnimationManager.GetInstance().PlayAnimation(anim, "Skill04");
                            break;
                    }

                }



                



            }

            time = 0;
        }




    }
}
