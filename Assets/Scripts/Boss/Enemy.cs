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
    bool isDie;

    Coroutine coroutine;
    void Start()
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = status.RunSpeed;
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
        //공격할때는 이동을 멈추도록 설정
        navMeshAgent.ResetPath();
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
        if (target == null)
            return;

        //플레이어(target)와 적의 거리 계산 후 거리에 따라 행동 선택
        float distance = Vector3.Distance(target.transform.position, transform.transform.position);

        if (distance <= attackRange)
        {
            if (coroutine != null)
            {
                //StopCoroutine(coroutine);
                //coroutine = null;
            }
            else
            {
                coroutine = StartCoroutine(Attack());

                if (Time.time - lastAttackTime > attackRate)
                {
                    //공격 주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장
                    target.GetComponent<PlayerManager>().TakeDamage(status.ATTACK);
                    lastAttackTime = Time.time;
                }

            }

        }
        else
        {
            if (coroutine != null)
            {
                //StopCoroutine(coroutine);
                //coroutine = null;
            }
            else
            {
                if(isDie)
                {
                    coroutine = StartCoroutine(IsDie());
                }
                else
                    coroutine = StartCoroutine(Move());
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
        anim.SetBool("IsDie", isDie);
        TakeDamagePrint(damage);
    }

    IEnumerator IsDie()
    {
        //죽었을때의 행동 개설
        Debug.Log("죽었다.");
        AnimationManager.GetInstance().PlayAnimation(anim, "Die");
        //AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            this.gameObject.SetActive(false);

            //삭제
            Destroy(this.gameObject);
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
