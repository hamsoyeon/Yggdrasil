#pragma once
#include "CPacket.h"
#include "CState.h"
#include "CLoginState.h"
#include "CLobbyState.h"
#include "CStageState.h"
#include "CInGameState.h"
#include "CRoomMgr.h"
#include "CSector.h"
#include "CMemoryPool.h"

class CGameObj;

#define STRING_SIZE 32

struct t_UserInfo
{
	t_UserInfo()
	{
		ZeroMemory(id, STRING_SIZE * 2);
		ZeroMemory(pw, STRING_SIZE * 2);
		ZeroMemory(nickname, STRING_SIZE * 2);
		is_login = false;
		charType = -1;
	}
	t_UserInfo(TCHAR* _id, TCHAR* _pw, TCHAR* _nickname)
	{
		ZeroMemory(id, STRING_SIZE * 2);
		ZeroMemory(pw, STRING_SIZE * 2);
		ZeroMemory(nickname, STRING_SIZE * 2);
		wcscpy(id, _id);
		wcscpy(pw, _pw);
		wcscpy(nickname, _nickname);
		is_login = false;
		charType = -1;
	}

	TCHAR id[STRING_SIZE];
	TCHAR pw[STRING_SIZE];
	TCHAR nickname[STRING_SIZE];
	bool is_login;

	int charType;
};

class CSession :public CPacket, public CMemoryPool<CSession, 10>
{
public:

	CSession();
	CSession(SOCKET _sock);

	void Init();
	void End();

public:

	t_UserInfo* GetUserInfo() { return m_userinfo; }
	CState* GetState() { return m_curstate; }
	CState* GetLoginState() { return m_loginstate; }
	CState* GetLobbyState() { return m_lobbystate; }
	CState* GetStageState() { return m_stagestate; }
	CState* GetInGameState() { return m_InGameState; }

	void SetState(CState* _state)
	{
		m_curstate = _state;
	}
	void SetUserInfo(t_UserInfo _tuserinfo)
	{
		memcpy(&m_userinfo, &_tuserinfo, sizeof(t_UserInfo));
	}
	void SetUserInfo(TCHAR* _id, TCHAR* _pw, TCHAR* _nick, bool _flag)
	{
		ZeroMemory(m_userinfo->id, STRING_SIZE * 2);
		ZeroMemory(m_userinfo->pw, STRING_SIZE * 2);
		ZeroMemory(m_userinfo->nickname, STRING_SIZE * 2);
		if (_id == NULL || _pw == NULL || _nick == NULL)
		{
			m_userinfo->is_login = _flag;
			return;
		}
		wcscpy(m_userinfo->id, _id);
		wcscpy(m_userinfo->pw, _pw);
		wcscpy(m_userinfo->nickname, _nick);
		m_userinfo->is_login = _flag;
	}

private:

	t_UserInfo* m_userinfo;

	// state
	CState* m_curstate;
	CLoginState* m_loginstate;
	CLobbyState* m_lobbystate;
	CStageState* m_stagestate;
	CInGameState* m_InGameState;

	// sector
	CSector* m_sector;

	// room
	tRoom* m_room;

	friend class CState;
	friend class CInGameMgr;
	friend class CRoomMgr;
	friend class CLobbyMgr;
	friend class CStageMgr;

};

// session은 sector를 소유. 
// sector는 현재 플레이어가 존재하는 스테이즈 정보를 담고 있다.
// viewList는 session의 근처에 있는 obj들을 담는다.

