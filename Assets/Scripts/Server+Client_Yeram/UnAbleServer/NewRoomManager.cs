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


    // ------------------------------------ 추가한 내용(남진) by Value
    [Space (10f)]
    [Header("스킬 설명")]

    // 스킬 설명 패널 
    public GameObject m_skInfoPanel;

    

    // 영상 들어갈 변수
    [SerializeField]
    private RawImage m_skVideo;

    // 스킬 아이콘 이미지를 받을 변수 (Resource에 있는 img폴더에 있는 리소스들을 가지고 옴)
    private Sprite[] m_skImg;

    // 버튼 7개 생성 후(Room/Player_BackImg/Image~Image(5) => ShowButton(6) + HideButton(1)) -> 버튼을 클릭하면 설명 패널창이 나온다.
    public List<Button> m_skButtons;

    // 스킬 설명 및 선택 창이 켜져 있는지의 여부(켜져있으면 = TRUE)
    private bool m_isInfo = false;

    private List<int> m_CheckIcon;

    // ---------------------------------------------------------


    MapSlot m_curmap;
    private void Start()
    {
        MapPannel.SetActive(false);
        Init_Map();
        Select_Map_Btn();

        //m_skImg = Resources.LoadAll<Sprite>("Prefabs/Icon"); // Prefabs/Icon폴더에리소스 가져오기.
        




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

    // ------------------------------------ 추가한 내용(남진) by Method

    public void ShowInfoPanel()
    {
        m_isInfo = true;
        m_skInfoPanel.SetActive(m_isInfo);

    }

    public void HideInfoPanel()
    {
        m_isInfo = false;
        m_skInfoPanel.SetActive(false);
    }


    
    public void ShowSkillList()
    {

        // 여기서 스프라이트 이미지 배치.
        for(int i=0; i< m_skImg.Length; i++)
        {
            //m_skImg

            // if( m_CheckIcon의 있는 인덱스는 블러처리)

        }


    }

    // ---------------------------------------------------------

}
