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
	// 룸을 만들면 해당 룸에 클라이언트를 넣어주고
	// - 방의 이름, 
	// - 방번호(서버가 클라에게 주는 것),
	// - 방에 들어갈 클라이언트

	//tRoom* room = new tRoom(_roomName, m_roomNum++, _ptr);
	tRoom* room = new tRoom(_roomName, m_roomNum++, _ptr);
	if (_passWord != nullptr)
		wcscpy(room->passWord, _passWord); // 비밀번호
	room->owner = _ptr; // 방 주인
	m_roomList.push_back(room);

	// 현재 클라이언트의 방을 설정.
	_ptr->m_room = room;


	return room;
}

void CRoomMgr::EnterRoom(CSession* _ptr, int _num)
{
	// roomList에서 플레이어가 선택한 num에 해당하는 방에 들어간다.
	for (tRoom* room : m_roomList)
	{
		if (room->roomNum == _num)
		{
			_ptr->m_room = room;
			room->playerList.push_back(_ptr);
		}
	}
}






