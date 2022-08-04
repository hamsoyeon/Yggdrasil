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
		case (int)CStageMgr::DETAIL_PROTOCOL::STAGE_UPDATE:
			CStageMgr::GetInst()->UpdateStage(m_session);
			break;
		}
	}
	break;
	}
}

void CStageState::Send()
{
}
