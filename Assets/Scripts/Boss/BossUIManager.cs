using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIManager : MonoBehaviour
{
    public GameObject minimap_2DIcon_Boss;
    // Start is called before the first frame update
    void Start()
    {
        minimap_2DIcon_Boss.transform.parent = GameObject.FindGameObjectWithTag("Boss").transform;
        minimap_2DIcon_Boss.transform.position = GameObject.FindGameObjectWithTag("Boss").transform.position + (Vector3.up * 30);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
