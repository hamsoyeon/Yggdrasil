#pragma once

struct t_pos
{
	float x, y, z;
};

struct t_GameObj
{
	t_GameObj()
	{
		ZeroMemory(m_charName, STR_SIZE * 2);
	}
	TCHAR m_charName[STR_SIZE];
	int m_kind;
	int m_objID;
	t_pos m_objPos;
};