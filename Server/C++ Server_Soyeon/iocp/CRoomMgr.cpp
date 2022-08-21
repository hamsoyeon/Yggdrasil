#include "pch.h"
#include "CRoomMgr.h"
#include "CSession.h"

CRoomMgr* CRoomMgr::m_instance = nullptr;

CRoomMgr* CRoomMgr::GetInst()
{
	return m_instance;
}

void CRoomMgr::Create()
{
	if (m_instance == nullptr)
		m_instance = new CRoomMgr();
}

void CRoomMgr::Destroy()
{
	delete m_instance;
}

CRoomMgr::CRoomMgr()
{
}

CRoomMgr::~CRoomMgr()
{
}

void CRoomMgr::Init()
{
	m_roomList.clear();
	m_roomNum = 0;

	//for (CRoom* room : m_roomList)
	//{
	//    room->GetRoomName();
	//}
}

void CRoomMgr::End()
{
}

tRoom* CRoomMgr::MakeRoom(TCHAR* _roomName, TCHAR* _passWord, CSession* _ptr)
{
	// ���� ����� �ش� �뿡 Ŭ���̾�Ʈ�� �־��ְ�
	// - ���� �̸�, 
	// - ���ȣ(������ Ŭ�󿡰� �ִ� ��),
	// - �濡 �� Ŭ���̾�Ʈ

	//tRoom* room = new tRoom(_roomName, m_roomNum++, _ptr);
	tRoom* room = new tRoom(_roomName, m_roomNum++, _ptr);
	if (_passWord != nullptr)
		wcscpy(room->passWord, _passWord); // ��й�ȣ
	room->owner = _ptr; // �� ����
	m_roomList.push_back(room);

	// ���� Ŭ���̾�Ʈ�� ���� ����.
	_ptr->m_room = room;


	return room;
}

void CRoomMgr::EnterRoom(CSession* _ptr, int _num)
{
	// roomList���� �÷��̾ ������ num�� �ش��ϴ� �濡 ����.
	for (tRoom* room : m_roomList)
	{
		if (room->roomNum == _num)
		{
			_ptr->m_room = room;
			room->playerList.push_back(_ptr);
		}
	}
}






