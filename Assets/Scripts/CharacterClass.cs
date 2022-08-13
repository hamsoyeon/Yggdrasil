using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CharacterClass : MonoBehaviour
{

	public enum Character {  SPIRIT=1, BOSS, MOB }
	//���߻���� �ޱ� ���絥 Monobehavior�� �ް��ִ� ������Ʈ�� ��� �ش� Ŭ������ ����ϴ°�?
	//�ش� cs�� ���̰� getComponent�� �����´�.
	public BossSkill_TableExcel m_BossSkillData;             //���� ��ų�� ���� ������
	public BossStat_TableExcel m_BossStatData;           //���� ���ݿ� ���� ������ 
	public CharStat_TableExcel m_CharacterStat;          //�÷��̾� ���ݿ� ���� ������ 
	public Spirit_TableExcel m_SpiritData;               //�÷��̾ ��ȯ�ϴ� ���ɿ� ���� ������
	public SpiritSkill_TableExcel m_SpiritSkillData;         //��ȯ�� ������ ����ϴ� ��ų�� ���� ������	
	public SkillManager m_SkillMgr;

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



	//�������� ���� �Լ��� ������ ����(�̵�,��ų)
	//�̵�
	public void move()
	{

	}


 
}
