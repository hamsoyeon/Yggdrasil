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

    public void Stun()
    {
        Animator anim;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("isStunned", true);//스턴 애니 재생

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
        
    }
}
