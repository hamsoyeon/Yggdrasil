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


    [SerializeField]
    // 스킬 아이콘 패널
    private GameObject m_IconPanel;


    // 유저가 고른 스킬 버튼
    public List<Button> m_skillButtons;


    // 스킬 버튼 리스트.
    public List<Button> m_skBtnList;



    // 영상 들어갈 변수
    [SerializeField]
    private RawImage m_skVideo;

    [SerializeField]
    // 스킬 아이콘 이미지를 받을 변수 (Resource에 있는 img폴더에 있는 리소스들을 가지고 옴)
    private Sprite[] m_skImg;

    

    // 스킬 설명 및 선택 창이 켜져 있는지의 여부(켜져있으면 = TRUE)
    private bool m_isInfo = false;

    // 선택한 아이콘을 블러처리할 
    private List<int> m_blurIcon;

    private int m_selectNumber = -1;
    private int[] m_prevNumber;

    // ---------------------------------------------------------


    MapSlot m_curmap;
    private void Start()
    {
        MapPannel.SetActive(false);
        Init_Map();
        Select_Map_Btn();

       
        // 아이콘 패널 가져오기.
        m_IconPanel = m_skInfoPanel.transform.Find("IconPanel").gameObject;

        // 스프라이트 생성 및 할당.
        //m_skImg = new Sprite[16];
        m_skImg = Resources.LoadAll<Sprite>("Icon"); // Icon폴더에리소스 가져오기.

        for (int i=0; i<16; i++)
        {
            int a = i;
            m_skBtnList[i] = m_IconPanel.transform.GetChild(i).gameObject.GetComponent<Button>(); //버튼 할당.
            m_skBtnList[i].interactable = true;
            m_skBtnList[i].GetComponent<Image>().sprite = m_skImg[i];
            m_skBtnList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-190f + (i%4)*120 , 230f - (i/4)*150, 0.0f);
            m_skBtnList[i].onClick.AddListener(() => GetSkillInfo(a));
        }

        // 버튼들 비활성화 처리.
        for (int i = 0; i < m_skillButtons.Count; i++)
        {
            int a = i;
            m_skillButtons[i].interactable = false;
            m_skillButtons[i].onClick.AddListener(() => SetSkillBtn(a));
            
           
        }

        m_prevNumber = new int[6];
        for(int i=0;i<m_prevNumber.Length;i++)
        {
            m_prevNumber[i] = -1;
        }    



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
        m_skInfoPanel.SetActive(true);

        // 버튼들 활성화 처리.
        for (int i = 0; i < m_skillButtons.Count; i++)
            m_skillButtons[i].interactable = true;

    }

    
    public void HideInfoPanel()
    {
        m_isInfo = false;
        m_skInfoPanel.SetActive(false);

        // 버튼들 비활성화 처리.
        for (int i = 0; i < m_skillButtons.Count; i++)
            m_skillButtons[i].interactable = false;
    }


    // 현재 선택한 버튼 셋팅
    public void SetSkillBtn(int index)
    {
        m_selectNumber = index;
        Debug.Log($"{index}번째 버튼 들어옴");
    }

    public void GetSkillInfo(int index)
    {
        // 예외처리 
        if(m_selectNumber <0)
        {
            Debug.Log("버튼이 선택되지 않았습니다.");
            return;
        }

        if(m_skBtnList[index].GetComponent<Image>().color.a != 1.0f)
        {
            Debug.Log("이미 선택한 스킬입니다.");
            return;
        }


        if(m_prevNumber[m_selectNumber] == -1)
        {
            // 처음 선택하는 거라면...
            m_skillButtons[m_selectNumber].GetComponent<Image>().sprite = m_skBtnList[index].GetComponent<Image>().sprite;
            Color color = m_skBtnList[index].GetComponent<Image>().color;
            color.a = 0.5f;
            m_skBtnList[index].GetComponent<Image>().color = color;
            m_prevNumber[m_selectNumber] = index;
        }
        else
        {
            // 이미 선택된 거라면
            Color color = m_skBtnList[m_prevNumber[m_selectNumber]].GetComponent<Image>().color;
            color.a = 1.0f;
            m_skBtnList[m_prevNumber[m_selectNumber]].GetComponent<Image>().color = color;

            m_skillButtons[m_selectNumber].GetComponent<Image>().sprite = m_skBtnList[index].GetComponent<Image>().sprite;
            color = m_skBtnList[index].GetComponent<Image>().color;
            color.a = 0.5f;
            m_skBtnList[index].GetComponent<Image>().color = color;
            m_prevNumber[m_selectNumber] = index;

        }

   


        // 선택된 
        Debug.Log($"IconPanel -> {index}번째 버튼 들어옴");
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
