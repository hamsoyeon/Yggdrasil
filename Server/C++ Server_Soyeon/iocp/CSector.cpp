#include "pch.h"
#include "CSector.h"
#include "CBoss.h"
#include "CMonster.h"
#include "CObject.h"
#include "CPlayer.h"

//template<typename T>
//void CSector::AddObj(T* _obj)
//{
//	
//}

void CSector::AddGameObj(t_GameObj _obj)
{
	switch (_obj.m_kind)
	{
	case (UINT)GAMEOBJ_TYPE::BOSS:
		m_boss = new CBoss(_obj);
		break;
	case (UINT)GAMEOBJ_TYPE::MOB:
		m_monsterList.push_back(new CMonster(_obj));
		break;
	case (UINT)GAMEOBJ_TYPE::OBJ:
		m_objectList.push_back(new CObject(_obj));
		break;
	}
}

void CSector::AddPlayer(CSession* _ptr)
{
	m_playerList.push_back(new CPlayer(_ptr));
}
