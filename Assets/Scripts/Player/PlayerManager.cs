using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yggdrasil.PlayerSkillSet;

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
        //방향키로 입력으로 변경

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(new Vector3(-0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            cam.cam.transform.Translate(new Vector3(-0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(new Vector3(0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            cam.cam.transform.Translate(new Vector3(0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.Translate(new Vector3(0.0f, 0.0f, 0.9f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            cam.cam.transform.Translate(new Vector3(0.0f, 0.6f, 0.6f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.Translate(new Vector3(0.0f, 0.0f, -0.9f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            cam.cam.transform.Translate(new Vector3(0.0f, -0.6f, -0.6f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
        }

        //transform.Translate(new Vector3(h, 0, v) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
    }



    private void Awake()
    {
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
        //m_SpiritSkill = this.GetComponent<SpiritSkill>();

        PlayerClass = this.gameObject.GetComponent<CharacterClass>();

        
        
        // DataManager라는 Object의 List에 해당 데이터를 넣어주면 찾아서 사용가능.(디버깅용)
        // Use ExcelReader
        //foreach (var element in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList)
        //{
        //    Debug.Log(element.No);
        //    Debug.Log(element.StageName);
        //}



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
        //이동
        Move();

        //키보드 입력 체크 함수.
        InputCheck();

        //플레이어 체력 백분률
        RearTimePerHP();

        hudPos = GameObject.Find("Player(Clone)").transform;
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
