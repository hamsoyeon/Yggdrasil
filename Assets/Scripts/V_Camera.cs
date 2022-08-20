using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class V_Camera : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera[] target;
    [SerializeField]
    GameObject temp;

    string[] m_tag;
    int init_Priority = 10;

    [SerializeField]
    float view_Value;
    [SerializeField]
    float view_Angle_Value;
    private void Awake()
    {
        view_Value = 50.0f;
        view_Angle_Value = 30.0f;
        m_tag = new string[] { "Boss", "Player" };                         //우선순위 역순으로 입력 ex)Player가 카메라우선권을 가져가야하니 Player가 인덱스를 크게가질것
        target = new CinemachineVirtualCamera[2];
        GameObject.Find("Main Camera").AddComponent<CinemachineBrain>();
        temp = Resources.Load<GameObject>("DumyObj");
        
    }
    private void Start()
    {
        VC_Init();
    }
    private void VC_Init()
    {
        for (int i = 0; i < m_tag.Length; i++)
        {
            if(m_tag[i] == "Boss")
            {
                view_Value = 80.0f;
            }
            else
            {
                view_Value = 50.0f;
            }
            var tmpObj = Instantiate(temp, GameObject.FindGameObjectWithTag(m_tag[i]).transform.position + (Vector3.up * view_Value) + (Vector3.back * view_Value), Quaternion.Euler(view_Angle_Value, 0, 0));
            tmpObj.transform.parent = GameObject.FindGameObjectWithTag(m_tag[i]).transform;
            target[i] = tmpObj.AddComponent<CinemachineVirtualCamera>();
            
        }
        for (int i = 0; i < m_tag.Length; i++)
        {
            target[i].Follow = GameObject.FindGameObjectWithTag(m_tag[i]).transform;
            if(m_tag[i] != "Boss")
            {
                target[i].LookAt = GameObject.FindGameObjectWithTag(m_tag[i]).transform;
            }
            target[i].Priority = init_Priority;
            //target[i].
            init_Priority++;
        }
            
    }
}
