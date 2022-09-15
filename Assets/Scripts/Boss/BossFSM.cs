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

    [SerializeField]
    private float currentBossStamina;
    private float bossStaminaSave;
    [SerializeField]
    private float maxStamina;
    
    private int BossRandomSkill = 0;

    public bool behavior = false;

    private BossSkill m_CurrentBossSkill;
    private CharacterClass m_BossClass;

    public GameObject hudDamageText;
    public Transform hudPos;

    private Animator anim;

    public Board GameBoard;

    public GameObject player;

    //int[] UpRotation = { -45, 45 }; // 타일 z - 1 을 할 때 사용할 로테이션 값
    //int[] SameRotation = { -90, 90 }; // 타일 z 값이 같을 때 사용할 로테이션 값
    //int[] DownRotation = { -135, 135 }; // 타일 z + 1 을 할 때 사용할 로테이션 값

    //int BossRotation;

    //보스의 디렉션값을 비교하는 방식은 보스의 현재 타일의 포지션 값과 다음 이동 타겟의 포지션값을 연산 -> 
    //현재 보스 타일 z < 다음 타일 z = DownRotation배열 사용 || 보스 타일 z > 다음 타일 z = UpRotation 배열 사용 || 보스 타일 z = 다음 타일 z = SameRotation 사용
    //배열 사용 방식은 x 값을 비교하고 결정한다(람다식 사용)
    //만약 z값이 UpRotation 을 사용한다고 하면 BossRotation =  현재 보스 x > 다음 타일 x ? UpRotaion[0] : UpRotation[1];
    //BossObject.transform.rotation.y = BossRotation;
    //이렇게 하면 보스 디렉션 만들 수 있을 것 같다.
    //그냥 이동한 로테이션 값 그대로 둔다.

    float h, v; //BossDirection


    private void Awake()
    {
        player = GameObject.Find("obj_10001");
        Debug.Log("됐다", player);
        hudDamageText = Resources.Load<GameObject>("DamageText");

        h = transform.position.x;
        v = transform.position.z;

    }

    void Start()
    {
        gameObject.AddComponent<BossStamina>();
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
        #region 주석
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
        #endregion
    }

    private void FixedUpdate()
    {
        MonsterDirection();
    }

    //보스가 이동할때.
    public void Move()
    {

        //보스 위치 업데이트 해줘야함(Row,Column)
        if(!bossMove) //보스가 움직이지 않고 있다면. 이동을 시작하는 단계니깐
        {

            // 보스이동 코드.
            if (originBlock.next == null || currentBossStamina < m_BossClass.m_BossStatData.MoveStUsed)   //  
            {
                MainManager.Instance.GetStageManager().m_BossRow = originBlock.x;
                MainManager.Instance.GetStageManager().m_BossColumn = originBlock.y;
                behavior = false;
                moving = false;




                while (true)
                {
                    if (startBlock != null)
                    {
                        Debug.Log($"길 지우기:{startBlock.x}/{startBlock.y}");
                        MainManager.Instance.GetStageManager().m_MapInfo[startBlock.x, startBlock.y].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                        tempBlock = startBlock.prev;
                    }
                    else
                    {
                        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");
                        break;
                    }
                    startBlock = tempBlock;
                }


                return;
            }

            tempBlock = originBlock.next;
            currentBossStamina -= m_BossClass.m_BossStatData.MoveStUsed; //스태미너 감소
            targetPos = MainManager.Instance.GetStageManager().m_MapInfo[tempBlock.x, tempBlock.y].MapPos;    //x=row y=column
            bossMove = true;
            Debug.Log(targetPos); //보스 이동 하는 다음 타일의 포지션값 던져준다.
           
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPos, 1f);
        }



        if(transform.position == targetPos)
        {
            int bossRow = MainManager.Instance.GetStageManager().m_BossRow;
            int bossColumn = MainManager.Instance.GetStageManager().m_BossColumn;
            int playerRow = MainManager.Instance.GetStageManager().m_PlayerRow;
            int playerColumn = MainManager.Instance.GetStageManager().m_PlayerCoulmn;

            

            if (bossRow == playerRow && bossColumn == playerColumn)
            {

                while (true)
                {
                    if (startBlock != null)
                    {
                        Debug.Log($"길 지우기:{startBlock.x}/{startBlock.y}");
                        MainManager.Instance.GetStageManager().m_MapInfo[startBlock.x, startBlock.y].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                        tempBlock = startBlock.prev;
                    }
                    else
                    {
                        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");
                        break;
                    }
                    startBlock = tempBlock;
                }

                behavior = false;
                moving = false;
                bossMove = false;
               
            }
            else
            {
                originBlock = tempBlock;
                bossMove = false;
            }
        }

    }



    public float rotateSpeed = 5.0f;

    
    void MonsterDirection()
    {
        float _h, _v;
        _h = 0;
        _v = 0;

        
        

        if (h!=transform.position.x || v!=transform.position.z)
        {
            if (h > transform.position.x)
                _h = -1;
            else if (h < transform.position.x)
                _h = 1;

            if (v > transform.position.z)
                _v = -1;
            else if (v < transform.position.z)
                _v = 1;

            Vector3 dir = new Vector3(_h, 0, _v);
            Debug.Log("In if vector : " + dir);

            transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * rotateSpeed);

            h = transform.position.x;
            v = transform.position.z;
        }
    }

        


    public void Damage(int _damage)
    {
        Debug.Log("현재 보스의 체력:" + m_BossClass.m_BossStatData.HP);
        TakeDamagePrint(_damage);
    }

    public void TakeDamagePrint(int damage)
    {

        GameObject hudText = Instantiate(hudDamageText);

        hudText.transform.position = hudPos.position + (Vector3.up * 20);
        hudText.GetComponent<DamageTxt>().damage = damage;
    }
    public float GetPerStamina()
    {
        return currentBossStamina / maxStamina;
    }
    

    bool moving = false;
    bool bossMove = false;
    float moveTime;

    Vector3 targetPos;

    Block bossBlock;
    Block PlayerBlock;
    Block startBlock;
    Block tempBlock;
    Block originBlock;

    


    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        hudPos = this.gameObject.transform;

        


        if (behavior && moving)
        {
            //moveTime += Time.deltaTime;
            Move();
        }

        if (time > 1.0f && !behavior)
        {
            currentBossStamina += DataTableManager.Instance.GetDataTable<Boss_TableExcelLoader>().DataList[0].Speed;
            Debug.Log($"턴미터 회복{maxStamina}/{currentBossStamina}");

            if (currentBossStamina >= maxStamina)
            {
                //int moveAndSkill = Random.Range(1, 3);

                // 랜덤값 추출후 행동(이동 or 스킬)을 정함.
                int moveAndSkill = Random.Range(1, 11); //이부분을 2로 고정하면 스킬만 사용
                if(moveAndSkill > 3)
                {
                    moveAndSkill = 2;
                }
                else
                {
                    moveAndSkill = 1;
                }

                //moveAndSkill = 1;  // 이동고정
                //moveAndSkill = 2;  // 스킬고정
                moveAndSkill = 2;  // 스킬고정

                if (moveAndSkill ==1)
                {
                    Debug.Log("상대방 추적");


                    int bossRow = MainManager.Instance.GetStageManager().m_BossRow;
                    int bossColumn = MainManager.Instance.GetStageManager().m_BossColumn;
                    int playerRow = MainManager.Instance.GetStageManager().m_PlayerRow;
                    int playerColumn = MainManager.Instance.GetStageManager().m_PlayerCoulmn;

                    

                    if( (bossRow == playerRow) && ( bossColumn == playerColumn ))
                    {
                        Debug.Log("보스가 플레이어 위치에 도착했습니다.");
                        Debug.Log($"Boss : {bossRow}/{bossColumn}");
                        Debug.Log($"Player: {playerRow}/{playerColumn}");
                    }
                    else
                    {
                        //a*알고리즘 발동.
                        bossBlock = new Block(bossRow, bossColumn); //보스 위치좌표 할당
                        PlayerBlock = new Block(playerRow, playerColumn); //플레이어 위치좌표 할당

                        //현재 패스얻어오는곳에서 가중치 값이 똑같은 타일에 있을경우 보스가 안움직이는 버그 발생 + 일정이상의 거리를 벌려야 추격.(한칸 주위에서는 플레이어 자리까지 추격안됨)

                        startBlock = AStarNJ.PathFinding(GameBoard, bossBlock, PlayerBlock);  // 두 좌표로 a* 사용후 패스 얻어오기
                        tempBlock = null;  //임시 블럭 다음블럭을 저장후 바꾸기위한 값. 

                        if (startBlock != null) //원본값 저장.
                        {
                            originBlock = startBlock;
                        }


                        // 플레이어까지의 타일 루트  노란색으로 색칠해주기
                        while (true)
                        {

                            if (startBlock.next != null)
                            {
                                Debug.Log($"길 찾기:{startBlock.next.x}/{startBlock.next.y}");
                                MainManager.Instance.GetStageManager().m_MapInfo[startBlock.next.x, startBlock.next.y].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.yellow;
                                tempBlock = startBlock.next;
                            }
                            else
                            {
                                break;
                            }
                            startBlock = tempBlock;
                        }

                        bossMove = false;
                        behavior = true;
                        moving = true;
                        AnimationManager.GetInstance().PlayAnimation(anim, "Run");
                    }

                    
                }
                else if(moveAndSkill == 2)
                {

                    Debug.Log("스킬발동");
                    currentBossStamina = 0;

                    BossRandomSkill = Random.Range(1, 5);  //스킬 3개만 사용중 (소환스킬 사용x)
                    //BossRandomSkill = Random.Range(1, 5);  //스킬 3개만 사용중 (소환스킬 사용x)
                    BossRandomSkill = 4;  //고정스킬(몹 소환)

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
