#pragma once

class CSession;
class CLock;

struct t_UserInfo;

class CStageMgr
{
public:
	static CStageMgr* GetInst();
	static void Create();
	static void Destroy();

private:
	static CStageMgr* m_instance;
	CStageMgr();
	~CStageMgr();

public:
	enum class SUB_PROTOCOL
	{
		CHARFIELD,
		CLICKBTN,
		SELECTEDMAP,
		CHAT,
		STAGE,

		CHARFIELD_RESULTL,
		CLICKBTN_RESULT,
		SELECTEDMAP_RESULT,
		STAGE_RESULT,
		CHAT_RESULT,
	};

	enum class DETAIL_PROTOCOL
	{
		// STAGE
		CHARFIELD_UPDATE = 1,
		OWNER_CHARFILED = 2,
		STAGE_EXIT = 4,

		// CLICK_BTN
		READY = 1,
		GAME_START = 2,
		SELECT_CHAR = 4,

		// CHAT
		ALL_MSG = 1,
	};

	enum class SERVER_DETAIL_PROTOCOL
	{
		// STAGE_RESULTL
		CHARFIELD_UPDATE_RESULT = 1,
		STAGE_OWNER = 2,
		STAGE_EXIT_SUCCESS = 4,

		// CLICKBTN_RESULT
		START_ACTIVE = 1, // 시작버튼 활성화를 알림.
		SELECTED_CHARTYPE = 2, // 캐릭터 선택을 알림.

		// STAGE_RESULT
		ENTER_INGAME = 1,

		// CHAT_RESULT
		ALL_MSG_SUCCESS = 1,
		ALL_MSG_FAIL = 2,
	};

public:

	void UpdateStage(CSession* _ptr);
	void ReadyProc(CSession* _ptr);
	void StartProc(CSession* _ptr);

	void ChatSendProc(CSession* _ptr);
	int SetCharType(CSession* _ptr);

	void ExitStage(CSession* _ptr);

public:

	CLock* m_lock;

	// stage를 update해달라는 요청이 오면, room에 있는 session들의 nick을 보낸다.
	void Packing(unsigned long _protocol, const TCHAR* _str, CSession* _ptr);
	void Packing(unsigned long _protocol, t_UserInfo* _users, int _arrNum, CSession* _ptr);
	void Packing(unsigned long _protocol, bool _flag, CSession* _ptr);
	void Packing(unsigned long _protocol, int _data, CSession* _ptr);
	void Packing(unsigned long _protocol, int _data, bool _flag, CSession* _ptr);

	void UnPacking(const BYTE* _recvbuf, int& _data);
	void UnPacking(const BYTE* _recvbuf, TCHAR* _str);
	void UnPacking(const BYTE* _recvbuf, TCHAR* _str1, TCHAR* _str2);

};

