using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshCom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        
    }

    
    [ContextMenu("[실시간빌드]")]
    public void TestBuild()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
