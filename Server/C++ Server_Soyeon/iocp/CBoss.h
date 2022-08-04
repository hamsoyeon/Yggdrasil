#pragma once
class CBoss
{
public:
	CBoss(t_GameObj _obj);
	~CBoss();

private:
	TCHAR m_name[STR_SIZE];
	t_pos m_pos;
	int m_id;
};

