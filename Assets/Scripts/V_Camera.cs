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
    float view_Value_y;
    [SerializeField]
    float view_Value_z;
    [SerializeField]
    float view_Angle_Value;
    private void Awake()
    {
        view_Value_y = 50.0f;
        view_Value_z = 40.0f;
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
    private void FixedUpdate()
    {
        for (int i = 0; i < m_tag.Length; i++)
        {
            target[i].transform.position = GameObject.FindGameObjectWithTag(m_tag[i]).transform.position + (Vector3.up * view_Value_y) + (Vector3.back * view_Value_z);
        }
    }
    private void VC_Init()
    {
        for (int i = 0; i < m_tag.Length; i++)
        {
            if(m_tag[i] == "Boss")
            {
                view_Value_y = 80.0f;
                view_Angle_Value = 50.0f;
            }
            else
            {
                view_Angle_Value = 60.0f;
                view_Value_y = 73.0f;
                view_Value_z = 37.0f;
            }
            var tmpObj = Instantiate(temp, GameObject.FindGameObjectWithTag(m_tag[i]).transform.position + (Vector3.up * view_Value_y) + (Vector3.back * view_Value_z), Quaternion.Euler(view_Angle_Value, 0, 0));
            tmpObj.transform.parent = GameObject.FindGameObjectWithTag(m_tag[i]).transform.parent;
            target[i] = tmpObj.AddComponent<CinemachineVirtualCamera>();
        }
        for (int i = 0; i < m_tag.Length; i++)
        {
            
            if(m_tag[i] != "Boss")
            {
                //target[i].Follow = GameObject.FindGameObjectWithTag(m_tag[i]).transform;
                //target[i].LookAt = GameObject.FindGameObjectWithTag(m_tag[i]).transform;
            }
            target[i].Priority = init_Priority;
            //target[i].
            init_Priority++;
        }
            
    }
}
