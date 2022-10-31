using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class SpiritSkill : MonoBehaviour
{

    enum SkillNumber { ICE=0, POISON ,INVINCIBILITY ,SANCTITY, HEAL ,SPEED }

    enum SkillType { ATTACK=1, WIDE_MOVE, TARGET, WIDE_FIX, TILE  }

	public GameObject[] Lunch_Prefabs;  //정령의 PosAtk에서 나오는 것.
    public GameObject[] Fire_Prefabs;
    public GameObject[] Damage_Prefabs;

    public Transform[] PosAtk;

    public GameObject[] LunchObjects;

    private int effectNumber = 0;

	private StageManager m_StageMgr;


    private int Row;
    private int Column;

    CharacterClass player_characterclass = null;

    public void SkillUse(SpiritSkill_TableExcel skillInfo, GameObject Spirit)  //비타일형
	{

        //현재 플레이어의 좌표
        Row = MainManager.instance.GetStageManager().m_PlayerRow;
        Column = MainManager.instance.GetStageManager().m_PlayerCoulmn;
        
        GameObject tempSpirit = Spirit;

        effectNumber = skillInfo.SpritSkillIndex - 170001;

       
        if(PosAtk[effectNumber] == null)
        {
            Transform[] allChildren = Spirit.GetComponentsInChildren<Transform>();

            foreach (Transform child in allChildren)
            {
                if (child.name == "PosAtk")
                {
                    PosAtk[effectNumber] = child;
                }
            }
        }

        Lunch_Prefabs[effectNumber] = PrefabLoader.Instance.PrefabDic[skillInfo.LunchPrefb];
        Fire_Prefabs[effectNumber] = PrefabLoader.Instance.PrefabDic[skillInfo.FirePrefb];
        Damage_Prefabs[effectNumber] = PrefabLoader.Instance.PrefabDic[skillInfo.DamPrefb];


        LockOn LockCheck;
        LockCheck = Fire_Prefabs[effectNumber].GetComponent<LockOn>();  
        if(LockCheck ==null)
        {
            Fire_Prefabs[effectNumber].AddComponent<LockOn>();
            //LockCheck = Fire_Prefabs[effectNumber].GetComponent<LockOn>();
        }


        // 데미지 체크 가능하게
        DamageCheck check;
        check = Fire_Prefabs[effectNumber].GetComponent<DamageCheck>();
        if(check == null)
        {
            Fire_Prefabs[effectNumber].AddComponent<DamageCheck>();
            check = Fire_Prefabs[effectNumber].GetComponent<DamageCheck>();
        }

        // 데미지 프리팹 셋팅 및 데미지 체크 가능하게
        check.dmg_check = true;
        check.DamageEffect = Damage_Prefabs[effectNumber];

        // 데미지 프리팹 설정 셋
        DamageEffect dmgCheck;
        dmgCheck = Damage_Prefabs[effectNumber].GetComponent<DamageEffect>();
        if(dmgCheck == null)
        {
            Damage_Prefabs[effectNumber].AddComponent<DamageEffect>();
        }



        // 이건 스킬 고정형태..
        
        switch (skillInfo.SpritSkillIndex)
		{
			case 170001:  //얼음장판
                StartCoroutine(IceField(skillInfo, Row, Column,effectNumber));
                break;
			case 170002:  //독구름
                StartCoroutine(PoisonCloud(skillInfo, tempSpirit, effectNumber));
                break;
			case 170003:  //무적
                StartCoroutine(Invincibility(skillInfo, tempSpirit, effectNumber));
                break;
			case 170004:  //신성지대
                StartCoroutine(Sanctity(skillInfo, tempSpirit, effectNumber));
                break;
            case 170005: //힐
                StartCoroutine(Heal(skillInfo, tempSpirit, effectNumber));
                break;
            case 170006: //이속증가
                StartCoroutine(SpeedField(skillInfo, Row,Column, effectNumber));
                break;
        }
        

        // 엑셀데이터에서 불러와 만들어지는 가변행태로 만들기.

        // skillInfo.SkillType  // 1-> 근접공격(파이어 프리팹 안씀,대미지 프리팹만 대상에게 출력)
        // 2-> 광역(cshape1 = 부채꼴 각도, cshape2 = 부채꼴의 넓이)  -> 가장 가까운 적을 찾아서 그 적을 추격하며 브레스? 형태의 스킬. 
        // 3-> 원거리 공격(range안에 있는 가장 가까운 적을 TargetNum값만큼 찾아서 fireFrepab실행)
        // 4-> 광역(2번과 똑같지만 고정 브레스 형태의 스킬)
        // 5-> 타일형(보스의 1번과 동일하게) 추후의 보스의 스킬 4가지형태로 추가하는 방향

        // 나중에 스킬셋형태로 보스가 1번 스킬을 사용할때(ex. 4개의 스킬을 들고있는 엑셀 데이터를 불러와서) 거기서 랜덤으로 스킬을 사용하는 ... 작업이 끝나면 추가 예정..
        // 모든 위치 추적 스킬은 보스를 가장 최우선으로 한다..

        //switch ((SkillType)skillInfo.SkillType)
        //{
        //    case SkillType.ATTACK:
        //        StartCoroutine(Spirit_Attack(skillInfo));
        //        break;
        //    case SkillType.WIDE_MOVE:
        //        StartCoroutine(Spirit_Wide_Move(skillInfo));
        //        break;
        //    case SkillType.TARGET:
        //        StartCoroutine(Spirit_Target(skillInfo,tempSpirit));
        //        break;
        //    case SkillType.WIDE_FIX:
        //        StartCoroutine(Spirit_Wide_Fix(skillInfo));
        //        break;
        //    case SkillType.TILE:
        //        StartCoroutine(Spirit_Tile(skillInfo));
        //        break;
        //}

    }



    IEnumerator Spirit_Attack(SpiritSkill_TableExcel skill)
    {


        yield return null;
    }


    IEnumerator Spirit_Wide_Move(SpiritSkill_TableExcel skill)
    {


        yield return null;
    }


    IEnumerator Spirit_Target(SpiritSkill_TableExcel skill, GameObject spirit)
    {
        Debug.Log("정령 타겟스킬 실행");
        // 정령이 소환된 후.

        int number = (int)skill.TargetNum;

        List<GameObject> findEnemys = null;

        // 근처에 가장 가까운 적을 찾는다.
        findEnemys = FindNearbyEnemys(spirit.transform.position, skill.SkillRange, number);
        
        
        if (findEnemys == null) // 널이라면
        {
            StopCoroutine("Spirit_Target");
        }
        else  // 널이 아니라면.
        {

            if(findEnemys.Count <= 0) //적을 발견하지 못했을 경우..
            {
                StopCoroutine("Spirit_Target");
            }
            else
            {

                Debug.Log("찾은 적의 Count:" + findEnemys.Count);

                // 디버깅용
                int a = 1;
                foreach (GameObject enemy in findEnemys)
                {
                    Debug.Log("찾은 적의 이름:" + a + "." + enemy.name + "적의 포지션 값:" + enemy.transform.position);
                    a++;
                }


                GameObject effect = null;

                for (int i = 0; i < findEnemys.Count; i++)
                {
                    effect = Instantiate(Fire_Prefabs[effectNumber]);
                    effect.transform.position = spirit.transform.position;
                    effect.GetComponent<LockOn>().m_lockOn = true;
                    effect.GetComponent<LockOn>().target = findEnemys[i];
                    effect.GetComponent<LockOn>().moveSpeed = skill.BulletSpeed;


                    // 타겟이 2(적군)일 경우에 데미지 셋팅을 해줌.
                    if (skill.Target == 2)
                    {
                        effect.GetComponent<DamageCheck>().Dot = skill.DoT;
                        effect.GetComponent<DamageCheck>().who = 1;
                    }
                }

                Destroy(effect);

                // 딜레이 및 LunchPrefab셋팅
                DelayAndLunchPrefabSet(effectNumber);

                float time = 0f;
                while (true)
                {

                    time += Time.deltaTime;

                    if (time > skill.LifeTime)
                    {
                        Destroy(LunchObjects[effectNumber]);
                        yield break;
                    }
                   
                    yield return null;
                }
            }
        }

        yield return null;
    }

    IEnumerator Spirit_Wide_Fix(SpiritSkill_TableExcel skill)
    {


        yield return null;
    }

    IEnumerator Spirit_Tile(SpiritSkill_TableExcel skill)
    {



        yield return null;
    }



    private void DelayAndLunchPrefabSet(int number)
    {

        LunchObjects[number] = Instantiate(Lunch_Prefabs[number]);
        LunchObjects[number].transform.SetParent(PosAtk[number]);
        LunchObjects[number].transform.position = PosAtk[number].position;

    }


    IEnumerator SpeedField(SpiritSkill_TableExcel skill, int row, int column,int n)
    {
        int Row = row;
        int Column = column;
        int number = n;
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
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].SpiritEffect2 = true;

                        }

                        if (checkRow_M >= 0)
                        {
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].SpiritEffect2 = true;

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
                        m_StageMgr.m_MapInfo[Row, checkColumn].SpiritEffect2 = true;
                    }


                }

                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
            m_StageMgr.m_MapInfo[Row, Column].SpiritEffect2 = true;

        }

        //경고시간(모든스킬 0.5f초로 고정)이  경과하면 파란색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.

        DelayAndLunchPrefabSet(number);

        yield return new WaitForSeconds(2f);

        Destroy(LunchObjects[number]);


        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].SpiritEffect2)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(Fire_Prefabs[(int)SkillNumber.SPEED]);

                    effect.GetComponent<DamageCheck>().Dot = skill.DoT;
                    effect.GetComponent<DamageCheck>().who = 1;

                    //버프 스킬이 있는지 확인후 스킬실행.
                    if (skill.BuffAdded != 0)
                    {
                        effect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
                    }

                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    m_StageMgr.m_MapInfo[i, j].SpiritEffectObject2 = effect;
                    
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

                        if (m_StageMgr.m_MapInfo[i, j].SpiritEffect2)
                        {
                            m_StageMgr.m_MapInfo[i, j].SpiritEffect2 = false;
                            Object.Destroy(m_StageMgr.m_MapInfo[i, j].SpiritEffectObject2);
                           
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

           if(MainManager.Instance.GetStageManager().GetPlayerMapInfo().SpiritEffect2)
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


    IEnumerator IceField(SpiritSkill_TableExcel skill, int row, int column, int n)
    {
        int Row = row;
        int Column = column;
        int number = n;

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
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].SpiritEffect1 = true;

                        }

                        if (checkRow_M >= 0)
                        {
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.blue;
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].SpiritEffect1 = true;

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
                        m_StageMgr.m_MapInfo[Row, checkColumn].SpiritEffect1 = true;
                    }


                }

                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
            m_StageMgr.m_MapInfo[Row, Column].SpiritEffect1 = true;

        }

        DelayAndLunchPrefabSet(number);
        yield return new WaitForSeconds(2f);
        Destroy(LunchObjects[number]);

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].SpiritEffect1)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(Fire_Prefabs[(int)SkillNumber.ICE]);

                    effect.GetComponent<DamageCheck>().Dot = skill.DoT;
                    effect.GetComponent<DamageCheck>().who = 1;
                    //버프 스킬이 있는지 확인후 스킬실행.
                    if (skill.BuffAdded != 0)
                    {

                        effect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
                    }

                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    m_StageMgr.m_MapInfo[i, j].SpiritEffectObject1 = effect;
                   
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

                        if (m_StageMgr.m_MapInfo[i, j].SpiritEffect1)
                        {
                            m_StageMgr.m_MapInfo[i, j].SpiritEffect1 = false;
                            Object.Destroy(m_StageMgr.m_MapInfo[i, j].SpiritEffectObject1);
                           
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

	IEnumerator Heal(SpiritSkill_TableExcel skill, GameObject spirit, int n)
	{
        Debug.Log("힐 실행");
        GameObject tempEffect = Instantiate(Fire_Prefabs[(int)SkillNumber.HEAL]);
        tempEffect.GetComponent<DamageCheck>().Dot = skill.DoT;
        tempEffect.GetComponent<DamageCheck>().who = 1;

        int number = n;

        //버프 스킬이 있는지 확인후 스킬실행.
        if (skill.BuffAdded != 0)
        {

            tempEffect.GetComponent<DamageCheck>().buffIndex = skill.BuffAdded;
        }

        DelayAndLunchPrefabSet(number);

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
                Destroy(LunchObjects[number]);
                yield break;
			}

			if(buff_Time >= skill.DoT)
			{
				buff_Time = 0f;

                colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 14);  //11번째 레이어 = Player    ,14번째 레이어 메인카메라.

                //colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange);

				foreach (var rangeCollider in colls)
				{
                    CharacterClass cc = rangeCollider.GetComponent<CharacterClass>();
                    PlayerManager pm = rangeCollider.GetComponent<PlayerManager>();


                    //플레이어 회복시키는 코드.
                    if(pm.GetMaxHp() > cc.m_CharacterStat.HP)
                    {
                        int heal = (int)(pm.GetMaxHp() - cc.m_CharacterStat.HP);

                        if(heal >= 50)
                        {
                            rangeCollider.GetComponent<CharacterClass>().m_CharacterStat.HP += 50f;
                        }
                        else
                        {
                            rangeCollider.GetComponent<CharacterClass>().m_CharacterStat.HP += heal;
                        }

                        Debug.Log("플레이어 회복중");
                    }
                    else
                    {
                        Debug.Log("플레이어의 HP가 MAX 입니다.");
                    }

                }

			}
			yield return null;
		}

	}

    //밀려나는 코드
	IEnumerator Sanctity(SpiritSkill_TableExcel skill, GameObject spirit, int n)
	{
        Debug.Log("신성지대 실행");
		GameObject tempEffect = Instantiate(Fire_Prefabs[(int)SkillNumber.SANCTITY]);
        tempEffect.transform.position = spirit.transform.position;
		Collider[] colls = null;

        int number = n;
        float spirit_time = 0f;

        DelayAndLunchPrefabSet(number);

        while (true)
		{
			//지속시간 체크
			spirit_time += Time.deltaTime;

			//정령 지속시간이 경과시 
			if (spirit_time >= skill.LifeTime)
			{
				//정령 파괴후 코루틴 종료
				Object.Destroy(tempEffect);
                Destroy(LunchObjects[number]);
                yield break;
			}

            colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 9);  //9번째 레이어 = Enemy  //콜라이더가 없어서 OverlapSphere가 안됨. 잡몹한테 콜라이더 부착.
            
           

            if (colls != null)
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

                        rangeCollider.gameObject.transform.position = Vector3.MoveTowards(rangeCollider.gameObject.transform.position, heading, 15f);
						Debug.Log($"{rangeCollider.ToString()}을 범위 내에서 밀어냅니다.");
					}
				}
			}

		

			yield return null;
		}

		

	}

	IEnumerator Invincibility(SpiritSkill_TableExcel skill, GameObject spirit, int n)
	{
        Debug.Log("무적 실행");
        GameObject tempEffect = Instantiate(Fire_Prefabs[(int)SkillNumber.INVINCIBILITY]);
		tempEffect.transform.position = spirit.transform.position;

        int number = n;

        //All 
        Collider[] colls = null;

        //8번째 레이어 = Player
        //무적버프 

        float spirit_time = 0f;

        

        DelayAndLunchPrefabSet(number);

        while (true)
        {
            //지속시간 체크
            spirit_time += Time.deltaTime;

            //정령 지속시간이 경과시 
            if (spirit_time >= skill.LifeTime)
            {
                //정령 파괴후 코루틴 종료
                Object.Destroy(tempEffect);
                Destroy(LunchObjects[number]);

                if (player_characterclass != null)
                {
                    player_characterclass.Invincibility = 1.0f;
                }
                    
                yield break;
            }

            // 현재 무적이 안되는 버그가 있음..
            //colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 11);  //11번째 레이어 = Player
            colls = Physics.OverlapSphere(spirit.transform.position, skill.SkillRange, 1 << 14);

            if (colls.Length > 0)
            {

                foreach(var p in colls)
                {

                    if (p.name != "911001(Clone)")
                        continue;
                    else
                    {
                        player_characterclass = p.GetComponent<CharacterClass>();
                        player_characterclass.Invincibility = 0.0f;
                        //Debug.Log("플레이어 무적 계수 :" + player_characterclass.Invincibility);
                        //Debug.Log("플레이어 무적실행...");
                    }
                }
            }
            else
            {
                Debug.Log("주위의 플레이어가 없습니다");
            }

            yield return null;
        }
	}

	
	IEnumerator PoisonCloud(SpiritSkill_TableExcel skill,GameObject spirit, int n)
	{
        Debug.Log("독구름 실행");
        GameObject nearEnemy = FindNearbyEnemy(spirit, skill.SkillRange);
		GameObject tempEffect = null;

        int number = n;

        DelayAndLunchPrefabSet(number);

        if (nearEnemy  != null)
		{
			tempEffect = Instantiate(Fire_Prefabs[(int)SkillNumber.POISON]);
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

                Destroy(LunchObjects[number]);

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


    List<GameObject> FindNearbyEnemys(Vector3 StartPos, float distance , int targetNum)
    {
        float Dist = 0f;
        GameObject findBoss = null;

        List<GameObject> findEnemyList = new List<GameObject>();
        
        

        // 범위 내의 적을 찾는다.
        Collider[] colls = Physics.OverlapSphere(StartPos, distance, 1 << 9);  //9번째 레이어 = Enemy

        

        // 범위에 적이없을경우
        if (colls.Length <= 0)
        {
            Debug.Log("범위에 적이 없습니다.");
            return null;
        }
        else  // 적이 있다면 그 적들 중에
        {
            List<float> EnemyNearList = new List<float>();

            for (int i = 0; i < colls.Length; i++)
            {
                // 보스가 있을경우 보스로 고정
                if (colls[i].gameObject.name == "931001(Clone)")
                {
                    findBoss = colls[i].gameObject;
                    continue;
                }

                // 정령과의 거리를 계산후
                Dist = Vector3.Distance(StartPos, colls[i].gameObject.transform.position);

                if (i < targetNum)  // 범위내의 찾은 적이 설정한 목표치 보다 적을때 전부 리스트에 추가.
                {
                    
                    findEnemyList.Add(colls[i].gameObject);
                    EnemyNearList.Add(Dist);
                    Debug.Log(i + "번째 Object추가하기");
                }
                else // i >= targetNum    // 범위내의 찾은 적이 설정한 목표보다 초과했을 때 
                {
                   
                    if(findEnemyList.Count < targetNum) // 보스때문에 i가 증가할 수 도 있음...
                    {
                        Debug.Log(i + "번째 Object추가하기");
                        findEnemyList.Add(colls[i].gameObject);
                        EnemyNearList.Add(Dist);
                    }
                    else // 그게 아니라면
                    {
                        Debug.Log(i + "번째 Object추가하기 및 요소 삭제하기.");
                        for (int j= findEnemyList.Count -1; j>0; j--)   // 현재 저장된 리스트 순회.(역for문으로 해야 오류가 안남) 정방향으로 하면 인덱스가 삭제될시 당겨지기 때문에.
                        {
                            // 그 거리가 작다면 거리를 저장하고 해당 오브젝트를 저장
                            if (Dist < EnemyNearList[j])
                            {
                                findEnemyList.RemoveAt(j);
                                EnemyNearList.RemoveAt(j);

                                //findEnemyList[j] = colls[i].gameObject;
                                //EnemyNearList[j] = Dist;
                                findEnemyList.Insert(j, colls[i].gameObject);
                                EnemyNearList.Insert(j,Dist);
                                break;
                            }
                        }

                    }

                }

            }

            if(findBoss != null)
            {

                if(findEnemyList.Count < targetNum)
                {
                    //Debug.Log("TargetNum보다 작음");
                    findEnemyList.Add(findBoss);
                }
                else
                {

                    //Debug.Log("TargetNum보다 같거나 크다");
                    int count = findEnemyList.Count-1;
                    findEnemyList.RemoveAt(count);
                    findEnemyList.Insert(count,findBoss);
                }
            }

            Debug.Log("적을 찾은 수" + findEnemyList.Count);
            return findEnemyList;
        }
    }


    private void Start()
	{

		m_StageMgr = MainManager.Instance.GetStageManager();
        Lunch_Prefabs = new GameObject[6];
        Fire_Prefabs = new GameObject[6];
        Damage_Prefabs = new GameObject[6];

        PosAtk = new Transform[6];
        LunchObjects = new GameObject[6];
    }

    // BackUp
    // NewSkill

    IEnumerator LaserFireAction(GameObject firePrefab, int PrefabNumber ,Vector3 TargetPos, float endTime)
    {

        GameObject laser = firePrefab;
        //laser.transform.position = spirit.transform.position;
        laser.transform.LookAt(TargetPos);

        //Object.Destroy(tempEffect);
       

        float time = 0f;

        while (true)
        {
            time += Time.deltaTime;

            if (time > endTime || (laser.transform.position == TargetPos ) )
            {
                Destroy(LunchObjects[PrefabNumber]);
                DestroyObject(laser);

                yield break;
            }

            laser.gameObject.transform.position = Vector3.MoveTowards(laser.gameObject.transform.position, TargetPos, 0.3f);

            yield return null;
        }

    }

    //고정 및 이동 -> 고정을 만들고
    IEnumerator SectorFormSkill()
    {


        yield return null;
    }
}
