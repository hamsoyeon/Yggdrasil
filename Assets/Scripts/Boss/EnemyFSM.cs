using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack, }

public class EnemyFSM : MonoBehaviour
{
    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionRange = 8; //인식 범위 (이범위안에 들어오면pursuit상태로 변경)
    [SerializeField]
    private float pursiotLimitRange = 10; //추적 범위(이 범위 바깥으로 나가면 wander 상태로 변경)

    [Header("Attack")]
    [SerializeField]
    private float attackRange = 6; //공격 범위 (범위 안에 들어오면 Attack상태로 변경)
    [SerializeField]
    private float attackRate = 1; //공격 속도

    private EnemyState enemyState = EnemyState.None; //현재 적의 행동
    private float lastAttackTime = 0; //공격 주기 계산용 변수

    private Status status; //이동속도 등의 정보
    private NavMeshAgent navMeshAgent; //이동제어를 위한 navigation
    private Transform target; //적의 공격 대상(플레이어)

    public void SetUp(Transform target)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target; // 시작하자마자 플레이어 집어넣고

        Debug.Log($"현재 타겟 : {target.name}");

        //NavMeshAgent 컴포넌트에서 회전을 업데이트하지 않도록 설정
        navMeshAgent.updateRotation = false;
    }

    private void OnEnable()
    {
        //적이 활성화 될 때 적의 상태를 "대기"로 설정
        ChangeState(EnemyState.Idle);
    }

    private void OnDisable()
    {
        //적이 비활성화 될때 현재 재생중인 상태를 종료하고, 상태를 "None"으로 설정
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }

    public void ChangeState(EnemyState newState)
    {
        //현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 바꿀 필요가 없다.
        if (newState == enemyState)
            return;

        //이전에 재생중이던 상태 종료
        StopCoroutine(enemyState.ToString());
        //현재 적의 상태를 newState로 설정
        enemyState = newState;
        //새로운 상태 재생
        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        //n초 후에 "배회" 상태로 변경하는 코루틴 실행
        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            //"대기" 상태 일때 하는 행동
            //타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 공격)
            CalculateDistacveToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        //1~4초 시간 대기
        int changeTime = Random.Range(1, 5);

        yield return new WaitForSeconds(changeTime);

        //상태를 "배회"로 변경
        ChangeState(EnemyState.Wander);
    }

    private IEnumerator Wander()
    {
        float currentTime = 0;
        float maxTime = 10;

        //이동속도 설정
        navMeshAgent.speed = status.RunSpeed;

        //목표 위치 설정
        navMeshAgent.SetDestination(CalculateWanderPosition());

        //목표위치로 회전
        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to = from);

        while (true)
        {
            currentTime += Time.deltaTime;

            //목표위치에 근접하게 도달하거나 너무 오랜시간동안 배회하기 상태에 머물러 있으면
            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
            if ((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                //상태를 "대기"로 변경
                ChangeState(EnemyState.Idle);
            }

            CalculateDistacveToTargetAndSelectState();

            yield return null;
        }
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        while(true)
        {
            //이동 속도 설정(배회할 때는 걷는 속도로 이동, 추척할때는 뛰는 속도로 이동)
            //navMeshAgent.speed = status.RunSpeed;

            //목표위치를 현재 플레이어의 위치로 설정
            navMeshAgent.SetDestination(target.position);

            //타겟 방향을 계속 주시하도록 함
            LockRotationToTarget();

            //타겟과의 거리에 따라 행동 선택
            CalculateDistacveToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        //공격할때는 이동을 멈추도록 설정
        navMeshAgent.ResetPath();

        while(true)
        {
            //타겟 방향 주시
            LockRotationToTarget();

            //타겟과의 거리에 따라 행동 선택
            CalculateDistacveToTargetAndSelectState();

            if(Time.time - lastAttackTime > attackRate)
            {
                //공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;

                //clone.GetComponent<EnemyProjectTile>().SetUp(target.position);
            }

            yield return null;
        }
    }

    private void LockRotationToTarget()
    {
        //목표 위치
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        //내 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        //바로 돌기
        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void CalculateDistacveToTargetAndSelectState()
    {
        if (target == null)
            return;

        //플레이어(target)와 적의 거리 계산 후 거리에 따라 행동 선택
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distance <= targetRecognitionRange)
        {
            ChangeState(EnemyState.Pursuit);
        }
        else if (distance >= targetRecognitionRange)
        {
            ChangeState(EnemyState.Wander);
        }
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10; //현재 위치를 원점으로 하는 원의 반지름
        int wanderJitter = 0; //선택된 각도(wanderJitterMin ~ wanderJitterMax)
        int wanderJitterMin = 0; //최소 각도
        int wanderJitterMax = 360; //최대 각도

        //현재 적 캐릭터가 있는 월드의 중심 위치와 크기(구역을 벗어난 행동을 하지 않도록)
        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        //자신의 위치를 중심으로 반지름 거리, 선택된 각도에 위치한 좌표를 목표지점으로 설정
        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        //생성된 목표위치가 자신의 이동구역을 벗어나지 않게 조절
        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private void OnDrawGizmos()
    {
        //목표 인식 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        //추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursiotLimitRange);

        //공격 범위
        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);

        //"배회" 상태일 때 이동할 경로 표시
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);
    }
}
