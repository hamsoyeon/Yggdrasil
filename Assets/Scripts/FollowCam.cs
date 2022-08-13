using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    // 메인카메라를 플레이어에게 심어서 이동을 제어한다.

    public GameObject Target;
    [SerializeField]
	private Vector3 camOffset;

    //본인이 무슨 캐릭터를 조종을 하고 있는지 명시를 할 수 있어야 하고 그 카메라를 명시된 사람에게 붙여야 한다.
    //코드상의 명시적인 클라이언트 번호(인덱스 값)를 부여를 하여서 그것으로 판단을 한다.

    void Start()
    {
		//Target = GameObject.Find("Player");
		//camOffset = new Vector3(0,45, -26.5f);

	}

    // Update is called once per frame
    void Update()
    {
        transform.position = Target.transform.position+camOffset;
    }
}
