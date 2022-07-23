using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;      //by MikangMark


public class BossFSM : MonoBehaviour
{

	private Yggdrasil.BossManager M_BossInfo;

	private float time;
	private int actionPoint;
	//private bool actionCheck=false;
	private bool spCheck = false;

    #region by MikangMark
    
    //보스 사망후 시간을 재는 변수
    public float dieTime;   
    //카메라의 우선순위를 건들여 시점변경 플레이어의 값 10 10보다 높게설정시 보스로 시점전환 ex)  V_BossCamera.Priority = 11; 다시플레이어로 전환할려면 10보다 낮게 설정할것
    public CinemachineVirtualCamera V_BossCamera;
    //보스 등장, 보스 광역기? 궁극기?, 보스 사망시(현제는 보스사망시에만 카메라가 보스로향하게 되어있음)

    public GameObject hudDamageText;
    public Transform hudPos;
    #endregion
    public enum BossState
	{
		NOMAL,      //일반(상태이상에 걸리지 않은 정상 상태),대기(SP를 회복하는 상태)
		IDLE,       //행동의 전환을 기다리는 시점(공격,이동,스킬)


		//행동의 변화 상태
		BUFF,       //버프 스킬을 사용하는 상태
		MOVE,       //이동
		BATTLE,     //전투


		//전투에서의 
		DETECTION,   //탐지
		ATTACK,     //공격(스킬을 사용하는 상태)
		AGGRO,      //어그로
		
		//외부의 상태(어떤 상태에서도 바로 접근할수 있도록)
		CC,         //상태이상(군중제어상태)
		DIE,        //죽음
		HIT         //피격
	}

	public BossState bossState;


	void StateNomal()
	{
		//보스의 SP가 100%라면 다음 행동을 실행.
		//if (M_BossInfo.GetTableExcel().MaxTM >= actionPoint)
		//{
		//	//sp게이지가 안채워지도록.
		//	spCheck = true;

		//	bossState = BossState.IDLE;

		//	Debug.Log("보스의 SP 게이지가 다차서 상태를 IDLE로 전환합니다.");
		//}
		//else
		//{
		//	//애니메이션 기본모션 셋팅.
		//}
	}

	void StateIdle()
	{

		int rand = Random.Range(0, 3);

		switch (rand)
		{
			case 0:
				if(actionPoint > 100)        //이동에 필요한 SP게이지가 있을때. 없으면 -> 다시 뽑는다.
					bossState = BossState.BUFF;
				break;
			case 1:
				if (actionPoint > 200)
					bossState = BossState.MOVE;
				break;
			case 2:
				if (actionPoint > 500)
					bossState = BossState.BATTLE;
				break;
		}
	}

    void StateDie()         
    {
        #region by MikangMark
        dieTime += Time.deltaTime;               //보스 사망경과시간
        V_BossCamera.Priority = 11;              //보스로 카메라 전환
        if (dieTime > 3f)                        //보스가 죽은뒤 3초가 지나면
        {                                        //
            V_BossCamera.Priority = 9;           //시점 다시 플레이어로 이동
        }
        #endregion
    }

    void StateHit()
    {
        #region by MikangMark
        int damage = 100;                                 //나중에 매개변수에서 데미지 받기
        GameObject hudText = Instantiate(hudDamageText); // 생성할 텍스트 오브젝트
        hudText.transform.position = hudPos.position; // 표시될 위치
        hudText.GetComponent<DamageTxt>().damage = damage; // 데미지 전달
        //base.TakeDamage(damage);
        bossState = BossState.NOMAL;
        #endregion
    }


    // Start is called before the first frame update
    void Start()
	{
		//M_BossInfo = new Yggdrasil.BossManager(21001);

		bossState = BossState.NOMAL;

		//Debug.Log($"BossFSM: 현재의 보스의 이름은 \"{M_BossInfo.GetTableExcel().Name_KR}\" 입니다.");
	}

	// Update is called once per frame
	void Update()
	{
		time += Time.deltaTime;

		//if (time > 1.0f && !spCheck)
		//{
		//	actionPoint += M_BossInfo.GetTableExcel().Speed;

		//	if (actionPoint > M_BossInfo.GetTableExcel().MaxTM)
		//	{
		//		int a = actionPoint - M_BossInfo.GetTableExcel().MaxTM;

		//		//MaxTM을 넘기 않게.
		//		actionPoint -= a;

		//	}
		//}

		switch (bossState)
		{
			case BossState.NOMAL:
				//이 상태에서는 맞아도 가만히 있음(그로기? 같은상태) -> SP를 채우고 있는중 == 플레이어의 공격타이밍.
				StateNomal();
				break;
			case BossState.CC:

				break;
			case BossState.DIE:
                StateDie();     

                break;
			case BossState.IDLE:
				//SP가 다 채워져 있거나 남아있는 상태, 행동의 변화를 대기하는 상태 -> 이때 피격 되면 전투상태로 바로 들어간다.  
				StateIdle();
				break;

			case BossState.MOVE:
				
				break;
			case BossState.BUFF:
				//보스에게 유리한 버프(방어력증가,체력회복,공격력증가등등)
				break;
			case BossState.BATTLE:
				//전투 상태 ->
				break;

			case BossState.ATTACK:
				//전투의 공격상태(기본 공격,스킬쿨타임에 맞춰 스킬?)
				break;
			case BossState.AGGRO:

				break;
			case BossState.HIT:
                StateHit();

                break;
			case BossState.DETECTION:
				//배틀 상태에서 적이 있는지 찾는상태(?) -> 굳이? 메서드로 빼버리는게 맞는것 같은데.
				break;
		}


	}
}
