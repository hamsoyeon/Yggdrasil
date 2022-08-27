#include "pch.h"
#include "CSession.h"

CSession::CSession()
	: CPacket(), CMemoryPool()
{
	m_loginstate = new CLoginState(this);
	m_lobbystate = new CLobbyState(this);
	m_stagestate = new CStageState(this);
	m_InGameState = new CInGameState(this);
	m_curstate = m_loginstate;
	m_userinfo = new t_UserInfo();
}

CSession::CSession(SOCKET _sock)
	: CPacket(_sock)
{
	m_loginstate = new CLoginState(this);
	m_lobbystate = new CLobbyState(this);
	m_stagestate = new CStageState(this);
	m_InGameState = new CInGameState(this);
	m_curstate = m_loginstate;
	//m_curstate = m_InGameState;
	m_userinfo = new t_UserInfo();
}

void CSession::Init()
{
	ZeroMemory(&r_overlap.overlapped, sizeof(OVERLAPPED));
	r_overlap.type = IO_TYPE::RECV;
	r_overlap.session = this;

	ZeroMemory(&s_overlap.overlapped, sizeof(OVERLAPPED));
	s_overlap.type = IO_TYPE::SEND;
	s_overlap.session = this;

}

void CSession::End() // removesession시 마무리작업하는 함수
{
	CloseSocket();
	//base
	//CPacket::Packing();
}
