using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    //외부에서 입력이 가능하게 메소드 생성
    [Header("Run Speed")]
    [SerializeField]
    private float runSpeed;

    [Header("ATK")]
    [SerializeField]
    private float Attack;

    [Header("HP")]
    [SerializeField]
    private int maxHP = 100;
    private int currentHP;

    public float RunSpeed => runSpeed;

    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    //데미지를 받았을 때 호출 함수
    public bool DecreaseHP(int damage)
    {
        int previousHP = currentHP;

        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(previousHP, currentHP);

        if(currentHP <= 0)
        {
            return true;
        }

        return false;
    }
}
