using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUIManager : MonoBehaviour
{
    [SerializeField]
    private BossFSM bossFsm;
    public GameObject minimap_2DIcon_Boss;

    public Slider bossHp_Slider;
    public Slider bossStamina_Slider;
    [SerializeField]
    TextMeshProUGUI bossHp_Text;
    [SerializeField]
    TextMeshProUGUI bossName_Text;




void Start()
    {
        
        //minimap_2DIcon_Boss.transform.position = GameObject.FindGameObjectWithTag("Boss").transform.position;
        bossFsm = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFSM>();

        bossHp_Slider.gameObject.SetActive(false);
        bossStamina_Slider.gameObject.SetActive(false);
        bossHp_Text.gameObject.SetActive(false);
        bossName_Text.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        HandleStamina(bossFsm.GetPerStamina());
        HandleHp(bossFsm.GetPerHp());
        bossHp_Text.text = bossFsm.GetCurrentHp().ToString() + " / " + bossFsm.GetMaxHp().ToString();
        bossName_Text.text = bossFsm.GetBossName();

        if((int)bossFsm.GetCurrentHp() != (int)bossFsm.GetMaxHp())
        {
            bossHp_Slider.gameObject.SetActive(true);
            bossStamina_Slider.gameObject.SetActive(true);
            bossHp_Text.gameObject.SetActive(true);
            bossName_Text.gameObject.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        minimap_2DIcon_Boss.transform.position = GameObject.FindGameObjectWithTag("Boss").transform.position + (Vector3.up * 30);
    }
    void HandleStamina(float _stamina)
    {
        bossStamina_Slider.value = _stamina;
    }
    void HandleHp(float _hp)
    {
       

        bossHp_Slider.value = _hp;

      
    }
}
