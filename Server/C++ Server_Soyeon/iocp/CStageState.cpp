#include "pch.h"
#include "CStageState.h"
#include "CProtocolMgr.h"
#include "CSession.h"
#include "CStageMgr.h"

void CStageState::Recv()
{
	int protocol;
	TCHAR buf[BUFSIZE]; ZeroMemory(buf, BUFSIZE);
	m_session->UnPacking(protocol);
	unsigned long sub_protocol = CProtocolMgr::GetInst()->GetSubProtocol(protocol);
	unsigned long detail_protocol = CProtocolMgr::GetInst()->GetDetailProtocol(protocol);

	switch (sub_protocol)
	{
	case (int)CStageMgr::SUB_PROTOCOL::STAGE:
	{
		switch (detail_protocol)
		{
		case (int)CStageMgr::DETAIL_PROTOCOL::STAGE_EXIT:
			CStageMgr::GetInst()->ExitStage(m_session);
			break;
		}
	}
		break;
	case (int)CStageMgr::SUB_PROTOCOL::CHARFIELD:
	{
		switch (detail_protocol)
		{
		case (int)CStageMgr::DETAIL_PROTOCOL::CHARFIELD_UPDATE:
			// 클라이언트가 stage에 들어가면 업데이트된다.
			CStageMgr::GetInst()->UpdateStage(m_session);
			break;
		}
	}
	break;
	case (int)CStageMgr::SUB_PROTOCOL::CLICKBTN:
	{
		switch (detail_protocol)
		{
		case (int)CStageMgr::DETAIL_PROTOCOL::READY:
			CStageMgr::GetInst()->ReadyProc(m_session);
			break;
		case(int)CStageMgr::DETAIL_PROTOCOL::GAME_START:
			CStageMgr::GetInst()->StartProc(m_session);
			break;
		}
	}
	break;
	case (int)CStageMgr::SUB_PROTOCOL::CHAT:
	{
		switch (detail_protocol)
		{
		case (int)CStageMgr::DETAIL_PROTOCOL::ALL_MSG:
			CStageMgr::GetInst()->ChatSendProc(m_session);
			break;
		}
	}
	break;
	}
}

void CStageState::Send()
{
}
