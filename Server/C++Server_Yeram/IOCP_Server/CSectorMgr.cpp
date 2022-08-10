#include "pch.h"
#include "CSectorMgr.h"
#include "CSector.h"
#include "GameObject.h"
CSectorMgr* CSectorMgr::GetInst()
{
    return m_instance;
}

void CSectorMgr::Create()
{
    if (m_instance == nullptr)
        m_instance = new CSectorMgr();
}

void CSectorMgr::Destory()
{
    delete m_instance;
}
CSectorMgr::CSectorMgr()
{

}
CSectorMgr::~CSectorMgr()
{
}

void CSectorMgr::Init()
{
    //db에서 값 읽어와서 읽어온 값으로 설정하도록 하기
    //현재는 임시로 값 넣기
    m_start_position = new Vector3(-45, m_default_y, 38);
    m_end_position = new Vector3(195, m_default_y, -142);
    m_tile_distance = new float(30);
    m_h_mapsize = new int(240);
    m_v_mapsize = new int(180);
    m_sector_count = 1;
    m_depth = new int(1);
    m_squared_value = new int(2);

    for (int i = 0; i < *m_depth; i++)
    {
        m_sector_count *= (*m_squared_value);
    }
    m_h_distance = new float(*m_h_mapsize / m_sector_count);
    m_v_distance = new float(*m_v_mapsize / m_sector_count);
    CreateQuadTree();
}

void CSectorMgr::End()
{
    delete m_tile_distance;
    delete m_h_mapsize;
    delete m_v_mapsize;
    delete m_squared_value;
    delete m_depth;
    delete m_h_distance;
    delete m_v_distance;
}

QuadNode* CSectorMgr::CreateQuadTree()
{
    Vector3 distance((*m_h_mapsize) / (*m_squared_value), 0, (*m_v_mapsize) / (*m_squared_value));
    Vector3 senter_pos((m_start_position->x + distance.x), m_default_y, (m_start_position->z - distance.z));

    root = new QuadNode(senter_pos, distance);

    SetChildren(root, senter_pos, distance, 0);

}

void CSectorMgr::SetChildren(QuadNode* _parent, Vector3 _senterpos, Vector3 _distance, int _curdepth)
{
    if (*m_depth == _curdepth)
        return;
    Vector3 distance(_distance.x / (*m_squared_value), 0, _distance.z / (*m_squared_value));
    Vector3* senterpos;
    QuadNode* child;
    for (int i = 0; i < (*m_squared_value) * 2; i++)
    {
        switch (i)
        {
        case 1:// left up
            senterpos = new Vector3(_senterpos.x - distance.x, m_default_y, _senterpos.z + distance.z);
            child = new QuadNode(*senterpos, distance);
            break;
        case 2://right up
            senterpos = new Vector3(_senterpos.x + distance.x, m_default_y, _senterpos.z + distance.z);
            child = new QuadNode(*senterpos, distance);
            break;
        case 3://left down
            senterpos = new Vector3(_senterpos.x - distance.x, m_default_y, _senterpos.z - distance.z);
            child = new QuadNode(*senterpos, distance);
            break;
        case 4://right down
            senterpos = new Vector3(_senterpos.x + distance.x, m_default_y, _senterpos.z - distance.z);
            child = new QuadNode(*senterpos, distance);
            break;
        }
        delete senterpos;
        _parent->AddChildren(child);
        SetChildren(child, *senterpos, distance, _curdepth++);
    }
}

void CSectorMgr::AddObjectNode(QuadNode* _parent,GameObject* obj,int curdepth)
{
    Vector3 obj_pos = obj->GetVector();
    if (*m_depth==curdepth)
    {
        //마지막 깊이에서 검색된 노드의 위치안에 오브젝트 위치가 속하면 해당 노드에 오브젝트 추가.
        if (_parent->IsInSector(obj_pos))
        {
            _parent->AddObject(obj);
        }
    }
    QuadNode* child;
    int size = _parent->Child_Size();
    for (int i = 0; i < size; i++)
    {
        child = _parent->GetChildNode(i);
        //아직 최대 깊이까지 도달 안했을때 
        //부모의 범위 내에 obj_pos가 일치할 때만 해당 영역의 
        //자식 노드를 순회하도록 한다.
        if(child->IsInSector(obj_pos))
        AddObjectNode(child,obj, curdepth++);
    }
   
}

void CSectorMgr::RemoveObjectNode(QuadNode* _parent, GameObject* obj, int _curdepth)
{
}

QuadNode* CSectorMgr::SerchNode(Vector3 _pos)
{
    return nullptr;
}

