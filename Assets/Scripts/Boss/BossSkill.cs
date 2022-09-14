using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public partial class EMath
{
    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        float Func(float x) => 4 * (-height * x * x + height * x);

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public static Vector2 Parabola(Vector2 start, Vector2 end, float height, float t)
    {
        float Func(float x) => 4 * (-height * x * x + height * x);

        var mid = Vector2.Lerp(start, end, t);

        return new Vector2(mid.x, Func(t) + Mathf.Lerp(start.y, end.y, t));
    }
}



public class BossSkill : MonoBehaviour
{

    public enum BossSkillType { WIDE = 1, TARGET, LINE, DIFFUSION, SUMMONS }

    public GameObject target;

    public GameObject LineSkillTarget;

    //public GameObject[] EnemyPrefebs = new GameObject[2];

    public GameObject MobPrefabs;


    public GameObject SkillPrefab;


    public GameObject Skill1;
    public GameObject Skill2;
    public GameObject Skill3;
    public GameObject Skill4;

    private Animator anim;
    private string currentState;

    //Animation States
    const string BOSS_IDLE = "Idle01";
    const string BOSS_SKILL01 = "Skill01";
    const string BOSS_SKILL02 = "Skill02";
    const string BOSS_SKILL03 = "Skill03";
    const string BOSS_SKILL04 = "Skill04";


    public float angleRange = 60f;
    public float distance = 5f;
    public bool isCollision = false;

    private float checkTime = 0f;
    private bool checkSkill = false;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    //Vector3 direction;

    float dotValue = 0f;

    //private BossSkillSet m_bossSkillSet;

    private int m_SkillCheck = 0;  //보스의 스킬연계가 얼만큼 진행되는지.
    private bool skillAction = true;
    private int m_BossSkillIndex;


    private int m_PlayerRow;
    private int m_PlayerColumn;

    private int m_BossRow;
    private int m_BossColumn;

    private StageManager m_StageMgr;
    private bool TargetLockOn = false;


    //private Map map;

    //private BossStat_TableExcel m_CurrentBossStat;
    public BossSkill_TableExcel m_CurrentBossSkill;


    struct ClockDirectionList
    {
        public int row;
        public int column;
        public int check;
        public bool effect;  //스킬(이펙트)을 발동할 타일.
    }

    ClockDirectionList[] OriginArr;

    public void BossSkillAction(int skillIndex)
    {
        //m_BossSkillIndex = skillIndex;

        Debug.Log("스킬진입");

        foreach (var item in DataTableManager.Instance.GetDataTable<BossSkill_TableExcelLoader>().DataList)
        {
            if (item.BossSkillIndex == skillIndex)
            {
                m_CurrentBossSkill = item; //현재 스킬 정보를 찾아낸다.
                break;
            }
        }

        //플레이어의 위치와 보스의 배열 위치를 알아낸다.
        m_PlayerRow = MainManager.Instance.GetStageManager().m_PlayerRow;
        m_PlayerColumn = MainManager.Instance.GetStageManager().m_PlayerCoulmn;
        m_BossRow = MainManager.Instance.GetStageManager().m_BossRow;
        m_BossColumn = MainManager.Instance.GetStageManager().m_BossColumn;

        SkillPrefab = PrefabLoader.Instance.PrefabDic[m_CurrentBossSkill.LunchPrefb];

        DamageCheck check;
        check = SkillPrefab.GetComponent<DamageCheck>();
        if (check == null)
        {
            SkillPrefab.AddComponent<DamageCheck>();
        }


        //StartCoroutine(SkillAction());
        SkillAction();
    }

    void SkillAction()
    {
        this.gameObject.GetComponent<BossFSM>().behavior = true;

        switch ((BossSkillType)m_CurrentBossSkill.SkillType)
        {
            case BossSkillType.WIDE:
                StartCoroutine(SkillWideAction());
                break;
            case BossSkillType.TARGET:
                StartCoroutine(SkillTargetAction());
                TargetLockOn = true;
                break;
            case BossSkillType.LINE:
                StartCoroutine(SkillLineAction());
                break;
            case BossSkillType.DIFFUSION:
                StartCoroutine(SkillDiffusionAction());
                break;
            case BossSkillType.SUMMONS:
                StartCoroutine(MobSummons());
                break;
        }
    }

    //쫄몹 소환 코루틴
    IEnumerator MobSummons()
    {
        int Row = MainManager.Instance.GetStageManager().m_BossRow; //현재 보스 Row
        int column = MainManager.Instance.GetStageManager().m_BossColumn; //현재 보스 Column

        m_StageMgr.m_MapInfo[Row, column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red; //현재 보스가 서있는 타일의 색깔을 red로 변경
        m_StageMgr.m_MapInfo[Row, column].BossEffect = true; //현재 타일의 보스 이펙트를 true로 설정

        Debug.Log("보스 스킬 범위 ");
        yield return new WaitForSeconds(2f);

        m_StageMgr.m_MapInfo[Row, column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
        GameObject effect = Instantiate(SkillPrefab);
        effect.transform.position = m_StageMgr.m_MapInfo[Row, column].MapPos + new Vector3(0, 5f, 0);
        m_StageMgr.m_MapInfo[Row, column].BossEffectObject = effect;

        //몬스터 소환.
        int m_Count = 10;
        GameObject[] monsters = new GameObject[m_Count];

        for (int i = 0; i < m_Count; i++)
        {
            float randX = Random.Range(this.gameObject.transform.position.x - 15f, this.gameObject.transform.position.x + 15f);
            float randZ = Random.Range(this.gameObject.transform.position.z - 15f, this.gameObject.transform.position.z + 15f);

            monsters[i] = Instantiate(MobPrefabs);

            monsters[i].transform.position = new Vector3(randX, 0, randZ);
        }

        yield return new WaitForSeconds(m_CurrentBossSkill.LifeTime);

        m_StageMgr.m_MapInfo[Row, column].BossEffect = false;
        Destroy(m_StageMgr.m_MapInfo[Row, column].BossEffectObject);
        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");
        this.gameObject.GetComponent<BossFSM>().behavior = false;

        yield return null;
    }

    IEnumerator SkillWideAction()
    {
        int Row = 0;
        int Column = 0;

        if (TargetLockOn)
        {
            Row = m_PlayerRow;
            Column = m_PlayerColumn;
        }
        else
        {
            Row = m_BossRow;
            Column = m_BossColumn;
        }

        Debug.Log("와이드 스킬 실행");
        float range = m_CurrentBossSkill.SkillRange - 1.0f;
        float xRange = m_CurrentBossSkill.SkillRange + range;

        //보스 스킬범위를 표시해 주는 부분.
        if (range > 0)  //range는 -1을한값 범위가 2부터 여기 들어온다. 범위가 1일경우는 해당 타일에 계산하면 된다.
        {
            // 규칙 1
            // 가운데 줄은 해당 거리+ -1 칸만큼   ex) 범위가 2인스킬의 경우 2+(2-1) = 3칸이 보스 중심의 가장 먼 거리가 되고 대칭으로 -1씩줄어들며 3칸...2칸이됨
            // ex) 범위가 3인스킬의 경우 3+(3-1) = 5칸이 보스위치의 가장 먼거리가 되고 대칭으로 -1씩 줄어들며 5칸...4칸...3칸으로 칸수가 된다.
            // 따라서 가운데 범위는 보스가 위치한 좌표를 기준으로 [z,x-(범위-1)]~ [z,x+(범위-1)]한 범위가 된다 ex) [2,2]가 보스위치고 범위가 3이라면 [2,0] ~ [2,4]의 위치로.

            // 규칙 2 
            // 보스라인이 끝나면 나머지 줄은 대칭해서  ex)  ([z-1,x] / [z+1,x]) | ([z-2,x] / [z+2,x])의     페어형식으로 증가한다.

            // 규칙 3
            // 보스의 위치가 배열의 0,2,4번째요소(1,3,5번째 줄) 일 경우 제일 마지막배열에서 row값+-1,  column값은 변경이없다. 
            // 배열의 1,3번째요소(2,4번째 줄) 일 경우 제일 마지막배열에서 row값+-1, column값 +1 값을 해줘야한다.


            int saveRow = Row;
            int saveColumn = 0;

            int checkRow_P;   //보스 기준 아래쪽에 있는 Column값
            int checkRow_M;   //보스 기준 위쪽에 있는 Column값
            int checkColumn;  //현재 색을 바꿀 타일의 Column값

            for (float i = 0; i < m_CurrentBossSkill.SkillRange; i += 1.0f)
            {

                checkRow_P = Row + (int)i;
                checkRow_M = Row - (int)i;


                if (checkRow_P % 2 == 1) //024(135라인)
                    saveColumn++;

                for (float j = 0; j < xRange; j += 1.0f)
                {

                    if (i != 0) //보스가 있는 라인의 +-1라인씩 그림.
                    {

                        checkColumn = saveColumn + (int)j;

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        if (checkRow_P < 5)
                        {
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].BossEffect = true;
                            //map.mapIndicatorArray[checkRow_P, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                        if (checkRow_M >= 0)
                        {
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].BossEffect = true;
                            //map.mapIndicatorArray[checkRow_M, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                    }
                    else  //보스가 있는 라인을 쭉그림.
                    {

                        checkColumn = Column - (int)range + (int)j;

                        if (j == 0)
                            saveColumn = checkColumn;   //보스라인에서 첫번째 타일 색변환위치 저장.

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        //map.mapIndicatorArray[m_BossRow, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[Row, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[Row, checkColumn].BossEffect = true;
                    }


                }

                //Debug.Log($"{i+1}번째 SaveColumn={saveColumn}");

                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
            m_StageMgr.m_MapInfo[Row, Column].BossEffect = true;
            //map.mapIndicatorArray[m_BossRow, m_BossColumn].GetComponent<MeshRenderer>().material.color = Color.red;
        }

        //경고시간(모든스킬 0.5f초로 고정)이  경과하면 빨간색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.
        Debug.Log("보스 스킬 범위 ");
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(SkillPrefab);
                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    effect.GetComponent<DamageCheck>().who = 2;
                    effect.GetComponent<DamageCheck>().Dot = m_CurrentBossSkill.DoT;
                    m_StageMgr.m_MapInfo[i, j].BossEffectObject = effect;
                }
            }
        }

        Debug.Log("이펙트 소환");
        //이쪽 에서 데미지 처리.
        //float time = 0f;
        //float lifeTime = 0f;
        //while(true)
        //{
        //	time += Time.deltaTime;
        //	lifeTime += Time.deltaTime;

        //	if (time >= m_CurrentBossSkill.DoT)
        //	{
        //		time = 0f;
        //		//도트마다 플레이어 위치를 체크해서 데미지를 주는방법.

        //	}
        //	if(lifeTime > m_CurrentBossSkill.LifeTime)
        //	{
        //		break;
        //	}
        //	yield return null;
        //}

        yield return new WaitForSeconds(m_CurrentBossSkill.LifeTime);  //생존시간이 지나면 이펙트 지우기

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].BossEffect = false;
                    Object.Destroy(m_StageMgr.m_MapInfo[i, j].BossEffectObject);
                }
            }
        }


        Debug.Log("와이드 스킬 종료");

        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");


        //버프가 있다면 버프를 발동.
        if(m_CurrentBossSkill.BuffAdded != 0)
        {
            MainManager.Instance.GetBuffManager().Buff(m_CurrentBossSkill.BuffAdded);
        }



        //연계스킬있는지 확인후 다시 스킬실행.
        if (m_CurrentBossSkill.SkillAdded != 0)
            BossSkillAction(m_CurrentBossSkill.SkillAdded);
        else
        {
            this.gameObject.GetComponent<BossFSM>().behavior = false;
            if (TargetLockOn)
                TargetLockOn = false;

        }
        yield break;
    }

    IEnumerator SkillTargetAction()
    {


        //타켓에서 연계스킬이 있는경우 광역,확산,도넛의 시작범위가 보스가 아니라 타겟된 대상을 기준으로 적용됨.
        Debug.Log("타겟 스킬 실행");
        float range = m_CurrentBossSkill.SkillRange - 1.0f;
        float xRange = m_CurrentBossSkill.SkillRange + range;



        //보스 스킬범위를 표시해 주는 부분.
        if (range > 0)  //range는 -1을한값 범위가 2부터 여기 들어온다. 범위가 1일경우는 해당 타일에 계산하면 된다.
        {
            //타겟의 경우 와이드 스킬의 범위기준을 보스가 아닌 플레이어의 위치좌표로 잡으면 된다. 규칙은 와이드 스킬가 동일.

            int saveRow = m_PlayerRow;
            int saveColumn = 0;

            int checkRow_P;   //보스 기준 아래쪽에 있는 Column값
            int checkRow_M;   //보스 기준 위쪽에 있는 Column값
            int checkColumn;  //현재 색을 바꿀 타일의 Column값

            for (float i = 0; i < m_CurrentBossSkill.SkillRange; i += 1.0f)
            {
                checkRow_P = m_PlayerRow + (int)i;
                checkRow_M = m_PlayerRow - (int)i;

                if (checkRow_P % 2 == 1) //024(135라인)
                    saveColumn++;

                for (float j = 0; j < xRange; j += 1.0f)
                {

                    if (i != 0) //보스가 있는 라인의 +-1라인씩 그림.
                    {

                        checkColumn = saveColumn + (int)j;

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;



                        if (checkRow_P < 5)
                        {
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                            m_StageMgr.m_MapInfo[checkRow_P, checkColumn].BossEffect = true;
                            //map.mapIndicatorArray[checkRow_P, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                        if (checkRow_M >= 0)
                        {
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                            m_StageMgr.m_MapInfo[checkRow_M, checkColumn].BossEffect = true;
                            //map.mapIndicatorArray[checkRow_M, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        }

                    }
                    else  //플레이어가 있는 라인을 쭉그림.
                    {

                        checkColumn = m_PlayerColumn - (int)range + (int)j;

                        if (j == 0)
                            saveColumn = checkColumn;   //보스라인에서 첫번째 타일 색변환위치 저장.

                        if (checkColumn < 0 || checkColumn > 5)
                            continue;

                        //map.mapIndicatorArray[m_BossRow, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[m_PlayerRow, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[m_PlayerRow, checkColumn].BossEffect = true;
                    }


                }

                //Debug.Log($"{i+1}번째 SaveColumn={saveColumn}");

                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[m_PlayerRow, m_PlayerColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
            m_StageMgr.m_MapInfo[m_PlayerRow, m_PlayerColumn].BossEffect = true;
            //map.mapIndicatorArray[m_BossRow, m_BossColumn].GetComponent<MeshRenderer>().material.color = Color.red;
        }

        //경고시간(모든스킬 2f초로 고정)이  경과하면 빨간색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.
        Debug.Log("보스 스킬 범위 ");
        yield return new WaitForSeconds(2f);


        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {
                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(SkillPrefab); //이펙트 표시.
                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    effect.GetComponent<DamageCheck>().who = 2;
                    effect.GetComponent<DamageCheck>().Dot = m_CurrentBossSkill.DoT;
                    m_StageMgr.m_MapInfo[i, j].BossEffectObject = effect;
                }
            }
        }

        Debug.Log("이펙트 소환");


        yield return new WaitForSeconds(m_CurrentBossSkill.LifeTime);  //생존시간이 지나면 이펙트 지우기

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].BossEffect = false;
                    Object.Destroy(m_StageMgr.m_MapInfo[i, j].BossEffectObject);
                }
            }
        }

        this.gameObject.GetComponent<BossFSM>().behavior = false;
        Debug.Log("타겟 스킬 종료");
        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");


        //버프가 있다면 버프를 발동.
        if (m_CurrentBossSkill.BuffAdded != 0)
        {
            MainManager.Instance.GetBuffManager().Buff(m_CurrentBossSkill.BuffAdded);
        }

        //연계스킬 처리
        if (m_CurrentBossSkill.SkillAdded != 0)
        {
            BossSkillAction(m_CurrentBossSkill.SkillAdded);
            TargetLockOn = true;
        }
        else
        {
            this.gameObject.GetComponent<BossFSM>().behavior = false;

            if (TargetLockOn)
                TargetLockOn = false;
        }


        yield break;

    }


    //각도얻기 함수
    public float CalculateAngle(Vector3 from, Vector3 to)
    {
        return Quaternion.FromToRotation(Vector3.forward, to - from).eulerAngles.y;
    }



    IEnumerator SkillLineAction()
    {


        LineSkillTarget = GameObject.Find("Player").transform.GetChild(0).gameObject;

        int Row = 0;
        int Column = 0;

        if (TargetLockOn)
        {
            Row = m_PlayerRow;
            Column = m_PlayerColumn;
        }
        else
        {
            Row = m_BossRow;
            Column = m_BossColumn;
        }

        Debug.Log("확산 스킬 시작");


        float range = m_CurrentBossSkill.SkillRange - 1.0f;
        //float xRange = m_CurrentBossSkill.SkillRange + range;

        //플레이어와의 각도를 구함. 
        float tempAngle = CalculateAngle(this.gameObject.transform.position, LineSkillTarget.transform.position);
        int checkIndex = 0;


        //각도의 범위를 고정으로
        if (tempAngle > 0 && tempAngle <= 45)
        {
            tempAngle = 45;
            checkIndex = 0;
        }
        else if (tempAngle > 45 && tempAngle <= 90)
        {
            tempAngle = 90;
            checkIndex = 1;
        }
        else if (tempAngle > 90 && tempAngle <= 180)
        {
            tempAngle = 135;
            checkIndex = 2;
        }
        else if (tempAngle > 180 && tempAngle <= 225)
        {
            tempAngle = 225;
            checkIndex = 3;
        }
        else if (tempAngle > 225 && tempAngle <= 270)
        {
            tempAngle = 270;
            checkIndex = 4;
        }
        else if (tempAngle > 270 && tempAngle <= 360)
        {
            tempAngle = 315;
            checkIndex = 5;
        }

       

        // 보스가 스킬을 사용할때 해당로 돌려준다음
        this.gameObject.transform.rotation = Quaternion.Euler(0, tempAngle, 0);


        


        // 해당 각도를 시작으로 시계방향으로 타일 방향체크후 증감값 셋팅. 
        for (int i = 0; i < OriginArr.Length; i++)
        {

           
            //Row
            if (i == 0 || i == 5)
            {
                OriginArr[i].row = Row - 1;
            }
            else if (i == 2 || i == 3)
            {
                OriginArr[i].row = Row + 1;
            }

            if (i == 1 || i == 4)
                OriginArr[i].row = Row;


            //Column
            if (Row % 2 == 0)
            {
                if (i == 0 || i == 2)
                {
                    OriginArr[i].column = Column +1;
                }
                else if (i == 3 || i == 5)
                {
                    OriginArr[i].column = Column;
                }

                if (i == 1)
                {
                    OriginArr[i].column = Column + 1;
                }
                else if (i == 4)
                {
                    OriginArr[i].column = Column - 1;
                }

            }
            else
            {

                if (i == 0 || i == 2)
                {
                    OriginArr[i].column = Column;
                }
                else if (i == 3 || i == 5)
                {
                    OriginArr[i].column = Column -1;
                }

                if (i == 1)
                {
                    OriginArr[i].column = Column + 1;
                }
                else if (i == 4)
                {
                    OriginArr[i].column = Column - 1;
                }
            }

        }


        List<ClockDirectionList> tempList = new List<ClockDirectionList>();
        tempList = OriginArr.ToList<ClockDirectionList>();

        //리스트에서 정렬을 끝내준뒤.
        ClockDirectionList tempCDL = new ClockDirectionList();
        for (int i = 0; i < tempList.Count; i++)
        {
            if (checkIndex == tempList[i].check)
                break;
            else   //보고있는방향부터 시계방향으로 돌아가도록 정렬.
            {
                tempCDL = tempList[i];
                tempList.RemoveAt(i);
                tempList.Add(tempCDL);
                i--;

            }
        }

        ClockDirectionList[] tempCDLARR = tempList.ToArray();

        //이펙트를 내보낼 타일 설정.
        if (m_CurrentBossSkill.Direction1)
        {
            tempCDLARR[0].effect = true;
        }
        if (m_CurrentBossSkill.Direction2)
        {
            tempCDLARR[1].effect = true;
        }
        if (m_CurrentBossSkill.Direction3)
        {
            tempCDLARR[2].effect = true;
        }
        if (m_CurrentBossSkill.Direction4)
        {
            tempCDLARR[3].effect = true;
        }
        if (m_CurrentBossSkill.Direction5)
        {
            tempCDLARR[4].effect = true;
        }
        if (m_CurrentBossSkill.Direction6)
        {
            tempCDLARR[5].effect = true;
        }


        //확산스킬은 무조건 거리가 2부터 시작.

        int checkRow = 0;
        int checkColumn = 0;

        for (float i = 0; i < range; i += 1.0f) // 
        {
            if (i == 0)
            {

                m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                m_StageMgr.m_MapInfo[Row, Column].BossEffect = true;


                for (int j = 0; j < tempCDLARR.Length; j++)
                {
                    if (tempCDLARR[j].effect)
                    {
                        if ((tempCDLARR[j].column >= 0 && tempCDLARR[j].column < 6) && (tempCDLARR[j].row >= 0 && tempCDLARR[j].row < 5))
                        {
                            m_StageMgr.m_MapInfo[tempCDLARR[j].row, tempCDLARR[j].column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                            m_StageMgr.m_MapInfo[tempCDLARR[j].row, tempCDLARR[j].column].BossEffect = true;
                        }
                    }
                }

            }
            else
            {
                for (int j = 0; j < tempCDLARR.Length; j++)
                {
                    if (tempCDLARR[j].effect)
                    {
                        //Column
                        if (tempCDLARR[j].row % 2 == 0) //135라인.
                        {

                            if (tempCDLARR[j].check == 0 || tempCDLARR[j].check == 2)
                            {
                                checkColumn = tempCDLARR[j].column + 1;
                                tempCDLARR[j].column = checkColumn;
                            }

                            if (tempCDLARR[j].check == 1)
                            {
                                checkColumn = tempCDLARR[j].column + 1;
                                tempCDLARR[j].column = checkColumn;
                            }

                            if (tempCDLARR[j].check == 4)
                            {
                                checkColumn = tempCDLARR[j].column - 1;
                                tempCDLARR[j].column = checkColumn;
                            }

                        }
                        else
                        {
                            if (tempCDLARR[j].check == 3 || tempCDLARR[j].check == 5)
                            {
                                checkColumn = tempCDLARR[j].column -1;
                                tempCDLARR[j].column = checkColumn;
                            }

                            if (tempCDLARR[j].check == 1)
                            {
                                checkColumn = tempCDLARR[j].column + 1;
                                tempCDLARR[j].column = checkColumn;
                            }

                            if (tempCDLARR[j].check == 4)
                            {
                                checkColumn = tempCDLARR[j].column - 1;
                                tempCDLARR[j].column = checkColumn;
                            }
                        }


                        //Row
                        checkRow = tempCDLARR[j].row - Row;
                        if (checkRow > 0)
                        {
                            tempCDLARR[j].row++;
                        }
                        else if (checkRow < 0)
                        {
                            tempCDLARR[j].row--;
                        }


                        if ((tempCDLARR[j].column >= 0 && tempCDLARR[j].column < 6) && (tempCDLARR[j].row >= 0 && tempCDLARR[j].row < 5))
                        {
                            m_StageMgr.m_MapInfo[tempCDLARR[j].row, tempCDLARR[j].column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                            m_StageMgr.m_MapInfo[tempCDLARR[j].row, tempCDLARR[j].column].BossEffect = true;
                        }

                    }
                }

            }

        }


        //경고시간(모든스킬 0.5f초로 고정)이  경과하면 빨간색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.
        Debug.Log("보스 스킬 범위 ");
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {
                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(SkillPrefab);
                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    effect.GetComponent<DamageCheck>().who = 2;
                    effect.GetComponent<DamageCheck>().Dot = m_CurrentBossSkill.DoT;
                    m_StageMgr.m_MapInfo[i, j].BossEffectObject = effect;
                }
            }
        }

        Debug.Log("이펙트 소환");
        //이쪽 에서 데미지 처리.
        yield return new WaitForSeconds(m_CurrentBossSkill.LifeTime);  //생존시간이 지나면 이펙트 지우기

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].BossEffect = false;
                    Object.Destroy(m_StageMgr.m_MapInfo[i, j].BossEffectObject);
                }
            }
        }

        Debug.Log("확산 스킬 종료");
        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");


        //버프가 있다면 버프를 발동.
        if (m_CurrentBossSkill.BuffAdded != 0)
        {
            MainManager.Instance.GetBuffManager().Buff(m_CurrentBossSkill.BuffAdded);
        }

        //연계스킬있는지 확인후 다시 스킬실행.
        if (m_CurrentBossSkill.SkillAdded != 0)
            BossSkillAction(m_CurrentBossSkill.SkillAdded);
        else
        {
            if (TargetLockOn)
                TargetLockOn = false;

            this.gameObject.GetComponent<BossFSM>().behavior = false;
        }




        yield break;
    }




    IEnumerator SkillDiffusionAction()
    {

        int Row = 0;
        int Column = 0;

        if (TargetLockOn)
        {
            Row = m_PlayerRow;
            Column = m_PlayerColumn;
        }
        else
        {
            Row = m_BossRow;
            Column = m_BossColumn;
        }


        Debug.Log("방출(도넛) 스킬 시작");
        float range = m_CurrentBossSkill.SkillRange - 1.0f;
        float xRange = m_CurrentBossSkill.SkillRange + range;

        //보스 스킬범위를 표시해 주는 부분.
        if (range > 0)  //range는 -1을한값 범위가 2부터 여기 들어온다. 범위가 1일경우는 해당 타일에 계산하면 된다.
        {
            //방출(도넛)의 경우 거리가 1일때는 자기자신은 동일하지만 거리가 2부터 자기자신을  제외한영역 
            //거리가 3일때는 거리가 2인 영역을 제외한 영역만 포함해야한다.

            //스킬범위의 시작 기준점을 중심으로 제일처음과 끝에만 색칠해주기 그리고 스킬범위의 마지막 부분은 전부 다 칠해주기.

            int saveRow = Row;
            int saveColumn_P = 0;
            int saveColumn_M = 0;

            int checkRow_P;   //보스 기준 아래쪽에 있는 Column값
            int checkRow_M;   //보스 기준 위쪽에 있는 Column값
            int checkColumn;  //현재 색을 바꿀 타일의 Column값

            for (float i = 0; i < m_CurrentBossSkill.SkillRange; i += 1.0f)
            {
                checkRow_P = Row + (int)i;
                checkRow_M = Row - (int)i;

                if (checkRow_P % 2 == 1) //024(135라인)
                    saveColumn_M++;
                else
                    saveColumn_P--;


                if (i != 0)
                {

                    if (i != range)
                    {
                        if (saveColumn_M >= 0)
                        {
                   ;

                            if (checkRow_M >= 0)
                            {
                                //map.mapIndicatorArray[m_BossRow, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_M, saveColumn_M].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_M, saveColumn_M].BossEffect = true;
                            }

                            if(checkRow_P < 6)
                            {
                                m_StageMgr.m_MapInfo[checkRow_P, saveColumn_M].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_P, saveColumn_M].BossEffect = true;
                            }


                        }

                        if (saveColumn_P < 6)
                        {
                       

                            if(checkRow_M >= 0)
                            {
                                //map.mapIndicatorArray[m_BossRow, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_M, saveColumn_P].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_M, saveColumn_P].BossEffect = true;
                            }


                            if(checkRow_P < 6)
                            {
                                m_StageMgr.m_MapInfo[checkRow_P, saveColumn_P].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_P, saveColumn_P].BossEffect = true;
                            }


                         
                        }

                    }
                    else
                    {
                        for (float j = 0; j < xRange; j += 1.0f)
                        {

                            checkColumn = saveColumn_M + (int)j;

                            if (checkColumn < 0 || checkColumn > 5)
                                continue;

                            if (checkRow_P < 5)
                            {
                                m_StageMgr.m_MapInfo[checkRow_P, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_P, checkColumn].BossEffect = true;
                                //map.mapIndicatorArray[checkRow_P, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                            }

                            if (checkRow_M >= 0)
                            {
                                m_StageMgr.m_MapInfo[checkRow_M, checkColumn].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                                m_StageMgr.m_MapInfo[checkRow_M, checkColumn].BossEffect = true;
                                //map.mapIndicatorArray[checkRow_M, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                            }

                        }

                    }

                }
                else
                {
                    int Column_M = Column - (int)range;
                    int Column_P = Column + (int)range;
                    saveColumn_M = Column_M;
                    saveColumn_P = Column_P;

                    if (Column_M >= 0)
                    {
                        //map.mapIndicatorArray[m_BossRow, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[Row, Column_M].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[Row, Column_M].BossEffect = true;
                    }

                    if (Column_P < 6)
                    {
                        //map.mapIndicatorArray[m_BossRow, checkColumn].GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[Row, Column_P].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
                        m_StageMgr.m_MapInfo[Row, Column_P].BossEffect = true;
                    }
                }
                xRange -= 1.0f;
            }
        }
        else  //range가 0이하면 사거리가 1 자기자신의 타일만 해당
        {
            m_StageMgr.m_MapInfo[Row, Column].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.red;
            m_StageMgr.m_MapInfo[Row, Column].BossEffect = true;
            //map.mapIndicatorArray[m_BossRow, m_BossColumn].GetComponent<MeshRenderer>().material.color = Color.red;
        }

        //경고시간(모든스킬 0.5f초로 고정)이  경과하면 빨간색을 다시 원래색깔로 돌린후 그 범위에 이펙트 출현하고 데미지 로직 처리.
        Debug.Log("보스 스킬 범위 ");
        yield return new WaitForSeconds(2f);


        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {
                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].MapObject.transform.Find("indicator hexa").GetComponent<MeshRenderer>().material.color = Color.white;
                    GameObject effect = Instantiate(SkillPrefab);
                    effect.transform.position = m_StageMgr.m_MapInfo[i, j].MapPos + new Vector3(0, 5f, 0);
                    effect.GetComponent<DamageCheck>().who = 2;
                    effect.GetComponent<DamageCheck>().Dot = m_CurrentBossSkill.DoT;
                    m_StageMgr.m_MapInfo[i, j].BossEffectObject = effect;
                }
            }
        }

        Debug.Log("이펙트 소환");
        //이쪽 에서 데미지 처리.
        yield return new WaitForSeconds(m_CurrentBossSkill.LifeTime);  //생존시간이 지나면 이펙트 지우기

        for (int i = 0; i < m_StageMgr.mapZ; i++)
        {
            for (int j = 0; j < m_StageMgr.mapX; j++)
            {

                if (m_StageMgr.m_MapInfo[i, j].BossEffect)
                {
                    m_StageMgr.m_MapInfo[i, j].BossEffect = false;
                    Object.Destroy(m_StageMgr.m_MapInfo[i, j].BossEffectObject);
                }
            }
        }


        Debug.Log("방출(도넛) 스킬 종료");
        AnimationManager.GetInstance().PlayAnimation(anim, "Idle01");

        //버프가 있다면 버프를 발동.
        if (m_CurrentBossSkill.BuffAdded != 0)
        {
            MainManager.Instance.GetBuffManager().Buff(m_CurrentBossSkill.BuffAdded);
        }


        //연계스킬있는지 확인후 다시 스킬실행.
        if (m_CurrentBossSkill.SkillAdded != 0)
            BossSkillAction(m_CurrentBossSkill.SkillAdded);
        else
        {
            this.gameObject.GetComponent<BossFSM>().behavior = false;

            if (TargetLockOn)
                TargetLockOn = false;
        }


        yield break;
    }






    // Start is called before the first frame update
    void Start()
    {
        
        m_StageMgr = MainManager.Instance.GetStageManager();

        OriginArr = new ClockDirectionList[6];

        for (int i = 0; i < 6; i++)
        {
            OriginArr[i].check = i;
            OriginArr[i].effect = false;
        }

        anim = this.transform.GetChild(0).GetComponent<Animator>();

        if (anim == null)
        {
            anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }


        //names[0].Name = "이세영"; names[0].Age = 102;
        //names[1].Name = "권경민"; names[1].Age = 31;

    }


    // Update is called once per frame
    void Update()
    {

        if (checkSkill)
        {
            checkTime += Time.deltaTime;

            if (checkTime > anim.GetCurrentAnimatorStateInfo(0).length)
            {
                checkSkill = false;
                checkTime = 0f;

                MainManager.Instance.GetAnimanager().ChangeAnimationState(anim, BOSS_IDLE, currentState);
            }
        }

    }

#if UNITY_EDITOR
    //씬뷰에서 확인용
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = isCollision ? _red : _blue;
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, distance);
        UnityEditor.Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, distance);
    }
#endif

}
