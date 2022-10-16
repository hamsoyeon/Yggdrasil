using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState { None = -1, Pursuit = 0, Attack};

public class Enemy : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    GameObject target;
    Status status;

    [Header("Attack")]
    [SerializeField]
    private float attackRange = 6; //공격 범위 (범위 안에 들어오면 Attack상태로 변경)
    [SerializeField]
    private float attackRate = 1; //공격 속도

    [SerializeField]
    private float pursiotLimitRange = 200; //추적 범위(이 범위 바깥으로 나가면 wander 상태로 변경)

    private EnemyState enemyState = EnemyState.None;
    private float lastAttackTime = 0; //공격 주기 계산용 변수

    private Animator anim;

    void Start()
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = status.RunSpeed;
        target = GameObject.Find("Player").transform.GetChild(0).gameObject;
        Debug.Log($"현재 타겟 : {target.transform.position}");

        anim = this.transform.GetChild(0).GetComponent<Animator>();

        if (anim == null)
        {
            anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }
    }

    void Update()
    {
        CalculateDistacveToTargetAndSelectState();
    }

    IEnumerator Move()
    {
        if (navMeshAgent.destination != target.transform.position)
        {
            navMeshAgent.SetDestination(target.transform.position);
            AnimationManager.GetInstance().PlayAnimation(anim, "Run");
        }
        else
        {
            AnimationManager.GetInstance().PlayAnimation(anim, "Run");
            navMeshAgent.SetDestination(transform.position);
        }
        yield return null;
    }

    private IEnumerator Attack()
    {
        Debug.Log("쫄몹 ATTACK FSM 변경");
        //공격할때는 이동을 멈추도록 설정
        navMeshAgent.ResetPath();
        anim.SetBool("IsAttack", true);

        while (true)
        {
            //타겟 방향 주시
            LockRotationToTarget();

            if (Time.time - lastAttackTime > attackRate)
            {
                //공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                lastAttackTime = Time.time;

               
            }

            yield return null;
        }
    }

    private void LockRotationToTarget()
    {
        //목표 위치
        Vector3 to = new Vector3(target.transform.position.x, 0, target.transform.position.z);
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
        float distance = Vector3.Distance(target.transform.position, transform.transform.position);

        if (distance <= attackRange)
        {
            StartCoroutine(Attack());
        }
        else
        {
            StartCoroutine(Move());
        }
    }

    private void OnDrawGizmos()
    {
        //공격 범위
        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
