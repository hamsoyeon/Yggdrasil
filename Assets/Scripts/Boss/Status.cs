using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

public class Status : MonoBehaviour
{
    private SubMonster_TableExcel m_CurrentSubMonsterStat;
    public int m_CurrentIndex;

    public CharacterClass SubMonsterClass;

    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    //외부에서 입력이 가능하게 메소드 생성
    [Header("Run Speed")]
    [SerializeField]
    private float runSpeed;

    [Header("ATK")]
    [SerializeField]
    private float Atk;

    [Header("DEF")]
    [SerializeField]
    private float Def;

    [Header("HP")]
    [SerializeField]
    private int maxHP;
    private int currentHP;

    public float RunSpeed => runSpeed;
    public int CurrentHP => currentHP;
    public int MaxHP => maxHP;
    public float ATTACK => Atk;

    void Awake()
    {
        if (SceneManager.GetActiveScene().name == "MainScene")
            m_CurrentIndex = 141001;
        if (SceneManager.GetActiveScene().name == "Stage2")
            m_CurrentIndex = 141002;


        SubMonsterClass = this.gameObject.GetComponent<CharacterClass>();

        foreach (var item in DataTableManager.Instance.GetDataTable<SubMonster_TableExcelLoader>().DataList)
        {
            if (item.SubMonsterIndex == m_CurrentIndex)
            {
                SubMonsterClass.m_SubMonsterData = item;
                break;
            }
        }

        currentHP = SubMonsterClass.m_SubMonsterData.HP;
        runSpeed = SubMonsterClass.m_SubMonsterData.Speed;
        Atk = SubMonsterClass.m_SubMonsterData.Demege;
        Def = SubMonsterClass.m_SubMonsterData.Defense;

        maxHP = currentHP;
    }

    //데미지를 받았을 때 호출 함수
    public bool DecreaseHP(int damage)
    {
        int previousHP = currentHP;

        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(previousHP, currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("체력 0");
            return true;
        }

        return false;
    }
}
