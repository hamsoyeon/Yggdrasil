#include "pch.h"
#include "CStageMgr.h"
#include "CRoomMgr.h"
#include "CSession.h"
#include "CLock.h"
#include "CProtocolMgr.h"

CStageMgr* CStageMgr::m_instance = nullptr;

CStageMgr* CStageMgr::GetInst()
{
	return m_instance;
}

void CStageMgr::Create()
{
	if (m_instance == nullptr)
		m_instance = new CStageMgr();
}

void CStageMgr::Destroy()
{
	delete m_instance;
}

CStageMgr::CStageMgr()
	: m_lock(new CLock)
{
}

CStageMgr::~CStageMgr()
{
	delete m_lock;
}

void CStageMgr::UpdateStage(CSession* _ptr)
{
	for (CSession* item : _ptr->m_room->playerList)
	{
		t_UserInfo* userinfo = item->GetUserInfo();
		
		// stage에 있는 플레이어들을 보낸다.
		unsigned long protocol = 0;
		CProtocolMgr::GetInst()->AddSubProtocol(&protocol,(unsigned long)CStageMgr::SUB_PROTOCOL::STAGE_RESULTL);
		CProtocolMgr::GetInst()->AddDetailProtocol(&protocol,(unsigned long)CStageMgr::SERVER_DETAIL_PROTOCOL::STAGE_UPDATE_RESULT);
		Packing(protocol, userinfo->nickname, _ptr);
	}
}

void CStageMgr::Packing(unsigned long _protocol, const TCHAR* _str, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;
	int msg_size = wcslen(_str) * 2;

	memcpy(ptr, &msg_size, sizeof(int));
	size = size + sizeof(int);
	ptr = ptr + sizeof(int);

	memcpy(ptr, _str, msg_size);
	size = size + msg_size;

	_ptr->Packing(_protocol, buf, size);
}
