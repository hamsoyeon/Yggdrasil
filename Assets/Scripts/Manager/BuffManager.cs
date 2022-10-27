using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuffManager : MonoBehaviour
{
    public CharacterClass PlayerClass;

    //임시(엑셀 데이터 안 받음)
    public void Stun(GameObject _target)
    {
        Animator ani;
        ani = _target.GetComponentInChildren<Animator>();
        ani.Play("Idle01"); //추후 디폴트 스테이트 애니 재생으로 변경

    }

    //임시 플레이어 힐
    public void Heal(GameObject _target, float _healAmount)
    {
        PlayerClass.m_CharacterStat.HP += _healAmount;
    }

    public void Buff(BuffTableExcel buff)
    {
        

        

        //if (buffIndex>=1  && buffIndex <=5)          //공격력,방어력,이동속도,치명타확률,치명타 배율
        //{
        //    //True일땐 버프로 False일땐 디버프로 적용

        //}
        //else if(buffIndex>=6 && buffIndex<=9)       //스턴,도트피해,넉백,밀어내기
        //{

        //    //디버프로만 적용(무조건false)

        //}
        //else if(buffIndex >=10 && buffIndex <=12)   //무적,즉시회복,도트회복
        //{

        //    //버프로만 적용(무조건 True)

        //}
        //else //소환(보스만)
        //{

        //}

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
