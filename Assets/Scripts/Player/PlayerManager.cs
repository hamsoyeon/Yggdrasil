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
    private float m_MaxHp;
    private float m_PerHp;

    public KeyCode[] m_SpiritSkillKey;


    public GameObject hudDamageText;
    public Transform hudPos;

    [SerializeField]
    private Animator anim;
    private PlayerDirection dir;

    
    float h, v;
    public float rotateSpeed = 5f;

    private CharacterController characterController;

    int x;
    int y;

    // 0 -> q / 1 -> w / 2 -> e / 3 -> a / 4 -> s / 5 -> d
    bool[] CanSkill; // 스킬이 사용이 가능한지 판단하기 위한 bool값

    //각각 스킬의 쿨타임 넣어놓는 배열
    float[] SkillCollTime;

    public float m_BuffCoolDown = 0.0f; //아이템 혹은 버프타일에 의하여 쿨타임이 줄어드는 버프에 사용을 하기 위해 넣어놓은 변수 

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
        // 0 -> q / 1 -> w / 2 -> e / 3 -> a / 4 -> s / 5 -> d
        if (Input.GetKeyDown(m_SpiritSkillKey[3]) && CanSkill[3]) //플레이어 A스킬
        {
            CanSkill[3] = false;
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill1);
            Debug.Log($"스킬 A 는 {CanSkill[3]}");
            // 쿨타임 코루틴 작동
            StartCoroutine(CoolTime(SkillCollTime[3], 3, m_BuffCoolDown));
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[4]) && CanSkill[4]) //플레이어 S스킬
        {
            CanSkill[4] = false;
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill2);
            Debug.Log($"스킬 S 는 {CanSkill[4]}");
            // 쿨타임 코루틴 작동
            StartCoroutine(CoolTime(SkillCollTime[4], 4, m_BuffCoolDown));
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[5]) && CanSkill[5]) //플레이어 D스킬
        {
            CanSkill[5] = false;
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill3);
            Debug.Log($"스킬 D 는 {CanSkill[5]}");
            // 쿨타임 코루틴 작동
            StartCoroutine(CoolTime(SkillCollTime[5], 5, m_BuffCoolDown));
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[0]) && CanSkill[0]) //플레이어 Q스킬
        {
            CanSkill[0] = false;
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill4);
            Debug.Log($"스킬 Q 는 {CanSkill[0]}");
            // 쿨타임 코루틴 작동
            StartCoroutine(CoolTime(SkillCollTime[0], 0, m_BuffCoolDown));
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[1]) && CanSkill[1]) //플레이어 W스킬
        {
            CanSkill[1] = false;
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill5);
            Debug.Log($"스킬 W 는 {CanSkill[1]}");
            // 쿨타임 코루틴 작동
            StartCoroutine(CoolTime(SkillCollTime[1], 1, m_BuffCoolDown));
        }

        if (Input.GetKeyDown(m_SpiritSkillKey[2]) && CanSkill[2]) //플레이어 E스킬
        {
            CanSkill[2] = false;
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill6);
            Debug.Log($"스킬 E 는 {CanSkill[2]}");
            // 쿨타임 코루틴 작동
            StartCoroutine(CoolTime(SkillCollTime[2], 2, m_BuffCoolDown));
        }

    }



    public void Move()
    {
        h = Input.GetAxis("Horizontal"); //프로젝트 셋팅으로 wasd 값 날려놓음 방향키만 적용이 될겁니다.
        v = Input.GetAxis("Vertical");

        // new Vector3(h, 0, v)가 자주 쓰이게 되었으므로 dir이라는 변수에 넣고 향후 편하게 사용할 수 있게 함
        Vector3 MoveForce = new Vector3(h * PlayerClass.m_CharacterStat.MoveSpeed, -1, v * PlayerClass.m_CharacterStat.MoveSpeed);

        if(h != 0 || v != 0)
        {
            //AnimationManager.GetInstance().PlayAnimation(anim, "Run"); //이동할때마다 호출을 하여서 Run 애니메이션을 호출하여 파닥파닥 거리는 현상 발생
            anim.SetBool("isRunning", true);
        }
        else if(h ==0 && v == 0)
        {
            anim.SetBool("isRunning", false);
        }

        characterController.Move(MoveForce * Time.deltaTime);
    }


    // Manager -> 관리자 Player 전부 3인용
    // Movement -> Rotation, Move - PlayerController
    public void ChangePlayerDirection()
    {
        float h, v;
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        //방향키 입력 말고 플레이어 이동 값을 받아와야겠다..

        //h = this.transform.position.x;
        //v = this.transform.position.z;

        Vector3 dir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            this.transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * rotateSpeed);
        }
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
        characterController = GetComponent<CharacterController>();
        cam = GetComponent<FollowCam>();
        p_Object = this.gameObject;
        m_Spirit = this.GetComponent<Spirit>();

        PlayerClass = this.gameObject.GetComponent<CharacterClass>();

        CanSkill = new bool[6]; // 0 -> q / 1 -> w / 2 -> e / 3 -> a / 4 -> s / 5 -> d 
        SkillCollTime = new float[6];

        //Start 되는 부분에서 스킬사용 가능 불값과 스킬 쿨타임값을 설정
        for (int i = 0; i < CanSkill.Length; i++)
        {
            CanSkill[i] = true;
            SkillCollTime[i] = 5.0f;
        }

        
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
        //AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");
        anim.SetBool("isRunning", false);


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

        //플레이어 체력 백분률
        RearTimePerHP();

        //hudPos = GameObject.Find("Player(Clone)").transform;
        hudPos = GameObject.Find("Player").transform.GetChild(0).gameObject.transform;
    }

    private void FixedUpdate()
    {
        ChangePlayerDirection();

        //PlayerDirection.GetInstance().ChangePlayerDirection(gameObject);
    }


    public void Damage(int _damage)
    {
        Debug.Log("현재 플레이어의 체력:" + PlayerClass.m_CharacterStat.HP);
        TakeDamagePrint(_damage);
    }

    public void TakeDamagePrint(int damage)
    {
        
        GameObject hudText = Instantiate(hudDamageText);
        
        hudText.transform.position = hudPos.position + (Vector3.up * 15);
        hudText.GetComponent<DamageTxt>().damage = damage;
    }

    private void RearTimePerHP()
    {
        m_PerHp = PlayerClass.m_CharacterStat.HP / m_MaxHp;
        //Debug.Log(m_CurrentCharStat.HP);
       
    }
    public float GetPlayerPerHp()
    {
        return m_PerHp;
    }

    IEnumerator CoolTime(float cool, int index, float Buff)
    {
        Debug.Log($"{index} 스킬의 쿨타임 시작");

        //if (cool > 1.0f)
        //{
        //    //img_Skill.fillAmount = (1.0f / cool); // 이미지 ui 에 차오르는 ui 구현
        //    yield return new WaitForFixedUpdate();
        //}
        yield return new WaitForSeconds(cool - Buff);
        CanSkill[index] = true;
    }
}
