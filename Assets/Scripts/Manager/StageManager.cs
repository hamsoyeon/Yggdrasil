using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageManager : MonoBehaviour
{

    //추가 전체 맵 정보
    public struct MapInfo
    {
        public GameObject MapObject;
        public GameObject BossEffectObject;
        public bool Spirit;
        public GameObject SpiritEffectObject;
        public Map_TableExcel MapData;     //맵에 엑셀 데이터
        public Vector3 MapPos;             //맵에 위치
        public int row;
        public int column;
        public bool BossEffect;
        public bool SpiritEffect;
    }

    //private GameObject BossObj;

    public Dictionary<int, List<Map_TableExcel>> map_info;

    private List<Map_TableExcel> list;

    private int mapMaxCount = 30;


    //현재 플레이어가 있는 타일의 배열정보.
    //보스가 스킬을사용할때 Map에 저장되어있는 MapInfo[row,coulmn] 으로 접근해서 해당 타일의 Pos 를 안아낸후 그 위치 지점으로 부터 OverlapSphere로 범위 판정.
    public int m_PlayerRow;
    public int m_PlayerCoulmn;

    public int m_BossRow;
    public int m_BossColumn;

    public MapInfo[,] m_MapInfo;


    //public TestMap map;
    public int mapX;
    public int mapZ;


    public MapInfo GetPlayerMapInfo()
    {
        return m_MapInfo[m_PlayerRow, m_PlayerCoulmn];
    }

    public MapInfo GetBossMapInfo()
    {
        return m_MapInfo[m_BossRow, m_BossColumn];
    }


    public void SetPlayerRowAndCoulmn(int row, int coulmn)
    {
        m_PlayerRow = row;
        m_PlayerCoulmn = coulmn;
    }

    public StageManager()
    {

        if (null == map_info)
        {
            map_info = new Dictionary<int, List<Map_TableExcel>>();

            for (int i = 0; i < 3; i++)
            {
                int idx = i * 10;

                //맵 데이터 끊을거 생각해줘야함
                //list = DataTableManager.Instance.GetDataTable<Map_TableExcelLoader>().DataList.OrderBy(x => x.).Skip(idx).Take(mapMaxCount).ToList();
                map_info.Add(i + 1, list);
            }

            m_MapInfo = new MapInfo[5, 6];

            //map = GameObject.Find("MapManager").GetComponent<TestMap>();

            //디버그용
            //foreach(var item in map_info[1])
            //{
            //    Debug.Log(item.No);
            //}
        }

    }




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
