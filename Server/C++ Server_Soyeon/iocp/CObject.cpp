#include "pch.h"
#include "CObject.h"

CObject::CObject(t_GameObj _obj)
{
	ZeroMemory(m_name, STR_SIZE * 2);
	wcscpy(m_name, _obj.m_charName);
	m_id = _obj.m_objID;
	m_pos = _obj.m_objPos;
}

CObject::~CObject()
{
}
