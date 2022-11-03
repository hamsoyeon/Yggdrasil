using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public Collider[] cols;
    public List<GameObject> FoundObeject;
    public GameObject TargetEnemy;
    public float shortDis;

   // public Transform target;    // 부채꼴에 포함되는지 판별할 타겟
    public float angleRange = 30f;
    public float radius = 3f;

    public float recognition = 10;

    Color _blue = new Color(0f, 0f, 1f, 0.2f);
    Color _red = new Color(1f, 0f, 0f, 0.2f);

    bool isCollision = false;

    private void Start()
    {
        FoundObeject = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        shortDis = Vector3.Distance(transform.position, FoundObeject[0].transform.position);
        TargetEnemy = FoundObeject[0];
        //target = GameObject.Find("Boss").transform;
    }

    void Update()
    {
        FoundObeject = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));

        cols = Physics.OverlapSphere(transform.position, recognition, 1 << 9);

        MinimalTranform();
        //Vector3 interV = target.position - transform.position;

        //// target과 나 사이의 거리가 radius 보다 작다면
        //if (interV.magnitude <= radius)
        //{
        //    // '타겟-나 벡터'와 '내 정면 벡터'를 내적
        //    float dot = Vector3.Dot(interV.normalized, transform.forward);
        //    // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
        //    float theta = Mathf.Acos(dot);
        //    // angleRange와 비교하기 위해 degree로 변환
        //    float degree = Mathf.Rad2Deg * theta;

        //    // 시야각 판별
        //    if (degree <= angleRange / 2f)
        //        isCollision = true;
        //    else
        //        isCollision = false;

        //}
        //else
        //    isCollision = false;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Enemy"))
    //        t_transform.Add(other.gameObject.transform);
    //}

    public void MinimalTranform()
    {
        foreach(GameObject found in FoundObeject)
        {
            float Distance = Vector3.Distance(gameObject.transform.position, found.transform.position);

            if(Distance < shortDis)
            {
                TargetEnemy = found;
                shortDis = Distance;
            }
        }

        Debug.Log($"가까운적: {TargetEnemy.name}");
    }

    // 유니티 에디터에 부채꼴을 그려줄 메소드
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = isCollision ? _red : _blue;
        // DrawSolidArc(시작점, 노멀벡터(법선벡터), 그려줄 방향 벡터, 각도, 반지름)
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, angleRange / 2, radius);
        Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward, -angleRange / 2, radius);

        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, recognition);
#else
        Application
#endif
    }
}
