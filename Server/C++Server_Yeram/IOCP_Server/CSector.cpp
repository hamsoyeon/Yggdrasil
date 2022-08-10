#include "pch.h"
#include "CSector.h"

QuadNode::QuadNode()
{
}

QuadNode::QuadNode(Vector3 _senter_pos,Vector3 _distance):CSector(_senter_pos,_distance)
{
    
}

QuadNode::~QuadNode()
{
    while (m_children.size() != 0)
    {
        QuadNode* item = m_children.back();
        m_children.pop_back();
        delete item;
    }
}

void QuadNode::AddChildren(QuadNode* _child_node)
{
    _child_node->m_parent = this;
    m_children.push_back(_child_node);
}

QuadNode* QuadNode::GetChildNode(int index)
{
    int i = 0;
    for (vector<QuadNode*>::iterator itr = m_children.begin(); itr != m_children.end(); itr++)
    {
        if (i == index)
        {
            return *itr;
        }
        i++;
    }
    return nullptr;
}

CSector::CSector()
{
}

CSector::CSector(Vector3 _senter_pos,Vector3 _distance)
{
    m_senter_pos = _senter_pos;
    m_distance = _distance;
}

BOOL CSector::IsInSector(const Vector3 _obj_pos)
{
    if (_obj_pos.x >= m_senter_pos.x - m_distance.x && _obj_pos.x <= m_senter_pos.x + m_distance.x
        && _obj_pos.z >= m_senter_pos.z + m_distance.z && _obj_pos.z <= m_senter_pos.z - m_distance.z)
    {
        return true;
    }
    
    return false;
}

int QuadNode::Child_Size()
{
    return m_children.size();
}
