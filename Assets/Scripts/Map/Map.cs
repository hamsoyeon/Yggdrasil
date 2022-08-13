using UnityEngine;
public class Map : MonoBehaviour
{
    //struct MapInfo
    //{
    //    public Map_TableExcel MapData;     //맵에 엑셀 데이터
    //    public Vector3 MapPos;             //맵에 위치
    //}

    //각 맵 정보에 데이터 넣어주기.
    //foreach(var item in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList)
    //{
    //    m_MapInfo[z, x].MapData = item;
    //    x++;
    //    if(x == hexMapSizeX)
    //    {
    //        z++;
    //        x = 0;
    //    }
    //}

    //declare grid types
    public static int GRIDTYPE_HEXA_MAP = 2;

    public static int hexMapSizeX = 6;
    public static int hexMapSizeZ = 5;

    //시작 위치
    public Transform mapStartPosition;

    //육각형 오브젝트
    public GameObject hexaIndicator;

    //일반적 색상
    public Color indicatorDefaultColor;

    //충돌 이후의 색상
    public Color indicatorActiveColor;


    
    void Start()
    {
        CreateGridPosition();
        CreateIndicators();


    }

    //맵 그리드 포지션 위치를 벡터의 배열에 저장
    [HideInInspector]
    public Vector3[,] mapGridPositions;

    //맵의 그리드 포지션 설정
    private void CreateGridPosition()
    {
        //맵 생성하는 그리드 배열 선언
        mapGridPositions = new Vector3[hexMapSizeX, hexMapSizeZ];

        //맵 위치 생성
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ; z++)
            {
                //육각형 6x5형식의 맵이기에 위와 아래가 맞물리도록 설정을 위한 rowOffset 변수 선언
                int rowOffset = z % 2;

                //x, z축 계산
                float offsetX = x * 31.4f + rowOffset * -15.7f;
                float offsetZ = z * -27.2f;
                float offsetY = -3f;

                //각 칸의 포지션을 계산 후에 저장
                Vector3 position = GetMapHitPoint(mapStartPosition.position + new Vector3(offsetX, offsetY, offsetZ));

                //계산된 포지션을 그리드 배열에 맞게 저장
                mapGridPositions[x, z] = position;
            }

        }

    }

    //육각형의 배열을 선언
    [HideInInspector]
    public GameObject[] ownIndicatorArray;
    [HideInInspector]
    public GameObject[,] mapIndicatorArray;

    //트리거의 정보를 포함할 배열 선언
    //TriggerInfo를 지워서 에러남.
    //[HideInInspector]
   //public TriggerInfo[] ownTriggerArray;
    //[HideInInspector]
   // public TriggerInfo[,] mapGridTriggerArray;





    //생성된 육각형들의 부모가 될 빈 게임 오브젝트
    private GameObject indicatorContainer;

    //포지션에 맞게 육각형 생성
    private void CreateIndicators()
    {
        //육각형을 담을 컨테이너 생성
        indicatorContainer = new GameObject();
        indicatorContainer.name = "IndicatorContainer";

        //트리거를 담을 컨테이너 생성
        GameObject triggerContainer = new GameObject();
        triggerContainer.name = "TriggerContainer";

        mapIndicatorArray = new GameObject[hexMapSizeX, hexMapSizeZ];
        //mapGridTriggerArray = new TriggerInfo[hexMapSizeX, hexMapSizeZ];

        //iterate map grid position
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ; z++)
            {
                //육각형 오브젝트 생성
                GameObject indicatorGO = Instantiate(hexaIndicator);

                //육각형 오브젝트의 포지션 값 설정
                indicatorGO.transform.position = mapGridPositions[x, z];

                //육각형 오브젝트들의 부모 설정
                indicatorGO.transform.parent = indicatorContainer.transform;

                //생성이된 육각형 오브젝트를 맵육각형배열에 저장
                mapIndicatorArray[x, z] = indicatorGO;

                //create trigger gameobject
                GameObject trigger = CreateSphereTrigger(GRIDTYPE_HEXA_MAP, x, z);

                //set trigger parent
                trigger.transform.parent = triggerContainer.transform;

                //set trigger gameobject position
                trigger.transform.position = mapGridPositions[x, z];

                //store triggerinfo
               // mapGridTriggerArray[x, z] = trigger.GetComponent<TriggerInfo>();
            }
        }

    }

    public Vector3 GetMapHitPoint(Vector3 p) 
    {
        Vector3 newPos = p;

        RaycastHit hit;

        if (Physics.Raycast(newPos + new Vector3(0, -3, 0), Vector3.down, out hit, 15))
        {
            newPos = hit.point;
        }

        return newPos;
    }

    public void resetIndicators()
    {
        for (int x = 0; x < hexMapSizeX; x++)
        {
            for (int z = 0; z < hexMapSizeZ; z++)
            {
                mapIndicatorArray[x, z].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
            }
        }


        for (int x = 0; x < 9; x++)
        {
            ownIndicatorArray[x].GetComponent<MeshRenderer>().material.color = indicatorDefaultColor;
        }

    }

    //트리거의 그리드 타입을 통해서 무슨 타일인지 찾는 함수
    //public GameObject GetIndicatorFromTriggerInfo(TriggerInfo triggerinfo)
    //{
    //    GameObject triggerGo = null;

    //    if (triggerinfo.gridType == GRIDTYPE_HEXA_MAP)
    //    {
    //        triggerGo = mapIndicatorArray[triggerinfo.gridX, triggerinfo.gridZ];
    //    }

    //    return triggerGo;
    //}

    //스피어콜라이더를 생성하는 함수
    private GameObject CreateSphereTrigger(int type, int x, int z)
    {
        //트리거라는 새로운 오브젝트 생성
        GameObject trigger = new GameObject();

        //콜라이더 컴포넌트 추가
        SphereCollider collider = trigger.AddComponent<SphereCollider>();

        //콜라이더 크기 설정
        collider.radius = 15f;

        //콜라이더의 트리거를 설정 
        collider.isTrigger = true;

        //트리거의 정보를 트리거 인포에 저장
       // TriggerInfo trigerInfo = trigger.AddComponent<TriggerInfo>();
        //trigerInfo.gridType = type;
       // trigerInfo.gridX = x;
        //trigerInfo.gridZ = z;

        trigger.layer = LayerMask.NameToLayer("Triggers");

        return trigger;
    }
}

