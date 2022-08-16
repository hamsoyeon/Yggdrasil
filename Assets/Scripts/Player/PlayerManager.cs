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


    private Animator anim;


    private bool move;

    float h, v;
    float Speed = 5f;
    float rotateSpeed = 5f;

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

        //h = Input.GetAxis("Horizontal");
        //v = Input.GetAxis("Vertical");


        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow)|| Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            move = false;
        }


        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (!move)
            {
                AnimationManager.GetInstance().PlayAnimation(anim, "Run");
            }

            move = true;
            this.transform.Translate(new Vector3(-0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            cam.cam.transform.Translate(new Vector3(-0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);

            //cam.CamPosUpdate(transform.position + new Vector3(0.0f,40f, -26.5f));


        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

            if (!move)
            {
                AnimationManager.GetInstance().PlayAnimation(anim, "Run");
            }
            move = true;
            this.transform.Translate(new Vector3(0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            cam.cam.transform.Translate(new Vector3(0.8f, 0.0f, 0.0f) * PlayerClass.m_CharacterStat.MoveSpeed * Time.deltaTime);
            //cam.CamPosUpdate(transform.position+ new Vector3(0.0f, 40f, -26.5f));


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
            //cam.CamPosUpdate(transform.position+ new Vector3(0.0f, 40f, -26.5f));


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
            //cam.CamPosUpdate(transform.position+ new Vector3(0.0f, 40f, -26.5f));


        }




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




    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<FollowCam>();
        p_Object = this.gameObject;
        m_Spirit = this.GetComponent<Spirit>();

        PlayerClass = this.gameObject.GetComponent<CharacterClass>();


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


        Damage();


    }


    void Update()
    {
        //이동
        Move();

        //키보드 입력 체크 함수.
        InputCheck();

        if(!move)
        {
            AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");
        }


    }


    public void Damage()
    {
        Debug.Log("현재 플레이어의 체력:" + PlayerClass.m_CharacterStat.HP);
    }
}
