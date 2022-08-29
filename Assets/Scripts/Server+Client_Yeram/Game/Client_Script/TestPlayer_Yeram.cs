using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer_Yeram : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SectorTest.Instance.InObjectCheck(this.transform.position);
    }
}
