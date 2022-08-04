#include "pch.h"
#include "CSectorMgr.h"
#include "CSector.h"
#include "CInGameMgr.h"
#include "CDBMgr.h"
#include "CInGameDB.h"

CSectorMgr* CSectorMgr::m_instance = nullptr;

CSectorMgr* CSectorMgr::GetInst()
{
	return m_instance;
}

void CSectorMgr::Create()
{
	if (m_instance == nullptr)
	{
		m_instance = new CSectorMgr;
	}
}

void CSectorMgr::Destroy()
{
	if (m_instance != nullptr)
	{
		delete m_instance;
	}
}

CSectorMgr::CSectorMgr() {}

CSectorMgr::~CSectorMgr()
{
	for (CSector* item : m_sectorList)
	{
		delete item;
	}
}

void CSectorMgr::PlayerSendPacket(CSession* _ptr, int _protocol, bool _moveflag)
{

}

void CSectorMgr::AddSectorList(CSession* _ptr)
{
	vector<t_GameObj> vec = dynamic_cast<CInGameDB*>(CDBMgr::GetInst()->GetDB(DB_TYPE::STAGE))->GetVecStage();

	CSector* sector = new CSector;

	for (t_GameObj item : vec)
	{	
		sector->AddGameObj(item);
	}
	
	//PlayerÀÇ °æ¿ì.. 
	sector->AddPlayer(_ptr);

	m_sectorList.push_back(sector);
}

// 
