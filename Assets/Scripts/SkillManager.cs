using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//스킬을 사용하면서  쿨타임이나 데미지 관련 데이터처리를 어떻게 할지.

public class SkillManager 
{

	public BossSkill m_BossSkill;
	public SpiritSkill m_SpiritSkill;
	public Spirit m_Spirit;


	//Skill Index로 접근(중복의 우려가 있음 어떤 스킬 index인지 모름)
	public bool UseSkill(int Index)
	{

		//index에서 스킬을 사용한다.(보스와 플레이어의 스킬 인덱스 값이 중복이라면?)
		return false;
	}

	

	public void BossUseSkill(int index)
	{
		m_BossSkill.BossSkillAction(index);

		
	}

	public void SpiritUseSkill(int index)
	{
		m_Spirit.SpiritSummon(index);
	}




}
