using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMap : MonoBehaviour
{
    //declare grid types
    public static int GRIDTYPE_HEXA_MAP = 2;

    public static int hexMapSizeX = 6;
    public static int hexMapSizeZ = 5;

    public Plane m_Plane;

    //시작 위치
    public Transform mapStartPosition;

    //육각형 오브젝트
    public GameObject hexaIndicator;

    //일반적 색상
    public Color indicatorDefaultColor;

    //충돌 이후의 색상
    public Color indicatorActiveColor;

    //추가한 부분(x축과 z축으로 밀어줘서 Plane 중앙에 타일들이 오도록 하기위해)
    public float XPush = 10f;
    public float ZPush = 10f;

    private GameObject BossObj;

    public int xp, zp;

    void Start()
    {

        CreateGridPosition();
        CreateIndicators();

        BossObj = GameObject.Find("Boss");

        int x = 0;
        int z = 0;
        //각 맵 정보에 데이터 넣어주기.
        foreach (var item in DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList)
        {
            MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapData = item;
            MainManager.Instance.GetStageManager().m_MapInfo[z, x].row = z;       //가로
            MainManager.Instance.GetStageManager().m_MapInfo[z, x].column = x;    //세로
            MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapObject = mapIndicatorArray[z, x];
            MainManager.Instance.GetStageManager().m_MapInfo[z, x].BossEffect = false;


            //if (MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapData.BossSummon != 0)
            //{
            //	BossObj.transform.position = mapGridPositions[z, x] + new Vector3(0, 2.3f, 0);
            //	MainManager.Instance.GetStageManager().m_BossRow = z;
            //	MainManager.Instance.GetStageManager().m_BossColumn = x;
            //}

            //디버그용
            if (xp == x && zp == z)
            {
                BossObj.transform.position = mapGridPositions[z, x] + new Vector3(0, 2.3f, 0);
                MainManager.Instance.GetStageManager().m_BossRow = z;
                MainManager.Instance.GetStageManager().m_BossColumn = x;
            }
            x++;
            if (x == hexMapSizeX)
            {
                z++;
                x = 0;
            }
        }

        MainManager.Instance.GetStageManager().mapX = hexMapSizeX;
        MainManager.Instance.GetStageManager().mapZ = hexMapSizeZ;

        m_Plane = new Plane(Vector3.up, Vector3.zero);
    }

    //맵 그리드 포지션 위치를 벡터의 배열에 저장
    [HideInInspector]
    public Vector3[,] mapGridPositions;

    //맵의 그리드 포지션 설정
    private void CreateGridPosition()
    {

        //수정본
        // m_MapInfo = new MapInfo[hexMapSizeZ, hexMapSizeX];
        //맵 생성하는 그리드 배열 선언
        mapGridPositions = new Vector3[hexMapSizeZ, hexMapSizeX];

        Vector3 tempPos = new Vector3(-94, 0, 67);       //Plane 좌상단 위치 좌표

        //맵 위치 생성
        for (int z = 0; z < hexMapSizeZ; z++)
        {
            float offsetZ = z * -27f - ZPush;

            if (z % 2 == 1)//2번쨰와 4번쨰는(0번부터 시작해서 1번과 3번라인)은 135라인보다 x축이 더 앞에 있기때문에 그 값(15.72)만큼 더해준다.
            {
                for (int x = 0; x < hexMapSizeX; x++)
                {
                    //x, z축 계산
                    float offsetX = (x * 31.5f) + 15.72f + XPush;

                    //각 칸의 포지션을 계산 후에 저장
                    //Vector3 position = GetMapHitPoint(tempPos + new Vector3(offsetX,0, offsetZ));

                    //계산된 포지션을 그리드 배열에 맞게 저장
                    mapGridPositions[z, x] = tempPos + new Vector3(offsetX, -3.3f, offsetZ);
                    MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapPos = mapGridPositions[z, x];


                }
            }
            else
            {
                for (int x = 0; x < hexMapSizeX; x++)
                {

                    //x, z축 계산
                    float offsetX = x * 31.5f + XPush;
                    //각 칸의 포지션을 계산 후에 저장
                    //Vector3 position = GetMapHitPoint(tempPos + new Vector3(offsetX,0, offsetZ));

                    //계산된 포지션을 그리드 배열에 맞게 저장
                    mapGridPositions[z, x] = tempPos + new Vector3(offsetX, -3.3f, offsetZ);
                    MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapPos = mapGridPositions[z, x];
                }
            }

        }

    }

    //육각형의 배열을 선언
    [HideInInspector]
    public GameObject[] ownIndicatorArray;
    [HideInInspector]
    public GameObject[,] mapIndicatorArray;

    //생성된 육각형들의 부모가 될 빈 게임 오브젝트
    private GameObject indicatorContainer;

    //포지션에 맞게 육각형 생성
    private void CreateIndicators()
    {
        //create a container for indicators
        indicatorContainer = new GameObject();
        indicatorContainer.name = "IndicatorContainer";

        //create a container for triggers
        GameObject triggerContainer = new GameObject();
        triggerContainer.name = "TriggerContainer";

        //수정한 부분
        mapIndicatorArray = new GameObject[hexMapSizeZ, hexMapSizeX];

        for (int z = 0; z < hexMapSizeZ; z++)
        {
            for (int x = 0; x < hexMapSizeX; x++)
            {
                //create indicator gameobject
                GameObject indicatorGO = Instantiate(hexaIndicator);

                //set indicator gameobject position
                indicatorGO.transform.position = mapGridPositions[z, x];
                MainManager.Instance.GetStageManager().m_MapInfo[z, x].MapPos = mapGridPositions[z, x];

                //set indicator parent
                indicatorGO.transform.parent = indicatorContainer.transform;

                indicatorGO.GetComponent<ColiderChk>().m_row = z;
                indicatorGO.GetComponent<ColiderChk>().m_coulmn = x;

                //store indicator gameobject in array
                mapIndicatorArray[z, x] = indicatorGO;

            }
        }


    }

    public Vector3 GetMapHitPoint(Vector3 p) //마우스 레이캐스트 이벤트
    {
        Vector3 newPos = p;

        RaycastHit hit;

        if (Physics.Raycast(newPos + new Vector3(0, 10, 0), Vector3.down, out hit, 15))
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
}
