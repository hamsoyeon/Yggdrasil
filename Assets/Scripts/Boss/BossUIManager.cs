using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    [SerializeField]
    private BossFSM bossFsm;
    public GameObject minimap_2DIcon_Boss;
    public Slider bossHp_Slider;
    public Slider bossStamina_Slider;
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
