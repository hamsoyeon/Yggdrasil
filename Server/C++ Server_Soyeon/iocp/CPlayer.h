#pragma once

class CSession;

class CPlayer
{
public:
	CPlayer(CSession* _ptr);
	~CPlayer();

private:
	CSession* m_session;
	TCHAR m_name[STR_SIZE];
	t_pos m_pos;
	int m_id;
};

