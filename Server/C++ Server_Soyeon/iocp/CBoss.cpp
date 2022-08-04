#include "pch.h"
#include "CBoss.h"

CBoss::CBoss(t_GameObj _obj)
{
	ZeroMemory(m_name, STR_SIZE * 2);
	wcscpy(m_name, _obj.m_charName);
	m_id = _obj.m_objID;
	m_pos = _obj.m_objPos;
}

CBoss::~CBoss()
{
}
