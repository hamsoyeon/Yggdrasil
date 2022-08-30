using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
   
    public PlayerManager playerManager;
    public GameObject UICanvas;
    public GameObject spritSkillPanel;
    public Slider hpBar;
    public Sprite buttonTexture;

    public Button p_spiritSkillBtn;

    public List<Button> spirit_Buttons;


    public float dummyHp;
    [SerializeField]
    private int m_SkillCount = 6;

    void Start()
    {
        playerManager = GameObject.Find("Player").transform.GetChild(0).gameObject.GetComponent<PlayerManager>();
        //playerManager = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();


        UICanvas = GameObject.Find("UICanvas");
        spritSkillPanel = GameObject.Find("SpritSkillPanel");
        hpBar = UICanvas.GetComponentInChildren<Slider>();

        for (int i = 0; i < m_SkillCount; i++)
        {
            Button child = Instantiate(p_spiritSkillBtn);
            child.transform.SetParent(spritSkillPanel.transform);
            child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerManager.m_SpiritSkillKey[i].ToString();

            child.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(255, 255, 255, 255);
            child.GetComponent<Image>().sprite = buttonTexture;
            spirit_Buttons.Add(child);

        }

    }

    // Update is called once per frame
    void Update()
    {
        
        hpBar.value = playerManager.m_PerHp;
        //dummyHp = hpBar.value;
    }
}
