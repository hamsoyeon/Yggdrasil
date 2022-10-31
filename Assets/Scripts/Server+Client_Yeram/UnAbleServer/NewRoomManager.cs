using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//using ECharacterType = NewCharacterInfo.ECharacterType;

public class NewRoomManager : Singleton_Ver2.Singleton<NewRoomManager>
{
    public GameObject MapPannel;

    [SerializeField]
    private GameObject selectMap_Content;

    [SerializeField]
    private Button start_Btn;

    public Button map_Change_Btn;
    [SerializeField]
    private Button p_MapBtn;
    [SerializeField]
    private int map_Count;
    [SerializeField]
    private Sprite[] map_Sprit;
    [SerializeField]
    private Image map_View;
    [SerializeField]
    private Image room_Map_View;
    [SerializeField]
    private int m_MapNum;

    [SerializeField]
    private List<Button> m_SelectMap_Btn;

    [SerializeField]
    private Button[] m_SelectChar_Btn;

    MapSlot m_curmap;
    private void Start()
    {
        MapPannel.SetActive(false);
        Init_Map();
        Select_Map_Btn();
    }

    // Start is called before the first frame update
    public void OnClick_Map()
    {
        MapPannel.SetActive(true);
    }
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    public void OnClick_Decide_Map(int mapNum)
    {
        room_Map_View.sprite = m_SelectMap_Btn[mapNum].transform.GetChild(0).GetComponent<Image>().sprite;
        m_curmap = m_SelectMap_Btn[mapNum].GetComponent<MapSlot>();
    }
    public void Select_Map_Btn()
    {
        for (int i = 0; i < map_Count; i++)
        {
            int temp = i;
            m_SelectMap_Btn[temp].onClick.AddListener(() => Map_ViewChange(temp));
            m_SelectMap_Btn[temp].onClick.AddListener(() => OnClick_Decide_Map(temp));
        }
    }
    public void Map_ViewChange(int _mapNum)
    {
        map_View.sprite = m_SelectMap_Btn[_mapNum].transform.GetChild(0).GetComponent<Image>().sprite;
        m_MapNum = _mapNum;
    }
    public void Init_Map()
    {
        for (int i = 0; i < map_Count; i++)
        {
            m_SelectMap_Btn.Add(Instantiate(p_MapBtn));
            m_SelectMap_Btn[i].transform.parent = selectMap_Content.transform;
            m_SelectMap_Btn[i].transform.GetChild(0).GetComponent<Image>().sprite = map_Sprit[i];
            m_SelectMap_Btn[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = map_Sprit[i].name;
            m_SelectMap_Btn[i].gameObject.AddComponent<MapSlot>().__Initialize(i);
        }
        Map_ViewChange(0);
        OnClick_Decide_Map(0);
    }
}
