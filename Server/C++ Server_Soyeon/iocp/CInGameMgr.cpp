#include "pch.h"
#include "CInGameMgr.h"
#include "CSession.h"
#include "CSectorMgr.h"
#include "CProtocolMgr.h"

CInGameMgr* CInGameMgr::m_instance = nullptr;

CInGameMgr* CInGameMgr::GetInst()
{
	return m_instance;
}

void CInGameMgr::Create()
{
	if (m_instance == nullptr)
		m_instance = new CInGameMgr();
}

void CInGameMgr::Destroy()
{
	delete m_instance;
}

CInGameMgr::CInGameMgr() {}

CInGameMgr::~CInGameMgr() {}

void CInGameMgr::StageInit(CSession* _ptr)
{
	// 방장이 시작하면, 
	unsigned long protocol = 0;

	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::INGAME_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::STAGE_ENTER_SUCCESS);
	_ptr->Packing(protocol, nullptr, 0); // 프로토콜만 전달

	CSectorMgr::GetInst()->AddSectorList(_ptr);
}

void CInGameMgr::Move(CSession* _ptr)
{
	BYTE buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	int eve[4]; // up, down, right, left

	_ptr->UnPacking(buf);
	UnPacking(buf, eve);

	unsigned long protocol = 0;

	CProtocolMgr::GetInst()->AddSubProtocol(&protocol, (unsigned long)SUB_PROTOCOL::PLAYER_RESULT);
	CProtocolMgr::GetInst()->AddDetailProtocol(&protocol, (unsigned long)SERVER_DETAIL_PROTOCOL::MOVE_SUCCESS);
	CSectorMgr::GetInst()->PlayerSendPacket(_ptr, protocol, true);

	// 이동성공시.. sector push
	//_ptr->m_sector->m_veiw_sectorList.push_back();
}

void CInGameMgr::UnPacking(const BYTE* _buf, int _data[])
{
	const BYTE* ptr = _buf;

	for (int i = 0; i < 4; i++)
	{
		memcpy(&_data[i], ptr, sizeof(int));
		ptr += sizeof(int);
	}
}

void CInGameMgr::UnPacking(const BYTE* _buf, TCHAR* _str)
{
	const BYTE* ptr = _buf;
	int msg_size = 0;

	memcpy(&msg_size, ptr, sizeof(int));
	ptr += sizeof(int);

	memcpy(_str, ptr, msg_size);
}
