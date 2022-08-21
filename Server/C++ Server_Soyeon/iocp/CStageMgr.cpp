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
	unsigned long protocol = 0;
	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)CStageMgr::SUB_PROTOCOL::CHARFIELD_RESULTL);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)CStageMgr::SERVER_DETAIL_PROTOCOL::CHARFIELD_UPDATE_RESULT);

	// ���� �� �ִ� �ִ� �ο��� 3��
	// stage�� �ִ� �÷��̾���� ������.

	t_UserInfo users[3]; int i = 0;
	int playerNum = _ptr->m_room->playerList.size();

	for (CSession* item : _ptr->m_room->playerList)
	{
		t_UserInfo userinfo = *(item->GetUserInfo());
		wcscpy(users[i].nickname, userinfo.nickname);
		users[i++].charType = userinfo.charType;
	}

	Packing(protocol, users, playerNum, _ptr);
}

void CStageMgr::ReadyProc(CSession* _ptr)
{
	// �̹� ���� ���� charType�� ������ ���ؾ���.
	// charType�� ��Ȱ��ȭ �Ǿ��ٰ� Ŭ���̾�Ʈ�鿡�� �˸���.
	int type = SetCharType(_ptr);
	_ptr->m_userinfo->charType = type;

	// �غ� ���� �÷��̾ push.
	_ptr->m_room->readyPlayerList.push_back(_ptr);

	// ������������ ������ ���� �����Ѵ�.
	if (type == 0)
		return;

	for (CSession* session : _ptr->m_room->playerList)
	{
		// �����ο��Ը� ����, ��� �÷��̾ select�ϸ�
		// ready��ư�� ���۹�ư���� �ٲٶ�� �˸�.

		bool flag = false;
		unsigned long protocol = 0;
		CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)CStageMgr::SUB_PROTOCOL::CLICKBTN_RESULT);
		CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)CStageMgr::SERVER_DETAIL_PROTOCOL::SELECTED_CHARTYPE);

		if (_ptr->m_room->owner == session)
		{
			if (_ptr->m_room->readyPlayerList.size() >= 3)
				flag = true;
		}

		Packing(protocol, type, flag, session);
	}
}

void CStageMgr::StartProc(CSession* _ptr)
{
	// state�� inGame���� �ٲپ��ش�.
	// �� Ŭ���̾�Ʈ�� state�� ingame���� �ٲپ��ش�.
	// ��� Ŭ���̾�Ʈ���� ������ �����Ѵ�.

	unsigned long protocol = 0;
	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)CStageMgr::SUB_PROTOCOL::STAGE_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)CStageMgr::SERVER_DETAIL_PROTOCOL::ENTER_INGAME);

	for (CSession* session : _ptr->m_room->playerList)
	{
		session->SetState(session->GetInGameState());
		session->Packing(protocol, nullptr, 0);
	}
}

void CStageMgr::ChatSendProc(CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	TCHAR str[BUFSIZE]; ZeroMemory(str, BUFSIZE * 2);
	_ptr->UnPacking(buf);
	UnPacking(buf, str);

	// msg�� Ŭ���̾�Ʈ�� �г����� ���δ�.
	TCHAR msg[BUFSIZE]; ZeroMemory(msg, BUFSIZE * 2);
	wsprintf(msg, L"[%s��] : %s", _ptr->GetUserInfo()->nickname, str);

	for (CSession* session : _ptr->m_room->playerList) // ���� �κ� �ִ� Ŭ���̾�Ʈ�鿡�� �޽����� ������.
	{
		unsigned long protocol = 0;
		CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::CHAT_RESULT);
		CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::ALL_MSG_SUCCESS);
		Packing(protocol, msg, session);
	}
}

void CStageMgr::ExitStage(CSession* _ptr)
{

	unsigned long protocol = 0;
	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::STAGE_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::STAGE_EXIT_SUCCESS);

	t_UserInfo users[3]; int i = 0;
	int playerNum = _ptr->m_room->playerList.size();

	for (CSession* item : _ptr->m_room->playerList)
	{
		if (item != _ptr)
		{
			t_UserInfo userinfo = *(item->GetUserInfo());
			wcscpy(users[i].nickname, userinfo.nickname);
			users[i++].charType = userinfo.charType;
			Packing(protocol, users, playerNum, item);
		}
	}

	_ptr->m_room->playerList.remove(_ptr);
}

int CStageMgr::SetCharType(CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	int charType = -1;

	_ptr->UnPacking(buf);
	UnPacking(buf, charType);

	unsigned long protocol = 0;
	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)CStageMgr::SUB_PROTOCOL::CLICKBTN_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)CStageMgr::SERVER_DETAIL_PROTOCOL::SELECTED_CHARTYPE);

	for (CSession* session : _ptr->m_room->playerList)
	{
		// �濡 �ִ� ��� Ŭ���̾�Ʈ���� �ش� charType�� ���õǾ��ٰ� �˸���.
		Packing(protocol, charType, session);
	}

	return charType;
}

void CStageMgr::Packing(unsigned long _protocol, const TCHAR* _str, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;
	int str_size = wcslen(_str) * 2;

	memcpy(ptr, &str_size, sizeof(int));
	size = size + sizeof(int);
	ptr = ptr + sizeof(int);

	memcpy(ptr, _str, str_size);
	size = size + str_size;

	_ptr->Packing(_protocol, buf, size);
}

void CStageMgr::Packing(unsigned long _protocol, t_UserInfo* _users, int _arrNum, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;

	memcpy(ptr, &_arrNum, sizeof(int));
	ptr = ptr + sizeof(int);
	size = size + sizeof(int);

	for (int i = 0; i < _arrNum; i++)
	{
		// �г��� ��ŷ.
		int nickSize = wcslen(_users->nickname) * 2;

		memcpy(ptr, &nickSize, sizeof(int));
		ptr = ptr + sizeof(int);
		size = size + sizeof(int);

		memcpy(ptr, &_users[i].nickname, nickSize);
		ptr = ptr + nickSize;
		size = size + nickSize;

		memcpy(ptr, &_users[i].charType, sizeof(int));
		ptr = ptr + sizeof(int);
		size = size + sizeof(int);
	}

	_ptr->Packing(_protocol, buf, size);
}

void CStageMgr::Packing(unsigned long _protocol, bool _flag, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;

	memcpy(ptr, &_flag, sizeof(bool));
	size = size + sizeof(bool);

	_ptr->Packing(_protocol, buf, size);
}

void CStageMgr::Packing(unsigned long _protocol, int _data, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;

	memcpy(ptr, &_data, sizeof(int));
	size = size + sizeof(int);

	_ptr->Packing(_protocol, buf, size);
}

void CStageMgr::Packing(unsigned long _protocol, int _data, bool _flag, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;

	memcpy(ptr, &_data, sizeof(int));
	size = size + sizeof(int);
	ptr = ptr + sizeof(int);

	memcpy(ptr, &_flag, sizeof(bool));
	size = size + sizeof(bool);

	_ptr->Packing(_protocol, buf, size);
}

void CStageMgr::UnPacking(const BYTE* _buf, int& _data)
{
	const BYTE* ptr = _buf;
	int msg_size = 0;

	memcpy(&_data, ptr, sizeof(int));
}

void CStageMgr::UnPacking(const BYTE* _buf, TCHAR* _str)
{
	const BYTE* ptr = _buf;
	int strSize = 0;

	memcpy(&strSize, ptr, sizeof(int));
	ptr += sizeof(int);

	memcpy(_str, ptr, strSize);
}
