using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yggdrasil.PlayerSkillSet;
using Model;

public class PlayerManager : MonoBehaviour
{



    //플레이어 오브젝트
    public static GameObject p_Object;

    FollowCam cam;

    //스킬 
    public Spirit m_Spirit;
    //public SpiritSkill m_SpiritSkill;

    private CharStat_TableExcel m_CurrentCharStat;
    private const int m_CurrentIndex = 10002;  //임시로 사용하는 값(플레이어가 메뉴 모드에서 선택한 index값을 참고해서 솔로모드인지 멀티모드인지 판별)

    public CharacterClass PlayerClass;
    public float m_MaxHp;
    public float m_PerHp;

    public KeyCode[] m_SpiritSkillKey;


    public GameObject hudDamageText;
    public Transform hudPos;

    private Animator anim;


    private bool move;

    float h, v;
    float Speed = 5f;
    float rotateSpeed = 5f;

    int x;
    int y;

    //벽인지 체크 하기 위한 불값
    public bool isWall;

    //이동못하는 타일 및 벽에 만나면 밀기 위한 벡터 변수
    Vector3 LeftPlayer = new Vector3(-0.2f, 0.0f, 0.0f);
    Vector3 RightPlayer = new Vector3(0.2f, 0.0f, 0.0f);
    Vector3 UpPlayer = new Vector3(0.0f, 0.0f, 0.225f);
    Vector3 DownPlayer = new Vector3(0.0f, 0.0f, -0.225f);

    Vector3 LeftCam = new Vector3(-0.2f, 0.0f, 0.0f);
    Vector3 RightCam = new Vector3(0.2f, 0.0f, 0.0f);
    Vector3 UpCam = new Vector3(0.0f, 0.0f, 0.198f);
    Vector3 DownCam = new Vector3(0.0f, 0.0f, -0.198f);

    private void InputCheck()
    {

        if (Input.GetKeyDown(m_SpiritSkillKey[3]))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill1);
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[4]))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill2);
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[5]))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill3);
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[0]))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill4);
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[1]))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill5);
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[2]))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill6);
        }

    }



    public void Move()
    {

        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            move = false;
        }

        //방향키로 입력으로 변경
        if (MainManager.Instance.GetStageManager().m_MapInfo[x, y].IsUnWalkable && !isWall)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!move)
                {
                    AnimationManager.GetInstance().PlayAnimation(anim, "Run");
                }

                move = true;
                this.transform.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
                cam.cam.transform.Translate(new Vector3(-1.0f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!move)
                {
                    AnimationManager.GetInstance().PlayAnimation(anim, "Run");
                }
                move = true;
                this.transform.Translate(new Vector3(1.0f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
                cam.cam.transform.Translate(new Vector3(1.0f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!move)
                {
                    AnimationManager.GetInstance().PlayAnimation(anim, "Run");
                }
                move = true;
                this.transform.Translate(new Vector3(0.0f, 0.0f, 0.9f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
                cam.cam.transform.Translate(new Vector3(0.0f, 0.6f, 0.6f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                if (!move)
                {
                    AnimationManager.GetInstance().PlayAnimation(anim, "Run");
                }
                move = true;
                this.transform.Translate(new Vector3(0.0f, 0.0f, -0.9f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
                cam.cam.transform.Translate(new Vector3(0.0f, -0.6f, -0.6f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            }
        }
        else if (!MainManager.Instance.GetStageManager().m_MapInfo[x, y].IsUnWalkable || isWall)
        {
            Debug.Log("아 못감 ㅅㄱㅂ");
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.position += RightPlayer * PlayerClass.m_CharacterStat.MoveSpeed;
                cam.cam.transform.position += RightCam * PlayerClass.m_CharacterStat.MoveSpeed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.position += LeftPlayer * PlayerClass.m_CharacterStat.MoveSpeed;
                cam.cam.transform.position += LeftCam * PlayerClass.m_CharacterStat.MoveSpeed;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.position += DownPlayer * PlayerClass.m_CharacterStat.MoveSpeed;
                cam.cam.transform.position += DownCam * PlayerClass.m_CharacterStat.MoveSpeed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.position += UpPlayer * PlayerClass.m_CharacterStat.MoveSpeed;
                cam.cam.transform.position += UpCam * PlayerClass.m_CharacterStat.MoveSpeed;
            }
        }

        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");

        //Vector3 dir = new Vector3(h, 0, v); // new Vector3(h, 0, v)가 자주 쓰이게 되었으므로 dir이라는 변수에 넣고 향후 편하게 사용할 수 있게 함

        //// 바라보는 방향으로 회전 후 다시 정면을 바라보는 현상을 막기 위해 설정
        //if (!(h == 0 && v == 0))
        //{
        //    // 이동과 회전을 함께 처리
        //    transform.position += dir * Speed * Time.deltaTime;
        //    // 회전하는 부분. Point 1.
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotateSpeed);
        //}

    }



    private void Awake()
    {
        isWall = false;

        m_SpiritSkillKey = new KeyCode[6];
        #region 정령스킬 키코드 값 설정
        m_SpiritSkillKey[0] = KeyCode.Q;
        m_SpiritSkillKey[1] = KeyCode.W;
        m_SpiritSkillKey[2] = KeyCode.E;
        m_SpiritSkillKey[3] = KeyCode.A;
        m_SpiritSkillKey[4] = KeyCode.S;
        m_SpiritSkillKey[5] = KeyCode.D;
        #endregion
        hudDamageText = Resources.Load<GameObject>("DamageText");
    }

    

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<FollowCam>();
        p_Object = this.gameObject;
        m_Spirit = this.GetComponent<Spirit>();

        PlayerClass = this.gameObject.GetComponent<CharacterClass>();
        
        
        // DataManager라는 Object의 List에 해당 데이터를 넣어주면 찾아서 사용가능.(디버깅용)
        // Use ExcelReader
        //foreach (var element in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList)
        //{
        //    Debug.Log(element.No);
        //    Debug.Log(element.StageName);
        //}

        anim = this.transform.GetChild(0).GetComponent<Animator>();

        if (anim == null)
        {
            anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }
        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");

        move = false;

        foreach (var item in DataTableManager.Instance.GetDataTable<CharStat_TableExcelLoader>().DataList)
        {
            if (item.CharStatIndex == m_CurrentIndex)
            {
                PlayerClass.m_CharacterStat = item; //현재 캐릭터 정보를 찾아낸다(나중가서는 로비창에서 선택한 데이터를 비교해서 캐릭터 선택해주기)
                break;
            }
        }


        //Damage();
        m_MaxHp = PlayerClass.m_CharacterStat.HP;
        
    }


    void Update()
    {
        x = MainManager.Instance.GetStageManager().m_PlayerRow;
        y = MainManager.Instance.GetStageManager().m_PlayerCoulmn;

        //이동
        Move();

        //키보드 입력 체크 함수.
        InputCheck();

        if(!move)
        {
            AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");
        }

        //플레이어 체력 백분률
        RearTimePerHP();

        //hudPos = GameObject.Find("Player(Clone)").transform;
        hudPos = GameObject.Find("Player").transform.GetChild(0).gameObject.transform;
    }


    public void Damage(int _damage)
    {
        Debug.Log("현재 플레이어의 체력:" + PlayerClass.m_CharacterStat.HP);
        TakeDamagePrint(_damage);
    }

    public void TakeDamagePrint(int damage)
    {
        
        GameObject hudText = Instantiate(hudDamageText);
        
        hudText.transform.position = hudPos.position + (Vector3.up * 5);
        hudText.GetComponent<DamageTxt>().damage = damage;
    }

    public void RearTimePerHP()
    {
        m_PerHp = PlayerClass.m_CharacterStat.HP / m_MaxHp;
        //Debug.Log(m_CurrentCharStat.HP);
    }

}
