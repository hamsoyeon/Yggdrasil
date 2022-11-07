using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public Collider[] cols;
    public List<GameObject> FoundObeject;
    public List<GameObject> targetList;
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
       
    }

    void Update()
    {
        TargetEnemy = FindNearbyEnemy(transform.position, 10);
        Vector3 interV = TargetEnemy.transform.position - transform.position;

        // target과 나 사이의 거리가 radius 보다 작다면
        if (interV.magnitude <= radius)
        {
            // '타겟-나 벡터'와 '내 정면 벡터'를 내적
            float dot = Vector3.Dot(interV.normalized, transform.forward);
            // 두 벡터 모두 단위 벡터이므로 내적 결과에 cos의 역을 취해서 theta를 구함
            float theta = Mathf.Acos(dot);
            // angleRange와 비교하기 위해 degree로 변환
            float degree = Mathf.Rad2Deg * theta;

            // 시야각 판별
            if (degree <= angleRange / 2f)
            {
                isCollision = true;
                targetList = new List<GameObject>();

                targetList.Add(GameObject.FindGameObjectWithTag("Enemy"));
                targetList.Add(GameObject.FindGameObjectWithTag("Boss"));
            }
            else
                isCollision = false;

        }
        else
            isCollision = false;
    }

    GameObject FindNearbyEnemy(Vector3 StartPos, float distance)
    {
        float Dist = 0f;
        float near = 0f;
        GameObject nearEnemy = null;

        //범위 내의 적을 찾는다.
        Collider[] colls = Physics.OverlapSphere(StartPos, distance, 1 << 9);  //9번째 레이어 = Enemy

        if (colls.Length == 0)
        {
            Debug.Log("범위에 적이 없습니다.");
            return null;
        }
        else
        {
            //적이 있다면 그 적들 중에
            for (int i = 0; i < colls.Length; i++)
            {
                //보스가 있을경우 보스로 고정
                if (colls[i].gameObject.name == "Boss")
                {
                    nearEnemy = colls[i].gameObject;
                    break;
                }

                //정령과의 거리를 계산후
                Dist = Vector3.Distance(StartPos, colls[i].transform.position);

                if (i == 0)
                {
                    near = Dist;
                    nearEnemy = colls[i].gameObject;
                }

                //그 거리가 작다면 거리를 저장하고 해당 오브젝트를 저장
                if (Dist < near)
                {
                    near = Dist;
                    nearEnemy = colls[i].gameObject;
                }
            }

            return nearEnemy;
        }
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

    //유니티 에디터에 부채꼴을 그려줄 메소드
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
