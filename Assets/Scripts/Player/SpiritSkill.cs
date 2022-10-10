using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스킬은 공통된 개념 -> 

public class SpiritSkill : MonoBehaviour
{ 
	public GameObject[] EffectPrefab;
    private int effectNumber = 0;

	private StageManager m_StageMgr;


    private int Row;
    private int Column;

	public void SkillUse(SpiritSkill_TableExcel skillInfo,GameObject Spirit)  //비타일형
	{
        //현재 플레이어의 좌표
        Row = MainManager.instance.GetStageManager().m_PlayerRow;
        Column = MainManager.instance.GetStageManager().m_PlayerCoulmn;
        
        GameObject tempSpirit = Spirit;


        switch (skillInfo.SpritSkillIndex)
        {
            case 170001:  //얼음장판
                effectNumber = 0;
                break;
            case 170002:  //독구름
                effectNumber = 1;
                break;
            case 170003:  //무적
                effectNumber = 2;
                break;
            case 170004:  //신성지대
                effectNumber = 3;
                break;
            case 170005: //힐
                effectNumber = 4;
                break;
            case 170006: //이속증가
                effectNumber = 5;
                break;
        }

        EffectPrefab[effectNumber] = PrefabLoader.Instance.PrefabDic[skillInfo.LunchPrefb];

        DamageCheck check;
        check = EffectPrefab[effectNumber].GetComponent<DamageCheck>();

        if(check == null)
        {
            EffectPrefab[effectNumber].AddComponent<DamageCheck>();
        }



        switch (skillInfo.SpritSkillIndex)
		{
			case 170001:  //얼음장판
                StartCoroutine(IceField(skillInfo, Row, Column));
                break;
			case 170002:  //독구름
                StartCoroutine(PoisonCloud(skillInfo, tempSpirit));
                break;
			case 170003:  //무적
                StartCoroutine(Invincibility(skillInfo, tempSpirit));
                break;
			case 170004:  //신성지대
                StartCoroutine(Sanctity(skillInfo, tempSpirit));
                break;
            case 170005: //힐
                StartCoroutine(Heal(skillInfo, tempSpirit));
                break;
            case 170006: //이속증가
                StartCoroutine(SpeedField(skillInfo, Row,Column));
                break;
        }

	}


    IEnumerator SpeedField(SpiritSkill_TableExcel skill, int row, int column)
    {
        int Row = row;
        int Column = column;

        float spirit_time = 0f;

        Debug.Log("스피드 필드 실행");
        float range = skill.SkillRange - 1.0f;
        float xRange = skill.SkillRange + range;

        //정령의 스킬범위를 표시해 주는 부분.
        if (range > 0)  //range는 -1을한값 범위가 2부터 여기 들어온다. 범위가 1일경우는 해당 타일에 계산하면 된다.
        {
            int saveRow = Row;
            int saveColumn = 0;

            int checkRow_P;   //정령 기준 아래쪽에 있는 Column값
            int checkRow_M;   //정령 기준 위쪽에 있는 Column값
            int checkColumn;  //현재 색을 바꿀 타일의 Column값

            for (float i = 0; i < skill.SkillRange; i += 1.0f)
            {

                checkRow_P = Row + (int)i;
                checkRow_M = Row - (int)i;


                if (checkRow_P % 2 == 1) //024(135라인)
                    saveColumn++;

                for (float j = 0; j < xRange; j += 1.0f)
                {

                    if (i != 0) //정령이 있는 라인의 +-1라인씩 그림.
                    {

                        checkColumn = saveColumn + (int)j;

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        if (checkRow_P < 5)
                        {
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].SpiritEffect = true;

                        }

                        if (checkRow_M >= 0)
                        {
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].SpiritEffect = true;

                        }

                    }
                    else  //정령이 있는 라인을 쭉그림.
                    {

                        checkColumn = Column - (int)range + (int)j;

                        if (j == 0)
                            saveColumn = checkColumn;   //정령라인에서 첫번째 타일 색변환위치 저장.

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        m_StageMgr.m_MapInfo[Row, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                        m_StageMgr.m_MapInfo[Row, checkColumn].SpiritEffect = true;
                    }


                }

                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
            m_StageMgr.m_MapInfo[Row, Column].SpiritEffect = true;

        }

        //경고시간(모든스킬 0.5f초로 고정)이  경과하면 파란색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].SpiritEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(EffectPrefab[5]);

                    effect.GetComponent<DamageCheck>().Dot = skill.DoT;
                    effect.GetComponent<DamageCheck>().who = 1;

                    //버프 스킬이 있는지 확인후 스킬실행.
                    if (skill.BuffAdded != 0)
                    {

                        effect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
                    }


                    //GameObject effect = Instantiate(PrefabLoader.Instance.PrefabDic[skill.LunchPrefb]);


                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    m_StageMgr.m_MapInfo[i, j].SpiritEffectObject = effect;
                    
                }
            }
        }

        float speed;
        float originspeed = this.gameObject.GetComponent<CharacterClass>().m_CharacterStat.MoveSpeed;

        while (true)
        {
            //지속시간 체크
            spirit_time += Time.deltaTime;

            //정령 지속시간이 경과시 
            if (spirit_time >= skill.LifeTime)
            {
                //이펙트 파괴
                for (int i = 0; i < m_StageMgr.mapZ; i++)
                {
                    for (int j = 0; j < m_StageMgr.mapX; j++)
                    {

                        if (m_StageMgr.m_MapInfo[i, j].SpiritEffect)
                        {
                            m_StageMgr.m_MapInfo[i, j].SpiritEffect = false;
                            Object.Destroy(m_StageMgr.m_MapInfo[i, j].SpiritEffectObject);
                           
                        }
                    }
                }

                this.gameObject.GetComponent<CharacterClass>().m_CharacterStat.MoveSpeed = originspeed;


                //연계스킬있는지 확인후 다시 스킬실행.
                if (skill.SkillAdded != 0)
                {

                }


                yield break;
            }

           if(MainManager.Instance.GetStageManager().GetPlayerMapInfo().SpiritEffect)
           {
                //transform.Find("Player").transform.GetChild(0).GetComponent<CharacterClass>().m_CharacterStat.MoveSpeed = 20f;

                speed = originspeed;
                speed *= 2f;


                this.gameObject.GetComponent<CharacterClass>().m_CharacterStat.MoveSpeed = speed;

           }
           else
           {
                this.gameObject.GetComponent<CharacterClass>().m_CharacterStat.MoveSpeed = originspeed;
           }

           
            
           yield return null;
        }


       
    }


    IEnumerator IceField(SpiritSkill_TableExcel skill, int row, int column)
    {
        int Row = row;
        int Column = column;

        float spirit_time = 0f;

        Debug.Log("아이스 필드 실행");
        float range = skill.SkillRange - 1.0f;
        float xRange = skill.SkillRange + range;

        //정령의 스킬범위를 표시해 주는 부분.
        if (range > 0)  //range는 -1을한값 범위가 2부터 여기 들어온다. 범위가 1일경우는 해당 타일에 계산하면 된다.
        {
            int saveRow = Row;
            int saveColumn = 0;

            int checkRow_P;   //정령 기준 아래쪽에 있는 Column값
            int checkRow_M;   //정령 기준 위쪽에 있는 Column값
            int checkColumn;  //현재 색을 바꿀 타일의 Column값

            for (float i = 0; i < skill.SkillRange; i += 1.0f)
            {

                checkRow_P = Row + (int)i;
                checkRow_M = Row - (int)i;


                if (checkRow_P % 2 == 1) //024(135라인)
                    saveColumn++;

                for (float j = 0; j < xRange; j += 1.0f)
                {

                    if (i != 0) //정령이 있는 라인의 +-1라인씩 그림.
                    {

                        checkColumn = saveColumn + (int)j;

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        if (checkRow_P < 5)
                        {
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].SpiritEffect = true;

                        }

                        if (checkRow_M >= 0)
                        {
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].SpiritEffect = true;

                        }

                    }
                    else  //정령이 있는 라인을 쭉그림.
                    {

                        checkColumn = Column - (int)range + (int)j;

                        if (j == 0)
                            saveColumn = checkColumn;   //정령라인에서 첫번째 타일 색변환위치 저장.

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        m_StageMgr.m_MapInfo[Row, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                        m_StageMgr.m_MapInfo[Row, checkColumn].SpiritEffect = true;
                    }


                }

                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
            m_StageMgr.m_MapInfo[Row, Column].SpiritEffect = true;

        }

        //경고시간(모든스킬 0.5f초로 고정)이  경과하면 파란색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].SpiritEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(EffectPrefab[0]);

                    effect.GetComponent<DamageCheck>().Dot = skill.DoT;
                    effect.GetComponent<DamageCheck>().who = 1;
                    //버프 스킬이 있는지 확인후 스킬실행.
                    if (skill.BuffAdded != 0)
                    {

                        effect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
                    }

                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    m_StageMgr.m_MapInfo[i, j].SpiritEffectObject = effect;
                   
                }
            }
        }

        Debug.Log("이펙트 소환");



        while(true)
        {
            //지속시간 체크
            spirit_time += Time.deltaTime;

            //정령 지속시간이 경과시 
            if (spirit_time >= skill.LifeTime)
            {
                //이펙트 파괴
                for (int i = 0; i < m_StageMgr.mapZ; i++)
                {
                    for (int j = 0; j < m_StageMgr.mapX; j++)
                    {

                        if (m_StageMgr.m_MapInfo[i, j].SpiritEffect)
                        {
                            m_StageMgr.m_MapInfo[i, j].SpiritEffect = false;
                            Object.Destroy(m_StageMgr.m_MapInfo[i, j].SpiritEffectObject);
                           
                        }
                    }
                }


                //연계스킬있는지 확인후 다시 스킬실행.
                if (skill.SkillAdded != 0)
                {

                }


                yield break;
            }


            yield return null;
        }


     

    }


	IEnumerator Heal(SpiritSkill_TableExcel skill, GameObject spirit)
	{
        Debug.Log("힐 실행");
        GameObject tempEffect = Instantiate(EffectPrefab[4]);
        tempEffect.GetComponent<DamageCheck>().Dot = skill.DoT;
        tempEffect.GetComponent<DamageCheck>().who = 1;

        //버프 스킬이 있는지 확인후 스킬실행.
        if (skill.BuffAdded != 0)
        {

            tempEffect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
        }



        tempEffect.transform.position = spirit.transform.position;
		Collider[] colls = null;

		float spirit_time = 0f;
		float buff_Time = 0f;

		while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;
			buff_Time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= skill.LifeTime)
			{
				//정령 파괴후 코루틴 종료
				Object.Destroy(tempEffect);
				yield break;
			}

			if(buff_Time >= skill.DoT)
			{
				buff_Time = 0f;

                colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 14);  //11번째 레이어 = Player    ,14번째 레이어 메인카메라.

                //colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange);

				foreach (var rangeCollider in colls)
				{
                    //플레이어 회복시키는 코드.
                    rangeCollider.GetComponent<CharacterClass>().m_CharacterStat.HP += 100f;
                    //rangeCollider.transform.GetChild(0).GetComponent<CharacterClass>().m_CharacterStat.HP += 100f;
                    //100씩 회복.
                    //rangeCollider.GetComponent<PlayerManager>().Damage();
                    Debug.Log("플레이어 회복중");

                }

			}
			yield return null;
		}

	}

    //밀려나는 코드
	IEnumerator Sanctity(SpiritSkill_TableExcel skill, GameObject spirit)
	{
        Debug.Log("신성지대 실행");
		GameObject tempEffect = Instantiate(EffectPrefab[3]);
        tempEffect.transform.position = spirit.transform.position;
		Collider[] colls = null;

		float spirit_time = 0f;

		while(true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= skill.LifeTime)
			{
				//정령 파괴후 코루틴 종료
				Object.Destroy(tempEffect);
				yield break;
			}


			colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 9);  //9번째 레이어 = Enemy  //콜라이더가 없어서 OverlapSphere가 안됨. 잡몹한테 콜라이더 부착.



           

			if(colls != null)
			{
				foreach (var rangeCollider in colls)
				{

					//보스가 아니라면
					if (rangeCollider.gameObject.name != "931001(Clone)")  //태그나 레이어로 바꿔야함   => 931001(Clone)는 보스
					{
                        Debug.Log(rangeCollider);
                        //밀리는 물체와 밀리는 마지막 점과의 거리(힘)을 구하고

                        //정령과 밀리는 물체사이의 방향을 구해서
                        var heading = rangeCollider.transform.position - spirit.transform.position;
                        heading.y = 0f;
                        heading *= skill.SkillRange;



                        rangeCollider.gameObject.transform.position = Vector3.MoveTowards(rangeCollider.gameObject.transform.position, heading, 5f);
						Debug.Log($"{rangeCollider.ToString()}을 범위 내에서 밀어냅니다.");
					}
				}
			}

		

			yield return null;
		}

		

	}

	IEnumerator Invincibility(SpiritSkill_TableExcel skill, GameObject spirit)
	{
        Debug.Log("무적 실행");
        GameObject tempEffect = Instantiate(EffectPrefab[2]);
		tempEffect.transform.position = spirit.transform.position;


		//All 
		Collider[] colls = null;

        //8번째 레이어 = Player
        //무적버프 

        float spirit_time = 0f;

        while (true)
        {
            //지속시간 체크
            spirit_time += Time.deltaTime;

            //정령 지속시간이 경과시 
            if (spirit_time >= skill.LifeTime)
            {
                //정령 파괴후 코루틴 종료
                Object.Destroy(tempEffect);
                yield break;
            }

            colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 11);  //11번째 레이어 = Player

            if(colls.Length !=0)
            {
                Debug.Log("무적실행");

            }
            else
            {
                Debug.Log("주위의 플레이어가 없습니다");
            }

            foreach (var rangeCollider in colls)
            {
                rangeCollider.GetComponent<CharacterClass>().Invincibility = 0;
            }

         

            yield return null;
        }
	}

	
	IEnumerator PoisonCloud(SpiritSkill_TableExcel skill,GameObject spirit)
	{
        Debug.Log("독구름 실행");
        GameObject nearEnemy = FindNearbyEnemy(spirit, skill.SkillRange);
		GameObject tempEffect = null;


		if (nearEnemy  != null)
		{
			tempEffect = Instantiate(EffectPrefab[1]);
            tempEffect.GetComponent<DamageCheck>().Dot = skill.DoT;
            tempEffect.GetComponent<DamageCheck>().who = 1;

            //버프 스킬이 있는지 확인후 스킬실행.
            if (skill.BuffAdded != 0)
            {

                tempEffect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
            }
            tempEffect.transform.position = nearEnemy.transform.position + new Vector3(0, 5f, 0);
		}


		float spirit_time = 0f;
		float attack_time = 0f;

		while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;
			attack_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= skill.LifeTime)
			{
				//정령 파괴후 코루틴 종료

				if(nearEnemy != null)
				{
					nearEnemy = null;
                    Object.Destroy(tempEffect);
                }

				yield break;
			}

			if (attack_time > skill.DoT)
			{
				//도트 시간이 지나면 데미지 판정.
			}

			yield return null;
		}

	}



	GameObject FindNearbyPlayer(GameObject findStartObject, float distance)
	{
		float Dist = 0f;
		float near = 0f;
		GameObject nearEnemy = null;

		//범위 내의 적을 찾는다.
		Collider[] colls = Physics.OverlapSphere(findStartObject.transform.position, distance, 1 << 8);  //8번째 레이어 = Player
		if (colls.Length == 0)
		{
			Debug.Log("범위에 플레이어가 없습니다.");
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


	GameObject FindNearbyEnemy(GameObject findStartObject, float distance)
	{
		float Dist = 0f;
		float near = 0f;
		GameObject nearEnemy = null;

		//범위 내의 적을 찾는다.
		Collider[] colls = Physics.OverlapSphere(findStartObject.transform.position, distance, 1 << 9);  //9번째 레이어 = Enemy


		if (colls.Length == 0)
		{
			Debug.Log("범위에 적이 없습니다.");
			return null;
		}
		else
		{
			//적이 있다면 그 적들 중에
			for (int i = 0; i < colls.Length; i++)
			{
				//보스가 있을경우 보스로 고정
				if(colls[i].gameObject.name == "Boss")
				{
					nearEnemy = colls[i].gameObject;
					break;
				}

				


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


	private void Start()
	{

		m_StageMgr = MainManager.Instance.GetStageManager();
        EffectPrefab = new GameObject[6];



    }


}
