#pragma once

class CPlayer;
class CMonster;
class CBoss;
class CSpirit;
class CObject;
class CItem;
class CSession;

class CSector
{
public:
	// ������Ƽ�� ����.
	// Player�� veiwList�� ��������.
	void AddGameObj(t_GameObj _obj);
	void AddPlayer(CSession* _ptr);

private:
	list<CPlayer*> m_playerList; //
	list<CMonster*> m_monsterList;
	list<CSpirit*> m_spiritList;
	list<CObject*> m_objectList;
	list<CItem*> m_itemList;
	CBoss* m_boss;

	list<CSector*> m_veiw_sectorList; // �þ߼��� // �� ���͸� �������� �þ߹����� �ִ� ���͸� ����.

	friend class CInGameMgr;
	friend class CSectorMgr;
};

