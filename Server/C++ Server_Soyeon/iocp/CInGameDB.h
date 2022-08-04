#pragma once
#include "CDB.h"

class CInGameDB : public CDB
{
public:
	CInGameDB();
	CInGameDB(MYSQL _sql);
	virtual ~CInGameDB() {}

public:
	vector<t_GameObj> GetVecStage() { return m_stageData; }
private:
	vector<t_GameObj> m_stageData;
};

