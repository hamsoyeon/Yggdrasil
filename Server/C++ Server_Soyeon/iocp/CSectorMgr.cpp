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

CSectorMgr::~CSectorMgr() {}

void CSectorMgr::PlayerSendPacket(CSession* _ptr, int _protocol, bool _moveflag)
{

}

void CSectorMgr::Init()
{
	vector<t_GameObj> vec = dynamic_cast<CInGameDB*>(CDBMgr::GetInst()->GetDB(DB_TYPE::STAGE))->GetVecStage();

	CSector sector[CELL_SIZE][CELL_SIZE];

	for (t_GameObj item : vec)
	{
		int z = (UINT)((item.m_objPos.z + MAX_Y) / SIZE_Z);
		int x = (UINT)((item.m_objPos.x + MAX_X) / SIZE_X);

		if (z >= CELL_SIZE)
		{
			z -= 1;
		}
		if (x >= CELL_SIZE)
		{
			x -= 1;
		}

		sector[z][x].AddGameObj(item);
	}

	for (int i = 0; i < CELL_SIZE; i++)
	{
		for (int j = 0; j < CELL_SIZE; j++)
		{
			m_sectorList[i][j].push_back(sector[i][j]);
		}
	}

	for (int i = 0; i < CELL_SIZE; i++)
	{
		for (int j = 0; j < CELL_SIZE; j++)
		{
			for (CSector setor : m_sectorList[i][j])
			{
				printf("m_sectorList[%d][%d]\n", i, j);
				printf("%d\n", setor.m_objectList.size());
				printf("%d\n", setor.m_itemList.size());
				printf("%d\n", setor.m_monsterList.size());
				printf("%d\n", setor.m_playerList.size());
				printf("%d\n", setor.m_spiritList.size());
				printf("\n\n");
			}
		}
	}
}

void CSectorMgr::AddSectorList(CSession* _ptr)
{
	//vector<t_GameObj> vec = dynamic_cast<CInGameDB*>(CDBMgr::GetInst()->GetDB(DB_TYPE::STAGE))->GetVecStage();

	//CSector* sector = new CSector;

	//for (t_GameObj item : vec)
	//{	
	//	sector->AddGameObj(item);
	//}
	//
	////PlayerÀÇ °æ¿ì.. 
	//sector->AddPlayer(_ptr);

	//m_sectorList.push_back(sector);
}

