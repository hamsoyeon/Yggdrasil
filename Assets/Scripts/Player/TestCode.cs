using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public Collider[] colls;
    public Vector3 boxSize = new Vector3(2, 2, 2);

    void Update()
    {
        colls = Physics.OverlapBox(transform.position, boxSize * 0.5f, transform.rotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.forward, boxSize);
    }
}
