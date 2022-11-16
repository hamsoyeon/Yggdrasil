using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yggdrasil.PlayerSkillSet;  //SkillFactory.cs -> PlayerSkillSet(namespace)





public class Action : MonoBehaviour
{

	public enum SkillList
	{
		AttackDmg = 11,
		AttackDis,
		AttackSpd,

		DefenseDmg = 21,
		DefenseDis,
		DefenseSpd,

		SupportDmg = 31,
		SupportDis,
		SupportSpd

	}


	private static Action instance = null;




	public GameObject SpiritPrefab;  //임시
	//private SkillManager skillMgr;
	private Vector3 direction;




	public static Action Instance
	{
		get
		{
			if (null == instance)
			{
				return null;
			}

			return instance;
		}
	}

	GameObject FindNearbyEnemy(GameObject findStartObject, float distance)
	{
		float Dist = 0f;
		float near = 0f;
		GameObject nearEnemy = null;

		//범위 내의 적을 찾는다.
		Collider[] colls = Physics.OverlapSphere(findStartObject.transform.position, distance, 1 << 9);  //9번째 레이어 = Enemy

		if (colls.Length == 0)
		{
			DestroyObject(findStartObject);
			return null;
		}
		else
		{
			//적이 있다면 그 적들 중에
			for (int i = 0; i < colls.Length; i++)
			{
				//정령과의 거리를 계산후
				Dist = Vector3.Distance(findStartObject.transform.position, colls[i].transform.position);

				if (i == 0)
				{
					near = Dist;
					nearEnemy = colls[i].gameObject;
				}

				//그 거리가 작다면 거리를 저장하고 해당 오브젝트를 저장
				if (Dist < near)
				{
					near = Dist;
					nearEnemy = colls[i].gameObject;
				}
			}

			return nearEnemy;
		}

	}


	IEnumerator InstallSprit(GameObject spirit, float duration, float range, SkillList skill)
	{
		float spirit_time = 0f;

		//Defense-dis
		float attack_time = 0f;

		//Defen-spd
		bool range_check = true;

		//All 
		Collider[] colls = null;

		while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= duration)
			{
				//버프 해제 코드

				//정령 파괴후 코루틴 종료
				DestroyObject(spirit);
				yield break;
			}

			switch (skill)
			{
				case SkillList.DefenseDmg:
					{
						//범위내의 플레이어를 찾아서 
						colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 8);  //8번째 레이어 = Player
					}
					break;

				case SkillList.DefenseDis:
					{
						attack_time += Time.deltaTime;

						if (attack_time >= 0.8f && attack_time < 1.0f)
						{
							attack_time = 0f;

							//범위내의 적을 찾아서 
							colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy
							foreach (var rangeCollider in colls)
							{
								//밀리는 물체와 밀리는 마지막 점과의 거리(힘)을 구하고

								//플레이어와 밀리는 물체사이의 방향을 구해서
								var heading = rangeCollider.transform.position - spirit.transform.position;
								heading.y = 0f;
								heading *= range;

								//보스가 아니라면
								if (rangeCollider.gameObject.name != "Boss")
								{
									rangeCollider.gameObject.transform.position = Vector3.MoveTowards(rangeCollider.gameObject.transform.position, heading, 1.5f);
									Debug.Log($"{rangeCollider.ToString()}을 범위 내에서 밀어냅니다.");
								}
							}

						}
					}
					break;

				case SkillList.DefenseSpd:
					{
						//범위내의 플레이어를 찾아서 
						if (range_check)
						{
							colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy
							range_check = false;
						}


						foreach (var rangeCollider in colls)
						{
							//범위내에서 적용시킬 버프 내용

							//디버깅
							Debug.Log($"{rangeCollider.ToString()}의 이동속도를 0으로(스턴효과)를 줍니다.");
						}
					}
					break;

				case SkillList.SupportDmg:
					{
						//범위내의 적을 찾아서 
						colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy
						foreach (var rangeCollider in colls)
						{
							//범위내에서 적용시킬 디버프 내용

							//디버깅
							Debug.Log($"{rangeCollider.name}은 입는 피해가 20% 증가합니다.");
						}
					}
					break;

			}

			yield return null;
		}

	}

	IEnumerator TrackingSpirit(GameObject spirit, GameObject nearbyEnemy, float duration, float range, SkillList skill)
	{

		float spirit_time = 0f;

		//Attack-dis,Attack-spd
		float attack_time = 0f;


		if (skill != SkillList.AttackDis)
		{
			//적에게 이동.
			while (true)
			{
				spirit.transform.position = Vector3.MoveTowards(spirit.transform.position, nearbyEnemy.transform.position, 0.1f);

				if (spirit.transform.position == nearbyEnemy.transform.position)
					break;

				yield return null;
			}
		}



		while (true)
		{
			spirit_time += Time.deltaTime;

			if (spirit_time >= duration)
			{
				DestroyObject(spirit);
				yield break;
			}

			switch (skill)
			{
				case SkillList.AttackDmg:
					{
						int randvalue = 0;

						//지정범위내의 모든 에너미(몹들만)를 찾은 후
						Collider[] colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy

						for (int i = 0; i < 10; i++)
						{
							randvalue = Random.Range(0, colls.Length);

							//같은 대상에게 피해를 입힐시 20% 감소된 피해를 주는 코드 추가.
							Debug.Log($"{colls[randvalue].name}에게 대미지를 줍니다");

						}
						DestroyObject(spirit);
						yield break;

					}
				case SkillList.AttackDis:
					{
						attack_time += Time.deltaTime;

						if (attack_time >= 0.5f)
						{
							attack_time = 0f;

							spirit.transform.position = Vector3.MoveTowards(spirit.transform.position, nearbyEnemy.transform.position + direction, 1.5f);

							Collider[] colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy

							foreach (var rangeCollider in colls)
							{
								//대미지 코드
								Debug.Log($"{rangeCollider.name}에게 피해를 줍니다.");
							}

						}


					}
					break;
				case SkillList.AttackSpd:
					{
						attack_time += Time.deltaTime;

						if (attack_time >= 0.33f)
						{
							attack_time = 0f;
							//지정범위내의 모든 에너미를 찾은 후
							Collider[] colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy

							foreach (var rangeCollider in colls)
							{
								//대미지 코드
								Debug.Log($"{rangeCollider.name}에게 장판 피해를 줍니다.");
							}

						}
					}
					break;
				case SkillList.SupportDis:
					{
						if (spirit_time < 0.75f)
						{
							Collider[] colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy
							foreach (var rangeCollider in colls)
							{
								//보스가 아니라면 + 위치가 같지 않다면 그 대상을 끌고온다.
								if (rangeCollider.gameObject.name != "Boss" && rangeCollider.transform.position != spirit.transform.position)
								{
									rangeCollider.gameObject.transform.position = Vector3.MoveTowards(rangeCollider.gameObject.transform.position, spirit.transform.position, 0.05f);
								}
							}
						}


						if (spirit_time >= 0.75f)
						{
							//나머지 시간동안 이동속도 0으로 만드는 코드.
							Collider[] colls = Physics.OverlapSphere(spirit.transform.position, 0.5f, 1 << 9);  //9번째 레이어 = Enemy
							foreach (var rangeCollider in colls)
							{
								Debug.Log($"{rangeCollider.name}는 스턴에 걸렸습니다.");
							}
						}
					}

					break;
				case SkillList.SupportSpd:
					{
						//범위내의 적을 찾는다.
						Collider[] colls = Physics.OverlapSphere(spirit.transform.position, range, 1 << 9);  //9번째 레이어 = Enemy
						foreach (var rangeCollider in colls)
						{
							//여기서 이동속도 감소 하는 내용 추가.

							//디버깅
							Debug.Log($"{rangeCollider.name}의 이동속도는 -50% 감소합니다.");
						}

					}
					break;
			}

			yield return null;
		}


	}


	#region SupportSkill 
	public void SupportDamage()  //지원-피해
	{
		GameObject SupDmg_Spirit = Instantiate(SpiritPrefab);

		//정령을 플레이어 위치에 이동.  -> 나중에 현재있는 타일 중앙에 위치시켜야함(이미 타일중앙에 정령이 있을경우 설치불가)
		SupDmg_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		StartCoroutine(InstallSprit(SupDmg_Spirit, 2f, 5f, SkillList.SupportDmg));
	}


	public void SupportDistance() //지원-거리
	{
		GameObject nearEnemy = null;
		GameObject SupDis_Spirit = Instantiate(SpiritPrefab);
		//정령을 플레이어 위치에 이동
		SupDis_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		//근처에 적을 찾는다
		nearEnemy = FindNearbyEnemy(SupDis_Spirit, 15f);

		if (nearEnemy == null)
			return;
		else
			StartCoroutine(TrackingSpirit(SupDis_Spirit, nearEnemy, 1.5f, 5f, SkillList.SupportDis));
	}

	public void SupportSpeed()  //지원-이동
	{
		GameObject nearEnemy = null;
		GameObject SupSpeed_Spirit = Instantiate(SpiritPrefab);
		//정령을 플레이어 위치에 이동
		SupSpeed_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		//근처에 적을 찾는다
		nearEnemy = FindNearbyEnemy(SupSpeed_Spirit, 15f);

		if (nearEnemy == null)
			return;
		else
			StartCoroutine(TrackingSpirit(SupSpeed_Spirit, nearEnemy, 2f, 15f, SkillList.SupportSpd));

	}
	#endregion

	#region AttackSkill
	public void AttackDamage()
	{
		GameObject nearEnemy = null;
		GameObject AtkDis_Spirit = Instantiate(SpiritPrefab);
		//정령을 플레이어 위치에 이동
		AtkDis_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		//근처에 적을 찾는다
		nearEnemy = FindNearbyEnemy(AtkDis_Spirit, 15f);

		if (nearEnemy == null)
			return;
		else
			StartCoroutine(TrackingSpirit(AtkDis_Spirit, nearEnemy, 0.1f, 5f, SkillList.AttackDmg));

	}

	public void AttackDistance()
	{
		GameObject nearEnemy = null;
		GameObject AtkDmg_Spirit = Instantiate(SpiritPrefab);
		//정령을 플레이어 위치에 이동
		AtkDmg_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		//근처에 적을 찾는다
		nearEnemy = FindNearbyEnemy(AtkDmg_Spirit, 10f);

		if (nearEnemy == null)
			return;
		else
		{
			var heading = nearEnemy.transform.position - AtkDmg_Spirit.transform.position;
			var distance = heading.magnitude;

			direction = heading / distance;
			direction.y = 0;
			direction *= 6f;

			StartCoroutine(TrackingSpirit(AtkDmg_Spirit, nearEnemy, 2.2f, 5f, SkillList.AttackDis));
		}

	}


	public void AttackSpeed()
	{
		GameObject nearEnemy = null;
		GameObject AtkSpeed_Spirit = Instantiate(SpiritPrefab);
		//정령을 플레이어 위치에 이동
		AtkSpeed_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		//근처에 적을 찾는다
		nearEnemy = FindNearbyEnemy(AtkSpeed_Spirit, 10f);

		AtkSpeed_Spirit.transform.position = nearEnemy.transform.position;

		if (nearEnemy == null)
			return;
		else
			StartCoroutine(TrackingSpirit(AtkSpeed_Spirit, nearEnemy, 3.2f, 3, SkillList.AttackSpd));

	}

	#endregion

	#region DefenseSkill
	public void DefenseDamage()
	{
		GameObject DefDmg_Spirit = Instantiate(SpiritPrefab);

		//정령을 플레이어 위치에 이동.  -> 나중에 현재있는 타일 중앙에 위치시켜야함(이미 타일중앙에 정령이 있을경우 설치불가)
		DefDmg_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		StartCoroutine(InstallSprit(DefDmg_Spirit, 1.5f, 3f, SkillList.DefenseDmg));
	}

	public void DefenseDistance()
	{
		GameObject DefDis_Spirit = Instantiate(SpiritPrefab);

		//정령을 플레이어 위치에 이동.  -> 나중에 현재있는 타일 중앙에 위치시켜야함(이미 타일중앙에 정령이 있을경우 설치불가)
		DefDis_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		StartCoroutine(InstallSprit(DefDis_Spirit, 3.2f, 5f, SkillList.DefenseDis));
	}

	public void DefenseSpeed()
	{
		GameObject DefSpd_Spirit = Instantiate(SpiritPrefab);

		//정령을 플레이어 위치에 이동.  -> 나중에 현재있는 타일 중앙에 위치시켜야함(이미 타일중앙에 정령이 있을경우 설치불가)
		DefSpd_Spirit.transform.position = PlayerManager.p_Object.transform.position;

		StartCoroutine(InstallSprit(DefSpd_Spirit, 2.0f, 5f, SkillList.DefenseSpd));
	}
    #endregion

    float rotateSpeed = 5.0f;



    public void SummonDirection(GameObject summon, GameObject target)
    {
        summon.transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(target.transform.position),
            Time.deltaTime * rotateSpeed);
    }


	private void Awake()
	{
		if (null == instance)
		{
			instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
		else
		{
			Destroy(this.gameObject);
		}

		//skillMgr = new SkillManager();
	}


	// Start is called before the first frame update
	void Start()
	{

	}


	// Update is called once per frame
	void Update()
	{

	}
}
