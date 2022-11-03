using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClass : MonoBehaviour
{

	public enum Character {  SPIRIT=1, BOSS, MOB }
	//다중상속을 받기 힘든데 Monobehavior을 받고있는 오브젝트에 어떻게 해당 클래스를 상속하는가?
	//해당 cs를 붙이고 getComponent로 가져온다.
	public BossSkill_TableExcel m_BossSkillData;             //보스 스킬에 관한 데이터
	public Boss_TableExcel m_BossStatData;           //보스 스텟에 관한 데이터 
	public CharStat_TableExcel m_CharacterStat;          //플레이어 스텟에 관한 데이터 
	public Spirit_TableExcel m_SpiritData;               //플레이어가 소환하는 정령에 관한 데이터
	public SpiritSkill_TableExcel m_SpiritSkillData;         //소환된 정령이 사용하는 스킬에 관한 데이터	
    public SubMonster_TableExcel m_SubMonsterData;
	public SkillManager m_SkillMgr;

    public float Invincibility = 1.0f;          //캐릭터가 사용하는 무적값.

    public CharacterClass()
	{
		m_SkillMgr = new SkillManager();
	}


	public void Skill(Character who,int index)
	{
		switch (who)
		{
			case Character.SPIRIT:
				m_SkillMgr.
				SpiritUseSkill(index);
				break;
			case Character.BOSS:
				m_SkillMgr.BossUseSkill(index);
				break;
			case Character.MOB:
				break;
		}
	}



	//공통으로 사용될 함수를 나열해 보자(이동,스킬)
	//이동
	public void move()
	{

	}


 
}
