using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    //게임 컨트롤러
    //public GamePlayController gamePlayContoroller;

    //트리거의 정보를 가져와야 하니 맵스크립트를 가져온다.
    public Map map;

    //트리거 레이어를 나누어서 설정이 가능하게 만든다.
    public LayerMask triggerLayer;

    //레이가 시작되는 포지션 값으로 사용할 변수 선언
    private Vector3 rayCastStartPosition;

    //마우스 포지션 값으로 사용하기 위한 변수 선언
    private Vector3 mousePostion;

    //트리거 인포를 통하여서 찾을 수 있게 한다.
    [HideInInspector]
   // public TriggerInfo triggerInfo = null;

    void Start()
    {
        //레이의 시작 위치값을 설정
        rayCastStartPosition = new Vector3(0, 20, 0);
    }

    void Update()
    {
       // triggerInfo = null;

        //맵으로 접근을 하여서 매 프레임마다 색상 초기화
        map.resetIndicators();

        //레이캐스트 선언
        RaycastHit hit;

        //카메라로 부터의 스크린의 점을 통해 레이를 반환한 값을 레이라는 변수에 저장한다.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 100f, triggerLayer, QueryTriggerInteraction.Collide))
        {
            //레이에 맞은 오브젝트의 트리거 인포의 정보를 저장
          //  triggerInfo = hit.collider.gameObject.GetComponent<TriggerInfo>();

            //if (triggerInfo != null)
            //{
            //    //GetIndicatorFromTriggerInfo 함수를 통해서 오브젝트의 정보를 저장
            //    GameObject indicator = map.GetIndicatorFromTriggerInfo(triggerInfo);

            //    // 오브젝트의 색상을 활성화
            //    indicator.GetComponent<MeshRenderer>().material.color = map.indicatorActiveColor;
            //}
            //else
            //    //색상을 초기화
            //    map.resetIndicators();
        }

        if(Input.GetMouseButtonDown(0))
        {
            //마우스 다운에 일어나는 이벤트를 설정
        }

        if (Input.GetMouseButtonUp(0))
        {
            //마우스 업을 하면 일어나는 이벤트를 설정
        }

        //마우스 포지션을 인풋이 되는 마우스 포지션으로 결정
        mousePostion = Input.mousePosition;
    }
}
