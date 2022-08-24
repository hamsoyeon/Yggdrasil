using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStamina : MonoBehaviour
{
    [SerializeField]
    private Slider staminaBar;
    [SerializeField]
    private BossFSM bossFsm;
    
    [SerializeField]
    private GameObject p_Slider;

    // Start is called before the first frame update
    private void Awake()
    {
        p_Slider = Resources.Load<GameObject>("StaminaCanvas");
        bossFsm = GetComponent<BossFSM>();
    }
    void Start()
    {
        Instantiate(p_Slider, gameObject.transform.position + Vector3.up * 20, Quaternion.identity).transform.parent = this.gameObject.transform;
        staminaBar = gameObject.transform.GetChild(5).GetChild(0).GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(staminaBar.value);
        Debug.Log(bossFsm.GetPerStamina());
        HandleStamina(bossFsm.GetPerStamina());
    }

    void HandleStamina(float _stamina)
    {
        staminaBar.value = _stamina;
    }
}
