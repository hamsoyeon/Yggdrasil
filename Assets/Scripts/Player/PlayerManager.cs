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

    int x;
    int y;

    bool isWall = false;

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

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill2);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill3);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill4);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill5);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_Spirit.SpiritSummon(PlayerClass.m_CharacterStat.Skill6);
        }

    }



    public void Move()
    {
        //방향키로 입력으로 변경
        if (MainManager.Instance.GetStageManager().m_MapInfo[x, y].IsUnWalkable || isWall == false)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
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
        }
        else if (!MainManager.Instance.GetStageManager().m_MapInfo[x, y].IsUnWalkable || isWall == true)
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


        Damage();


    }

    void Update()
    {
        x = MainManager.Instance.GetStageManager().m_PlayerRow;
        y = MainManager.Instance.GetStageManager().m_PlayerCoulmn;

        //이동
        Move();

        //키보드 입력 체크 함수.
        InputCheck();

    }


    public void Damage()
    {
        Debug.Log("현재 플레이어의 체력:" + PlayerClass.m_CharacterStat.HP);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("벽");
            isWall = true;
        }
    }
}
