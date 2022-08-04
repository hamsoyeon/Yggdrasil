#include "pch.h"
#include "CProtocolMgr.h"
#include "CSession.h"
#include "CInGameState.h"
#include "CInGameMgr.h"

void CInGameState::Recv()
{
	int protocol;
	m_session->UnPacking(protocol);

	unsigned long sub_protocol = CProtocolMgr::GetInst()->GetSubProtocol(protocol);
	unsigned long detail_protocol = CProtocolMgr::GetInst()->GetDetailProtocol(protocol);

	switch (sub_protocol)
	{
	case (int)CInGameMgr::SUB_PROTOCOL::PLAYER:
	{
		switch (detail_protocol)
		{
		case (int)CInGameMgr::DETAIL_PROTOCOL::MOVE:
			CInGameMgr::GetInst()->Move(m_session);
		break;
		}
	}
	break;
	case (int)CInGameMgr::SUB_PROTOCOL::INGAME:
	{
		switch (detail_protocol)
		{
		case (int)CInGameMgr::DETAIL_PROTOCOL::INIT: // sector init
			CInGameMgr::GetInst()->StageInit(m_session);
			break;
		}
	}
	break;
	}
}

void CInGameState::Send()
{
}
