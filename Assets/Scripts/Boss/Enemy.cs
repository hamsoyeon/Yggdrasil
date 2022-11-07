using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private float lastAttackTime = 0; //공격 주기 계산용 변수

    public GameObject hudDamageText;
    public Transform hudPos;

    private Animator anim;

    //쫄몹이 죽었는지 확인할 변수
    bool isDie;

    //플레이어와 쫄몹간의 차이를 계산할 변수
    float distance;

    Coroutine coroutine;
    void Start()
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = status.SubMonsterClass.m_SubMonsterData.Speed;
        target = GameObject.Find("Player").transform.GetChild(0).gameObject;
        Debug.Log($"현재 타겟 : {target.transform.position}");
        hudDamageText = Resources.Load<GameObject>("DamageText");
        hudPos = this.gameObject.transform;

        anim = this.transform.GetChild(0).GetComponent<Animator>();

        if (anim == null)
        {
            anim = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        }
    }

    void Update()
    {
        //플레이어(target)와 적의 거리 계산 후 거리에 따라 행동 선택
        distance = Vector3.Distance(target.transform.position, transform.transform.position);
        CalculateDistacveToTargetAndSelectState();
    }

    IEnumerator Move()
    {
        AnimationManager.GetInstance().PlayAnimation(anim, "Run");
        if (navMeshAgent.destination != target.transform.position)
        {
            navMeshAgent.SetDestination(target.transform.position);
        }
        else
        {
            navMeshAgent.SetDestination(transform.position);
        }
        yield return null;

        coroutine = null;
    }

    IEnumerator Attack()
    {
        anim.SetBool("IsAttack", true);

        while (true)
        {
            //타겟 방향 주시
            LockRotationToTarget();

            yield return null;

            coroutine = null;
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
        //Debug.Log($"거리 차이 : {distance}");
        if (target == null)
            return;

        if (distance <= attackRange)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            else
            {
                navMeshAgent.isStopped = true;
                navMeshAgent.updatePosition = false;
                navMeshAgent.updateRotation = false;
                navMeshAgent.velocity = Vector3.zero;

                coroutine = StartCoroutine(Attack());

                if (Time.time - lastAttackTime > attackRate)
                {
                    //공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                    target.GetComponent<PlayerManager>().TakeDamage(status.SubMonsterClass.m_SubMonsterData.Demege);
                    lastAttackTime = Time.time;
                }

            }

        }
        else
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.updatePosition = true;
            navMeshAgent.updateRotation = true;

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            else
            {
                if (isDie)
                {
                    coroutine = StartCoroutine(IsDie());
                }
                else
                {
                    coroutine = StartCoroutine(Move());
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        //공격 범위
        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    //쫄몹 데미지 받는 함수
    public void TakeDamage(int damage)
    {
        Debug.Log("데미지 받는다.");
        isDie = status.DecreaseHP(damage);
        TakeDamagePrint(damage);
    }

    IEnumerator IsDie()
    {
        //죽었을때의 행동 개설
        Debug.Log("죽었다.");
        anim.SetBool("IsDie", isDie);
        AnimationManager.GetInstance().PlayAnimation(anim, "Die");
        //AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            this.gameObject.SetActive(false);

            //삭제
            Destroy(this.gameObject);

            coroutine = null;
            yield break;
        }
    }

    public void TakeDamagePrint(int damage)
    {

        GameObject hudText = Instantiate(hudDamageText);

        hudText.transform.position = hudPos.position + (Vector3.up * 20);
        hudText.GetComponent<DamageTxt>().damage = damage;
    }
}
