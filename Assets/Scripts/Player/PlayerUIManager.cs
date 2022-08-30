using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    static int a;
    public PlayerManager playerManager;
    public GameObject spritSkillPanel;
    public Image hpBar;
    public Sprite buttonTexture;

    public Button p_spiritSkillBtn;

    public List<Button> spirit_Buttons;

    public GameObject minimap_2DIcon_Player;

    [SerializeField]
    private int m_SkillCount = 6;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        spritSkillPanel = GameObject.Find("SpritSkillPanel");
        minimap_2DIcon_Player.transform.parent = GameObject.FindGameObjectWithTag("Player").transform;
        minimap_2DIcon_Player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
        GameObject.FindGameObjectWithTag("Player").layer = 14;
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
        hpBar.fillAmount = playerManager.GetPlayerPerHp();
        
    }
}
