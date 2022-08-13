using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStamina : MonoBehaviour
{
    [SerializeField]
    private Slider staminaBar;

    private BossFSM bossFsm;
    [SerializeField]
    private float perStamina;
    


    // Start is called before the first frame update
    void Start()
    {
        bossFsm = GameObject.Find("Boss").GetComponent<BossFSM>();
    }

    // Update is called once per frame
    void Update()
    {
        //perStamina = bossFsm.perStamina;
        HandleStamina();
    }

    void HandleStamina()
    {
        staminaBar.value = perStamina;
    }
}
