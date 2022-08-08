using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public PlayerManager playerManager;
    public GameObject UICanvas;
    public Slider hpBar;

    void Start()
    {
        UICanvas = GameObject.Find("UICanvas");
        hpBar = UICanvas.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {

        hpBar.value = playerManager.p_PerHp;
    }
}
