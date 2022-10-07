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
    // Start is called before the first frame update
    void Start()
    {
        minimap_2DIcon_Boss.transform.parent = GameObject.FindGameObjectWithTag("Boss").transform;
        minimap_2DIcon_Boss.transform.position = GameObject.FindGameObjectWithTag("Boss").transform.position + (Vector3.up * 30);
        bossFsm = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleStamina(bossFsm.GetPerStamina());
        HandleHp(bossFsm.GetPerHp());
        bossHp_Text.text = bossFsm.GetCurrentHp().ToString() + " / " + bossFsm.GetMaxHp().ToString();
        bossName_Text.text = bossFsm.GetBossName();
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
