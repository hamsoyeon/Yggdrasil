using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Hex : MonoBehaviour
{

    Vector3[] vertex = new Vector3[6];
    Vector3 senter = new Vector3(0, 0, 0);
    static float radius = 17;
    const int COUNT = 6;
    const float PI = 3.141592f;


    public Vector3 SenterPos
    {
        get => senter;
    }
    public static float Radius
    {
        get => radius;
    }
    public Hex(Vector3 _sendter,float _radius)
    {
        senter = _sendter;
        radius = _radius;
    }
    public bool InHex(Vector3 _objpos)
    {
        if (Mathf.Abs(_objpos.x - senter.x) < radius && Mathf.Abs(_objpos.z - senter.z) < radius)
        {
            return true;
        }
        else 
            return false;
    }
    public void CreateVertex()
    {
        for (int i = 0; i < COUNT; i++)
        {
            vertex[i] = Point_Hex_Corner(senter, radius, i);
        }

    }
    private Vector3 Point_Hex_Corner(Vector3 _center, float _length, int _count)
    {
        float angle_deg = 60 * _count - 30;
        float angle_rad = PI / 180 * angle_deg;

        return new Vector3(_center.x + _length * Mathf.Cos(angle_rad), 0f, _center.z + _length * Mathf.Sin(angle_rad));
    }
    public Vector3[] GetVertex
    {
        get => vertex;
    }

}
