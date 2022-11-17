using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public GameObject stunEffect;
    public GameObject stunEffectTemp;
    public Vector3 effectOffset = new Vector3(0, 10.0f, 0);


    // 배열이나 리스트로 버프/디버프를 관리

    List<BuffTableExcel> BuffList;  //버프의 인덱스를 들고있다가 ( BuffIndex ) -> 그 버프의 고유값 버프 중첩이 가능하기 때문에. 같을경우 버프 시간 갱신.

    BuffTableExcel m_CurrentBuff;
    bool SearchBuff(int index)
    {
        foreach (var element in DataTableManager.Instance.GetDataTable<BuffTableExcelLoader>().DataList)
        {
            if (element.BuffsTableIndex == index)
            {
                m_CurrentBuff = element;
                //break;
                return true;
            }
        }

        return false;

    }

    //몬스터인지 플레이어인지 식별. 0이면 플레이어, 1이면 보스
    public GameObject isPlayerOrMonster(int isWho)
    {
        GameObject target = GameObject.FindWithTag("Boss"); //할당 안 해주면 오류나서 기본적으로 보스로 설정

        if (isWho == 0)//플레이어면
        {
            GameObject player = GameObject.FindWithTag("Player");//player 불러오기
            target = player;
        }
        else if (isWho == 1) //보스면
        {
            return target;  //처음에 보스로 할당했으니까 그대로 반환.
        }
        return target;
    }

    public void StatBuff(int _index, int isBuff)//버프 인덱스 번호, 버프 디버프 여부 버프 일 경우 1, 디버프는 -1
    {
        switch(_index)
        {
            case 1://공격력

                break;
        }
    }

    //target 0이면 플레이어 1이면 보스
    public void Stun(int target)
    {
        Animator anim;
        Object targetObj = isPlayerOrMonster(target);
        float originSpeed = 1.0f;
        float durationTime = 2.0f; //지연 시간 2초.

        anim = isPlayerOrMonster(target).GetComponentInChildren<Animator>();//플레이어나 보스 오브젝트의 애니메이터 받아옴

        if (target == 1) //boss면
        {
            originSpeed = isPlayerOrMonster(target).GetComponent<BossFSM>().bossSpeed;  //기존 이속 저장
            isPlayerOrMonster(target).GetComponent<BossFSM>().bossSpeed = 0.0f; //boss 이속을 0으로 만듦
            //스턴 이펙트 생성
            stunEffectTemp = Instantiate(stunEffect);
            stunEffectTemp.transform.position = (isPlayerOrMonster(target).transform.position) + effectOffset;  //이펙트 위치 셋
            //공격력 0으로 만들기
        }

        //애니 도저히 안 되면 그냥 이펙트만 넣기
        anim.SetBool("isStunned", true);//스턴 애니 재생

        StartCoroutine(Normalization(durationTime, anim, 1, originSpeed));
    }

    IEnumerator Normalization(float duration, Animator _anim, int target, float originSpeed)   //지연시간, 애니메이터, 타겟(1이면보스)
    {
        yield return new WaitForSeconds(duration);

        //지연후 값 정상화
        _anim.SetBool("isStunned", false);
        isPlayerOrMonster(target).GetComponent<BossFSM>().bossSpeed = originSpeed;
        Destroy(stunEffectTemp);
    }

    // 버프와 디버프를 하나의 함수에 모두 구혀할 것인가?
    public void AddBuff(int index)
    {

        if (SearchBuff(index))
        {
            //현재 버프 리스트에 있는 버프인지 확인하고 없는 버프이면 추가 있는 버프면 업데이트.
            BuffList.Add(m_CurrentBuff);
        }
    }

    void Start()
    {
        BuffList = new List<BuffTableExcel>();


    }
}
