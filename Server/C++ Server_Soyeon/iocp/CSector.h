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
	// 프로퍼티는 자유.
	// Player만 veiwList를 갖고있음.
	void AddGameObj(t_GameObj _obj);
	void AddPlayer(CSession* _ptr);

private:
	list<CPlayer*> m_playerList; //
	list<CMonster*> m_monsterList;
	list<CSpirit*> m_spiritList;
	list<CObject*> m_objectList;
	list<CItem*> m_itemList;
	CBoss* m_boss;

	list<CSector*> m_veiw_sectorList; // 시야섹터 // 내 섹터를 기준으로 시야범위에 있는 섹터를 저장.

	friend class CInGameMgr;
	friend class CSectorMgr;
};

