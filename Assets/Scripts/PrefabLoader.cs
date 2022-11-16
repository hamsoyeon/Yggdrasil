using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{

    public static PrefabLoader instance;

    [SerializeField]
    private List<GameObject> PrefabList;
    public Dictionary<int, GameObject> PrefabDic;


    //public Sprite[] m_pSelectSkill;

    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            PrefabDic = new Dictionary<int, GameObject>();
            //m_pSelectSkill = new Sprite[6];

            DontDestroyOnLoad(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public static PrefabLoader Instance
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

    void Start()
    {
        foreach(var item in PrefabList)
        {
            PrefabDic.Add(int.Parse(item.name), item);
        }
    }

    void Update()
    {
        
    }
}
