#pragma once
struct Vector3
{
    Vector3()
    {
        x = 0;
        y = 0;
        z = 0;
    }
    Vector3(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }
    float x;
    float y;
    float z;
};
enum class E_GameObject
{
    None = 1,
    Player,
    Boss,
    Spirit,
    Monster,
    Max
};
class GameObject
{
public:
    GameObject()
    {
    }
    GameObject(int _index)
    {
        m_index = _index;
    }
    Vector3 GetVector()
    {
        return m_position;
    }
    void SerVector(Vector3 _pos)
    {
        m_position = _pos;
    }
    int GetIndex()
    {
        return m_index;
    }
    E_GameObject GetType()
    {
        return m_objtype;
    }
    void SetType(E_GameObject _type)
    {
        m_objtype = _type;
    }
    virtual ~GameObject()
    {

    }
protected:
    Vector3 m_position;
    int m_index;
    E_GameObject m_obj_type;
};


