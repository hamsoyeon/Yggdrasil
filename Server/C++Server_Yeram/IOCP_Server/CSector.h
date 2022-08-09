#pragma once
class GameObject;
class CSector
{
public:
    CSector(float length);
private:
    list<GameObject*> m_objects;
	//List<CPlayer> m_playerlist
	//List<CMonster> m_monsterlist
	//CBoss*         m_boss
	//List<CSpirit>   m_spiritlist
	//List<CObject>   m_objectlist
	//List<CItem>     m_itemlist
	//List<CSector>   m_view_sectorlist
};

