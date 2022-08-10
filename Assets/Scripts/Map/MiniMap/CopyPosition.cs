using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyPosition : MonoBehaviour
{
    [SerializeField]
    private bool x, y, z;                                          //이값들이 true면 target의 좌표, false면 현재좌표를 그대로 사용
    [SerializeField]
    private Transform target;                                      //캐릭터 타겟 Tranaform
    private void Update()
    {
        if (!target)
        {
            return;                                                 //타겟이 없으면 현재 좌표전송하고 매소드 종료
        }

        transform.position = new Vector3(                           //true인 축만 타겟의 축 좌표 쓰기 false면 현제축의 값 그대로 사용
                (x ? target.position.x : transform.position.x),
                (y ? target.position.y : transform.position.y),
                (z ? target.position.z : transform.position.z));
    }
}
