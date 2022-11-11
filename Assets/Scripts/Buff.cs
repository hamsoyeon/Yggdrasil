using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{

    // 배열이나 리스트로 버프/디버프를 관리

    List<BuffTableExcel> BuffList;  //버프의 인덱스를 들고있다가 ( BuffIndex ) -> 그 버프의 고유값 버프 중첩이 가능하기 때문에. 같을경우 버프 시간 갱신.

    BuffTableExcel m_CurrentBuff;
    bool SearchBuff(int index)
    {
        foreach(var element in DataTableManager.Instance.GetDataTable<BuffTableExcelLoader>().DataList)
        {
            if(element.BuffsTableIndex == index)
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
        else if(isWho == 1) //보스면
        {
            return target;  //처음에 보스로 할당했으니까 그대로 반환. 사실 이 구문 없어도 되는데 보기 좋으라고 넣음
        }
        return target;
    }


    //target 0이면 플레이어 1이면 보스
    public void Stun(int target)
    {
        Animator anim;
        anim = isPlayerOrMonster(target).GetComponentInChildren<Animator>();//플레이어나 보스 오브젝트의 애니메이터 받아옴
        anim.SetBool("isStunned", true);//스턴 애니 재생
        Debug.Log("애니 컨트롤러 : ", anim);
        
        //이동 속도 0으로 만들기



        //아직
        IEnumerator Duration(float duration)
        {
            float time = 0.0f;
            while(time<1.0f)
            {
                time += Time.deltaTime / duration;
                yield return null;
            }
        }

        anim.SetBool("isStunned", false);
    }



    // 버프와 디버프를 하나의 함수에 모두 구혀할 것인가?
    public void AddBuff(int index) 
    {
        
        if(SearchBuff(index))
        {
            //현재 버프 리스트에 있는 버프인지 확인하고 없는 버프이면 추가 있는 버프면 업데이트.
            BuffList.Add(m_CurrentBuff);
            
        }


    }

    void AddDeBuff()
    {

    }

    void UpdateBuff()
    {

    }

    void UpdateDeBuff()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        BuffList = new List<BuffTableExcel>();


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("스페이스바 눌림");
            Stun(1);
        }
    }
}
