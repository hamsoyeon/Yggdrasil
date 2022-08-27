#include "pch.h"
#include "CLobbyMgr.h"
#include "CStageMgr.h"

#include "CSession.h"
#include "CProtocolMgr.h"
#include "CRoomMgr.h"

CLobbyMgr* CLobbyMgr::m_instance = nullptr;

CLobbyMgr* CLobbyMgr::GetInst()
{
	return m_instance;
}

void CLobbyMgr::Create()
{
	if (m_instance == nullptr)
		m_instance = new CLobbyMgr();
}

void CLobbyMgr::Destroy()
{
	delete m_instance;
}

CLobbyMgr::CLobbyMgr()
{
}

CLobbyMgr::~CLobbyMgr()
{
}

void CLobbyMgr::Init()
{
}

void CLobbyMgr::End()
{
}

void CLobbyMgr::RoomListUpdateProc(CSession* _ptr)
{
	// roomlist전체를 클라이언트에게 보내준다.
	list<tRoom*>* roomList = CRoomMgr::GetInst()->GetRoomList();

	// test용으로 방만들어보기
	//CRoomMgr::GetInst()->MakeRoom(_ptr);

	for (tRoom* room : *roomList) // 현재 존재하는 방들의 정보를 클라이언트에게 보낸다.
	{
		unsigned long protocol = 0;
		CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::ROOM_RESULT);
		CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::ROOMLIST_UPDATE_SUCCESS);
		Packing(protocol, room->roomName, room->passWord, room->roomNum, room->playerList.size(), _ptr);
	}
}

void CLobbyMgr::MakeRoomProc(CSession* _ptr)
{
	_ptr->SetState(_ptr->GetStageState());
	m_sessionList.remove(_ptr); // 로비에서 나감.

	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	TCHAR roomName[BUFSIZE]; ZeroMemory(roomName, BUFSIZE * 2);
	TCHAR passWord[BUFSIZE]; ZeroMemory(passWord, BUFSIZE * 2);

	_ptr->UnPacking(buf); // 데이터 버퍼만..
	UnPacking(buf, roomName, passWord); // 언팩한 이름의 방을 만듦.

	tRoom* room = CRoomMgr::GetInst()->MakeRoom(roomName, passWord, _ptr);

	unsigned long protocol = 0;
	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::ROOM_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::MAKE_ROOM_SUCCESS);

	// 로비에 있는 클라이언트들에게는 방이 업데이트가 되었다고 보냄. 
	for (CSession* session : m_sessionList)
	{
		list<tRoom*>* roomList = CRoomMgr::GetInst()->GetRoomList();

		for (tRoom* room : *roomList) // 현재 존재하는 방들의 정보를 클라이언트들에게 보낸다.
		{
			unsigned long protocol = 0;
			CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::ROOM_RESULT);
			CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::ROOMLIST_UPDATE_SUCCESS); 
			Packing(protocol, room->roomName, room->passWord, room->roomNum, room->playerList.size(), session);
		}
	}

	_ptr->Packing(protocol, nullptr);
}

void CLobbyMgr::EnterRoomProc(CSession* _ptr)
{
	// 방넘버를 통해서 session 집어넣기
	// 방넘버를 unpack

	// 방에 들어가면 로비에 있는 클라이언트들도 update

	_ptr->SetState(_ptr->GetStageState());
	m_sessionList.remove(_ptr); // 로비에서 나가고 stageState로..

	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	int room_number;

	_ptr->UnPacking(buf);
	UnPacking(buf, room_number);

	CRoomMgr::GetInst()->EnterRoom(_ptr, room_number); // 해당 방에 들어가고

	// 로비에 있는 클라이언트들에게는 방이 업데이트가 되었다고 보냄. 
	for (CSession* session : m_sessionList)
	{
		list<tRoom*>* roomList = CRoomMgr::GetInst()->GetRoomList();

		for (tRoom* room : *roomList) // 현재 존재하는 방들의 정보를 클라이언트들에게 보낸다.
		{
			unsigned long protocol = 0;
			CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::ROOM_RESULT);
			CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::ROOMLIST_UPDATE_SUCCESS);
			Packing(protocol, room->roomName, room->passWord, room->roomNum, room->playerList.size(), session);
		}
	}

	// 해당 방에 이미 선택된 charType을 들어온 클라이언트에게 보낸다.
	//for (CSession* session : _ptr->m_room->readyPlayerList)
	//{
	//	session->m_gameinfo.charType;
	//}

	// 방에 있는 클라이언트들에게 방이 업데이트 되었다는 프로토콜을 보낸다.
	for (CSession* session : _ptr->m_room->playerList)
	{
		if (session != _ptr)
		{
			CStageMgr::GetInst()->UpdateStage(session);
		}
	}

	unsigned long protocol = 0;
	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::ROOM_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::ENTER_ROOM_SUCCESS);

	_ptr->Packing(protocol, nullptr);
}

void CLobbyMgr::ExitLobby(CSession* _ptr)
{
	_ptr->SetState(_ptr->GetLoginState());
	m_sessionList.remove(_ptr);
}

void CLobbyMgr::ChatSendProc(CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	TCHAR str[BUFSIZE]; ZeroMemory(str, BUFSIZE * 2);
	_ptr->UnPacking(buf);
	UnPacking(buf, str);

	// msg에 클라이언트의 닉네임을 붙인다.
	TCHAR msg[BUFSIZE]; ZeroMemory(msg, BUFSIZE * 2);
	wsprintf(msg, L"[%s님] : %s", _ptr->GetUserInfo()->nickname, str);

	for (CSession* session : m_sessionList) // 현재 로비에 있는 클라이언트들에게 메시지를 보낸다.
	{
		unsigned long protocol = 0;
		CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::CHAT_RESULT);
		CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::ALL_MSG_SUCCESS);
		Packing(protocol, msg, session);
	}
}

void CLobbyMgr::Packing(unsigned long _protocol, const TCHAR* _msg, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;
	int msg_size = wcslen(_msg) * 2;

	memcpy(ptr, &msg_size, sizeof(int));
	size = size + sizeof(int);
	ptr = ptr + sizeof(int);

	memcpy(ptr, _msg, msg_size);
	size = size + msg_size;

	_ptr->Packing(_protocol, buf, size);
}

void CLobbyMgr::Packing(unsigned long _protocol, const TCHAR* _roomName, const int _roomNum, const int _roomCnt, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;
	int roomName_size = wcslen(_roomName) * 2;

	memcpy(ptr, &roomName_size, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	memcpy(ptr, _roomName, roomName_size);
	size += roomName_size;
	ptr += roomName_size;

	memcpy(ptr, &_roomNum, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	memcpy(ptr, &_roomCnt, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	_ptr->Packing(_protocol, buf, size);
}

void CLobbyMgr::Packing(unsigned long _protocol, const TCHAR* _roomName, TCHAR* _passWord, const int _roomNum, const int _roomCnt, CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	BYTE* ptr = buf;
	int size = 0;
	int roomName_size = wcslen(_roomName) * 2;
	int passWord_size = wcslen(_passWord) * 2;

	memcpy(ptr, &roomName_size, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	memcpy(ptr, _roomName, roomName_size);
	size += roomName_size;
	ptr += roomName_size;

	memcpy(ptr, &passWord_size, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	memcpy(ptr, _passWord, passWord_size);
	size += passWord_size;
	ptr += passWord_size;

	memcpy(ptr, &_roomNum, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	memcpy(ptr, &_roomCnt, sizeof(int));
	size += sizeof(int);
	ptr += sizeof(int);

	_ptr->Packing(_protocol, buf, size);
}

void CLobbyMgr::UnPacking(const BYTE* _buf, TCHAR* _str)
{
	const BYTE* ptr = _buf;
	int strSize = 0;

	memcpy(&strSize, ptr, sizeof(int));
	ptr += sizeof(int);

	memcpy(_str, ptr, strSize);
}

void CLobbyMgr::UnPacking(const BYTE* _buf, TCHAR* _str1, TCHAR* _str2)
{
	const BYTE* ptr = _buf;
	int strSize = 0;

	memcpy(&strSize, ptr, sizeof(int));
	ptr += sizeof(int);

	memcpy(_str1, ptr, strSize);
	ptr += strSize;

	memcpy(&strSize, ptr, sizeof(int));
	ptr += sizeof(int);

	memcpy(_str2, ptr, strSize);
}

void CLobbyMgr::UnPacking(const BYTE* _buf, int& _data)
{
	const BYTE* ptr = _buf;
	int msg_size = 0;

	memcpy(&_data, ptr, sizeof(int));
}

