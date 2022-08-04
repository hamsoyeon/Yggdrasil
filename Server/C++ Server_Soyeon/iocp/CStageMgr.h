#pragma once

class CSession;
class CLock;

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
		STAGE,

		STAGE_RESULTL,
	};

	enum class DETAIL_PROTOCOL
	{
		// STAGE
		STAGE_UPDATE = 1,

	};

	enum class SERVER_DETAIL_PROTOCOL
	{
		// STAGE_RESULTL
		STAGE_UPDATE_RESULT = 1,
	};

public:
	void UpdateStage(CSession* _ptr);

public:
	CLock* m_lock;

	// stage�� update�ش޶�� ��û�� ����, room�� �ִ� session���� nick�� ������.
	void Packing(unsigned long _protocol, const TCHAR* _str, CSession* _ptr);

};

