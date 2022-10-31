using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
public enum E_DataTableType
{
    None = -1,
    Boss,
    CharStat,
    SoulStat,
    Map,
    BossSkill,
    Spirit,
    SpiritSkill,
    SubMonster,

    Max
}

//해야할것 : 싱글톤 구현해서 매니저를 싱글톤으로 생성시키기.
[DefaultExecutionOrder(-99)]
public class DataTableManager : Singleton_Ver1.Singleton<DataTableManager>
{
    #region ScriptableObject 설명
    /*ScriptableObject 데이터를 사용하기 위함, 직렬화 하여 유니티에서 생성 및 관리 가능.
	https://junwe99.tistory.com/13
	https://everyday-devup.tistory.com/53
	*/
//<<<<<<< Updated upstream
    #endregion
    [SerializeField]
    protected List<ScriptableObject> m_DataTableList;

    protected static Dictionary<E_DataTableType, ScriptableObject> m_DataTables;
    #region 다른 스크립트에서 데이터 호출

    public List<ScriptableObject> GetDataList()
    {
        return m_DataTableList;
    }

    public T GetDataTable<T>() where T : ScriptableObject
    {
        string typeName = typeof(T).ToString().Split('_')[0];
        E_DataTableType type;
        //문자열을 열거형으로 변환.
        if (Enum.TryParse<E_DataTableType>(typeName, out type))
        {
            return m_DataTables[type] as T;
        }
        return null;
    }
    #endregion



    private void Awake()
    {
        UnityEngine.Object[] tempResource = Resources.LoadAll("ExcelDataObject");
        foreach (var element in tempResource)
        {
            m_DataTableList.Add((ScriptableObject)element);
        }

        m_DataTables = new Dictionary<E_DataTableType, ScriptableObject>();

        for (E_DataTableType i = E_DataTableType.None + 1; i < E_DataTableType.Max; ++i)
        {
            //datatable list(데이터 프리팹 목록)에서 타입과 일치하는 것이 있으면 딕셔너리에 추가한다.

            #region SingleOrDefault 설명
            //SingleOrDefault :같은 이름의 데이터 1개 이상을 조회할 시 오류 생김. 1개만 조회할때 사용.
            //https://im-first-rate.tistory.com/91
            #endregion
            m_DataTables.Add(i, m_DataTableList.Where(item => i.ToString() + "_TableLoader" == item.name).SingleOrDefault());
        }
    }





}

