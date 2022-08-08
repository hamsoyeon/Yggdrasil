using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yggdrasil.PlayerSkillSet;

public class PlayerManager : MonoBehaviour
{
	//플레이어 오브젝트
	public static GameObject p_Object;

	//스탯
	public Yggdrasil.CharacterStats p_Status;
    [SerializeField]
    private float p_MaxHp;
    [SerializeField]
    private float p_CurHp;

    public float p_PerHp;

    //스킬
    private ISkill skill;
	private SkillManager M_skillMgr;


	public Text SkillType_txt;
	private int skillType_num;
	// 이벤트 관련
	//private TempCodes.TempMessageSystem eSystem;
	//private MoveEvent me;
	//private Handler player;



	private void InputCheck()
	{

		if (Input.GetKeyDown(KeyCode.Tab))
		{
			
			skillType_num++;
			skillType_num %= 3;

			var type_num = (SkillType)skillType_num;

			switch (type_num)
			{
				case SkillType.Attack:
					skill = SkillFactory.SkillTypeSet(type_num);
					SkillType_txt.text = "Attack";
					break;
				case SkillType.Defense:
					skill = SkillFactory.SkillTypeSet(type_num);
					SkillType_txt.text = "Defense";
					break;
				case SkillType.Support:
					skill = SkillFactory.SkillTypeSet(type_num);
					SkillType_txt.text = "Support";
					break;
			}

		}


		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			//스킬을 사용할수 있는지 1차 점검(마나,쿨타임 등 체크)


			//1차점검에서 통과되면 스킬발동
			if(M_skillMgr.SkillCheck())
			{
				skill.SkillAction(AbilityType.Damage);
				Debug.Log("1");
			}
			
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			//스킬을 사용할수 있는지 1차 점검(마나,쿨타임 등 체크)
			if (M_skillMgr.SkillCheck())
			{
				skill.SkillAction(AbilityType.Distance);
				Debug.Log("2");
			}
			
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			//스킬을 사용할수 있는지 1차 점검(마나,쿨타임 등 체크)
			if (M_skillMgr.SkillCheck())
			{
				skill.SkillAction(AbilityType.Speed);
				Debug.Log("3");
			}
			
		}


	}



	private void Move()
	{
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		transform.Translate(new Vector3(h, 0, v) * p_Status.MoveSpeed * Time.deltaTime);
	}
	
    private void PlayerHp_Per()
    {
        p_PerHp = p_CurHp / p_MaxHp;
    }

	// Start is called before the first frame update
	void Start()
    {
		p_Object = this.gameObject;
		p_Status = new Yggdrasil.CharacterStats();
        p_MaxHp = p_Status.HP;
        p_CurHp = p_MaxHp;

        //캐릭터 초기셋팅
        p_Status.MoveSpeed = 8f; //기본스피드

		skillType_num = 0;
		skill = SkillFactory.SkillTypeSet(SkillType.Attack);
		M_skillMgr = new SkillManager();


        //데이터 리스트 확인용(디버깅용)
        //foreach (var element in DataTableManager.Instance.GetDataList())
        //{
        //    if(element == null)
        //    {
        //        break;
        //    }

        //    Debug.Log(element.name);
        //}


        // DataManager라는 Object의 List에 해당 데이터를 넣어주면 찾아서 사용가능.(디버깅용)
        // Use ExcelReader
        //foreach (var element in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList)
        //{
        //    Debug.Log(element.No);
        //    Debug.Log(element.StageName);
        //}


    }

    // Update is called once per frame
    void Update()
    {
		//이동
		Move();

		//키보드 입력 체크 함수.
		InputCheck();

        //플레이어 체력 퍼센트
        PlayerHp_Per();


    }
}
