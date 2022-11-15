using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject spritSkillPanel;
    public Slider hpBar;
    public TextMeshProUGUI hpText;
    public Sprite buttonTexture;

    public Button p_spiritSkillBtn;

    public List<Button> spirit_Buttons;
    public Image[] is_CoolTime;

    public TextMeshProUGUI[] collTime_Text;

    public GameObject minimap_2DIcon_Player;

    public Sprite[] spritSkill_Img;

    //public Color m_ATK_Spirit;
    //public Color m_DEF_Spirit;
    //public Color m_SUP_Spirit;

    [SerializeField]
    private int m_SkillCount = 6;

    private void Awake()
    {
        is_CoolTime = new Image[6];
        collTime_Text = new TextMeshProUGUI[6];
    }

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        spritSkillPanel = GameObject.Find("SpritSkillPanel");

        //minimap_2DIcon_Player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;

        GameObject.FindGameObjectWithTag("Player").layer = 14;
        
        for (int i = 0; i < m_SkillCount; i++)
        {
            Button child = Instantiate(p_spiritSkillBtn);
            child.transform.SetParent(spritSkillPanel.transform);
            child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerManager.m_SpiritSkillKey[i].ToString();

            child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
            child.GetComponent<Image>().sprite = buttonTexture;
            spirit_Buttons.Add(child);
            ColorBlock colorBlock = spirit_Buttons[i].colors;
            is_CoolTime[i] = spirit_Buttons[i].transform.GetChild(1).GetComponent<Image>();
            is_CoolTime[i].fillAmount = 0f;
            //spirit_Buttons[i].transform.GetChild(1).GetComponent<Image>().color = new Color(0, 0, 0, 190);
            collTime_Text[i] = spirit_Buttons[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            spirit_Buttons[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 18;
            spirit_Buttons[i].transform.GetChild(0).GetComponent<RectTransform>().position = new Vector3(-30, -30, 0);

            // -------------------------- 추가한 내용(남진)
            // 유저가 고른 스킬의 이미지를 동적으로 받아와야한다.


            // 메인씬에서 바로 시작할시 데이터매니저가 없기때문에 에러생김 -> 예외처리
            if(DataManager.Instance == null)
            {
                spirit_Buttons[i].GetComponent<Image>().sprite = spritSkill_Img[i];
            }
            else
            {
                string path = "Icon/" + DataManager.Instance.m_userSelectSkillIndex[i].ToString();
                Sprite tempSprite = Resources.Load<Sprite>(path);

                if (tempSprite == null)
                {
                    spirit_Buttons[i].GetComponent<Image>().sprite = spritSkill_Img[i];
                }
                else
                {
                    spirit_Buttons[i].GetComponent<Image>().sprite = tempSprite;
                }
            }
            // ------------------------------------------



            //(r, g, b, a) 기준 빨간색으로 normal Color 지정
            /*switch (i % 3)
            {

                case 0:
                    colorBlock.normalColor = new Color(m_ATK_Spirit.r, m_ATK_Spirit.g, m_ATK_Spirit.b);
                    colorBlock.highlightedColor = new Color(m_ATK_Spirit.r, m_ATK_Spirit.g, m_ATK_Spirit.b);
                    colorBlock.pressedColor = new Color(m_ATK_Spirit.r, m_ATK_Spirit.g, m_ATK_Spirit.b);
                    colorBlock.selectedColor = new Color(m_ATK_Spirit.r, m_ATK_Spirit.g, m_ATK_Spirit.b);
                    break;
                case 1:
                    colorBlock.normalColor = new Color(m_DEF_Spirit.r, m_DEF_Spirit.g, m_DEF_Spirit.b);
                    colorBlock.highlightedColor = new Color(m_DEF_Spirit.r, m_DEF_Spirit.g, m_DEF_Spirit.b);
                    colorBlock.pressedColor = new Color(m_DEF_Spirit.r, m_DEF_Spirit.g, m_DEF_Spirit.b);
                    colorBlock.selectedColor = new Color(m_DEF_Spirit.r, m_DEF_Spirit.g, m_DEF_Spirit.b);
                    break;
                case 2:
                    colorBlock.normalColor = new Color(m_SUP_Spirit.r, m_SUP_Spirit.g, m_SUP_Spirit.b);
                    colorBlock.highlightedColor = new Color(m_SUP_Spirit.r, m_SUP_Spirit.g, m_SUP_Spirit.b);
                    colorBlock.pressedColor = new Color(m_SUP_Spirit.r, m_SUP_Spirit.g, m_SUP_Spirit.b);
                    colorBlock.selectedColor = new Color(m_SUP_Spirit.r, m_SUP_Spirit.g, m_SUP_Spirit.b);
                    break;
            }*/
            spirit_Buttons[i].colors = colorBlock;

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.value = playerManager.GetPlayerPerHp();
        hpText.text = playerManager.GetRealHp().ToString();
        for(int i = 0; i < playerManager.CanSkill.Length; i++)
        {
            if (playerManager.CanSkill[i] == false)//스킬 쿨타임 중일때
            {
                is_CoolTime[i].fillAmount = playerManager.currenCollTime[i] / playerManager.SkillCollTime[i];
                collTime_Text[i].gameObject.SetActive(true);
                collTime_Text[i].text = ((int)playerManager.currenCollTime[i]).ToString();
                spirit_Buttons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                is_CoolTime[i].fillAmount = 0f;
                collTime_Text[i].gameObject.SetActive(false);
                spirit_Buttons[i].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        
    }
    private void FixedUpdate()
    {
        minimap_2DIcon_Player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + (Vector3.up * 10);
    }

}
