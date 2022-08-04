#pragma once
class CObject
{
public:
	CObject(t_GameObj _obj);
	~CObject();

private:
	TCHAR m_name[STR_SIZE];
	t_pos m_pos;
	int m_id;
};

