using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public int[] m_userSelectSkillIndex;
    public int m_userSelectStage;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static DataManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        m_userSelectSkillIndex = new int[6];  // 초기값 0으로 셋팅됨
        m_userSelectStage = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
