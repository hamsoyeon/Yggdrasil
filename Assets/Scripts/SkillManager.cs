using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//��ų�� ����ϸ鼭  ��Ÿ���̳� ������ ���� ������ó���� ��� ����.

public class SkillManager 
{

	public BossSkill m_BossSkill;
	public SpiritSkill m_SpiritSkill;
	public Spirit m_Spirit;


	//Skill Index�� ����(�ߺ��� ����� ���� � ��ų index���� ��)
	public bool UseSkill(int Index)
	{

		//index���� ��ų�� ����Ѵ�.(������ �÷��̾��� ��ų �ε��� ���� �ߺ��̶��?)
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
