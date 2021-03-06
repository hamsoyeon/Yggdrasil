#include "pch.h"
#include "CLoginState.h"
#include "CSession.h"
#include "CLoginMgr.h"
#include "CProtocolMgr.h"
#include "CLobbyMgr.h"
void CLoginState::Recv()
{
	unsigned long protocol;
	unsigned long mainprotocol;
	
	m_session->UnPacking(protocol);
	mainprotocol = CProtocolMgr::GetInst()->GetMainProtocol(protocol);
	//현재 받아온게 메뉴정보인지 로그인정보인지 회원가입정보인지 구분
	switch (static_cast<MAINPROTOCOL>(mainprotocol))
	{
	case MAINPROTOCOL::LOGIN:
		CLoginMgr::GetInst()->LoginProcess(m_session);
		break;
	case MAINPROTOCOL::LOBBY:
		is_lobby = true;
		CLoginMgr::GetInst()->EnterLobbyProcess(m_session);
		break;
	}
}

void CLoginState::Send()
{
	if (is_lobby)
	{
		m_session->SetState(m_session->GetLobbyState());
	}
}
