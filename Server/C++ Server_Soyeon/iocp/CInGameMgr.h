#pragma once

class CSession;

enum class EVENT
{
	UP = 1, DOWN, RIGHT, LEFT,

	SECTOR_INIT,
};

class CInGameMgr
{
public:
	enum class SUB_PROTOCOL
	{
		INGAME,
		PLAYER,

		INGAME_RESULT,
		PLAYER_RESULT,
	};
	enum class DETAIL_PROTOCOL
	{
		MOVE = 1,
		INIT = 2,
	};
	enum class SERVER_DETAIL_PROTOCOL
	{
		// INGAME_RESULT
		STAGE_ENTER_SUCCESS = 1,

		// PLAYER_RESULT
		MOVE_SUCCESS = 1,
		MOVE_FAIL = 2,
	};

public:
	static CInGameMgr* GetInst();
	static void Create();
	static void Destroy();

private:
	static CInGameMgr* m_instance;

	CInGameMgr();
	~CInGameMgr();

public:
	void StageInit(CSession* _ptr);
	void Move(CSession* _ptr);

public:
	void UnPacking(const BYTE* _buf, int _data[]);
	void UnPacking(const BYTE* _buf, TCHAR* _str);

private:


};

