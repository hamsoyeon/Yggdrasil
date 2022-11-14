using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Video;
//using ECharacterType = NewCharacterInfo.ECharacterType;

//Singleton_Ver2.Singleton < NewRoomManager >

public class NewRoomManager : MonoBehaviour
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

    // ------------------------------------------ 변경한 내용 (승렬)

    //[SerializeField]
    [Header("선택 맵")]
    public int m_MapNum;

    // ------------------------------------------

    [SerializeField]
    private List<Button> m_SelectMap_Btn;

    [SerializeField]
    private Button[] m_SelectChar_Btn;


    // ------------------------------------ 추가한 내용(남진) by Value
    [Space(10f)]
    [Header("스킬 설명")]

    public Text m_Information;
    // 스킬 설명 패널 
    public GameObject m_skInfoPanel;
    [SerializeField]
    // 스킬 아이콘 패널
    private GameObject m_IconPanel;
    // 유저가 고른 스킬 버튼
    public List<Button> m_skillButtons;
    // 스킬 버튼 리스트.
    public List<Button> m_skBtnList;
    // 그냥 테두리
    public Sprite m_originWindow;
    // 선택된 테두리
    public Sprite m_selectWindow;


    // 영상 들어갈 변수
    [SerializeField]
    private RawImage m_skVideo;

    // 영상 틀어줄 변수
    [SerializeField]
    private VideoPlayer m_VideoPlayer;

    [SerializeField]
    // 스킬 아이콘 이미지를 받을 변수 (Resource에 있는 img폴더에 있는 리소스들을 가지고 옴)
    private Sprite[] m_skImg;

    // 스킬 설명 및 선택 창이 켜져 있는지의 여부(켜져있으면 = TRUE)
    private bool m_isInfo = false;

    // 현재 선택된 스킬 번호
    private int m_selectNumber = -1;
    // 전에 선택된 스킬 번호
    private int[] m_prevNumber;

    // 인덱스와 스킬설명 연결.
    private Dictionary<int, string> m_skText;

    // 유저가 고른 스킬 인덱스  -> 게임 스타트 버튼 시 해당 정보로 유저 스킬창에 이미지를 셋팅해줘야함. (DataManager)
    //private int[] m_skIndex;

    private VideoClip[] m_VideoArr;

    private bool videoCheck = true;



    // ---------------------------------------------------------

    MapSlot m_curmap;
    private void Start()
    {
        MapPannel.SetActive(false);
        Init_Map();
        Select_Map_Btn();

        //m_skIndex = new int[6];  // 시작값 0으로 초기화.

        m_skText = new Dictionary<int, string>();

        m_skText.Add(170001, "한 타일에 회오리 참격을 일으켜 타일에 있는 적에게 데미지를 준다.");
        m_skText.Add(170002, "투사체 공격 스킬, 초당 ~데미지를 입힌다.");
        m_skText.Add(170003, "범위 지정형 무적 스킬, ~초간 지속된다.");
        m_skText.Add(170004, "범위 지정형 넉백 스킬, ~초간 몬스터를 못다가오게 한다.");
        m_skText.Add(170005, "범위 지정형 힐 스킬, 한번에 ~HP를 회복한다.");
        m_skText.Add(170006, "타일 지정형 이속 증가 스킬, ~타일에 설치되어 ~초간 지속된다.");
        m_skText.Add(170007, "1타일 내 적에게 3회 참격 피해를 준다.");
        m_skText.Add(170008, "한 타일에 불기둥을 생성해 닿는 적에게 데미지를 준다.");
        m_skText.Add(170009, "적은 범위에 강력한 폭발 피해를 준다.");
        m_skText.Add(170010, "직선 장판을 5번 발사해 공격한다");
        m_skText.Add(170011, "얼음 공을 발사해 공격한다.");
        m_skText.Add(170012, "범위 내 적에게 3회 참격 피해를 준다.");
        m_skText.Add(170014, "투사체를 발사해 적을 기절시킨다.");
        m_skText.Add(170015, "광범위에 ~hp 회복장판을 펼친다.");
        m_skText.Add(170016, "타일 지정형, ~초동안 공격력을 증가 시킨다.");
        m_skText.Add(170017, "플레이어를 따라오며 3번 ~Hp회복 장판을 펼친다");



        // 아이콘 패널 가져오기.
        m_IconPanel = m_skInfoPanel.transform.Find("IconPanel").gameObject;

        // 스프라이트 생성 및 할당.
        //m_skImg = new Sprite[16];
        m_skImg = Resources.LoadAll<Sprite>("Icon"); // Icon폴더에 리소스 가져오기.
        m_VideoArr = Resources.LoadAll<VideoClip>("Video");  // Video폴더에 리소스 가져오기. 
        
        for (int i = 0; i < 16; i++)
        {
            int a = i;
            m_skBtnList[i] = m_IconPanel.transform.GetChild(i).gameObject.GetComponent<Button>(); //버튼 할당.
            m_skBtnList[i].interactable = true;
            m_skBtnList[i].GetComponent<Image>().sprite = m_skImg[i];
            m_skBtnList[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(-190f + (i % 4) * 120, 230f - (i / 4) * 150, 0.0f);
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
        for (int i = 0; i < m_prevNumber.Length; i++)
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


        m_skillButtons[m_selectNumber].transform.parent.GetComponent<Image>().sprite = m_originWindow;

    }
    // 현재 스킬 선택할 버튼 셋팅
    public void SetSkillBtn(int index)
    {
        // 값이 들어온 것.
        if (m_selectNumber < 0)
        {
            m_skillButtons[index].transform.parent.GetComponent<Image>().sprite = m_selectWindow;
        }
        else
        {
            m_skillButtons[m_selectNumber].transform.parent.GetComponent<Image>().sprite = m_originWindow;
            m_skillButtons[index].transform.parent.GetComponent<Image>().sprite = m_selectWindow;
        }

        m_selectNumber = index;
        Debug.Log($"{index}번째 버튼 들어옴");
    }


    // ButtonEvent -> 스킬 클릭시 해당 스킬 슬롯에 등록.
    public void GetSkillInfo(int index)
    {
        // 예외처리 
        if (m_selectNumber < 0)
        {
            Debug.Log("버튼이 선택되지 않았습니다.");
            return;
        }

        if (m_skBtnList[index].GetComponent<Image>().color.a != 1.0f)
        {
            Debug.Log("이미 선택한 스킬입니다.");
            return;
        }


        if (m_prevNumber[m_selectNumber] == -1)
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



        // 동영상 설정도 해줘야한다.
        if (videoCheck)
        {
            m_VideoPlayer.clip = m_VideoArr[0];
            videoCheck = false;
        }
        else
        {
            m_VideoPlayer.clip = m_VideoArr[1];
            videoCheck = true;
        }

        // 텍스트 설정.
        Debug.Log(m_skImg[index].name);
        int dic_index = int.Parse(m_skImg[index].name);
        m_Information.text = m_skText[dic_index];


        DataManager.Instance.m_userSelectSkillIndex[m_selectNumber] = dic_index;

        // 선택된 
        Debug.Log($"IconPanel -> {index}번째 버튼 들어옴");
    }



    // ---------------------------------------------------------

}
