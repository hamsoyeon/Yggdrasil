#pragma once

class CSector;

class CMonster
{
public:
	CMonster(t_GameObj _obj);
	~CMonster();

private:
	TCHAR m_name[STR_SIZE];
	t_pos m_pos;
	int m_id;

	list<CSector*> m_view_SectorList;

};

