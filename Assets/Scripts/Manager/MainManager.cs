using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static MainManager instance;


    private UIManager m_UIManager;
    private StageManager m_StageManager;
    private AnimationManager m_Animanager;
    private BuffManager m_BuffManager;


    public int xp, zp;
    public int hexMapSizeX = 6;
    public int hexMapSizeZ = 5;

    //private ResourceManager m_Resource;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            m_UIManager = new UIManager();
            m_StageManager = new StageManager();
            m_Animanager = new AnimationManager();
            m_BuffManager = new BuffManager();
            //m_Resource = new ResourceManager();
          

            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }

    }


    public static MainManager Instance
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

    //public static ResourceManager Resource { get { return Instance.m_Resource; } }

  


    public BuffManager GetBuffManager()
    {
        return m_BuffManager;
    }

    public AnimationManager GetAnimanager()
    {
        return m_Animanager;
    }


    public UIManager GetUIManager()
    {
        return m_UIManager;
    }

    public StageManager GetStageManager()
    {
        return m_StageManager;
    }
}
