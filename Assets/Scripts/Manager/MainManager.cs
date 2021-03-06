using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{

    public static MainManager instance;


    private UIManager m_UIManager;
    private StageManager m_StageManager;
    private AnimationManager m_Animanager;


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



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
