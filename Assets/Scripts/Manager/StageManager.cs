using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MapInformation;

namespace MapInformation
{
    //추가 전체 맵 정보
    public struct MapInfo
    {
        public GameObject MapObject;
        public GameObject BossEffectObject;
        public GameObject BossDelayObject;
        public bool Spirit;
        public GameObject SpiritEffectObject1;
        public GameObject SpiritEffectObject2;
       
        
        public Map_TableExcel MapData;     //맵에 엑셀 데이터
        public Vector3 MapPos;             //맵에 위치
        public int row;
        public int column;
        public bool BossEffect;
        public bool DelayEffect;
        public bool SpiritEffect1;
        public bool SpiritEffect2;

        public bool IsUnWalkable; // 이동불가 타일 bool값 

        public int fCost;
        public int gCost;
        public int hCost;
    }
}





public class StageManager : MonoBehaviour
{


    //private GameObject BossObj;

    public class GetObjectWorldPos
    {
        public Vector3 bossPos;
        public Vector3 PlayerPos;
        public Vector3 ItemPos;  //아직 값 없음.
    }

    public GetObjectWorldPos m_GetWorldPosByObjects;


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

    

    public void GetBossAndPlayerRowBuyColumn(int bossRow,int bossColumn, int playerRow,int playerColumn)
    {
        bossRow = m_BossRow;
        bossColumn = m_BossColumn;
        playerRow = m_PlayerRow;
        playerColumn = m_PlayerCoulmn;
    }


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

    public void SetBossRowAndCoulmn(int row, int column)
    {
        m_BossRow = row;
        m_BossColumn = column;
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
            

            m_GetWorldPosByObjects = new GetObjectWorldPos();


        }

    }

}
