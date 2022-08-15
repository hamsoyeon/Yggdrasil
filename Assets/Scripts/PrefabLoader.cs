using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabLoader : MonoBehaviour
{

    public static PrefabLoader instance;

    [SerializeField]
    private List<GameObject> PrefabList;

    public Dictionary<int, GameObject> PrefabDic;


    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            PrefabDic = new Dictionary<int, GameObject>();

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

    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in PrefabList)
        {
            PrefabDic.Add(int.Parse(item.name), item);
        }


        //Debug.Log(PrefabDic[911001].name);
        

        //foreach (var item in PrefabDic)
        //{
        //    Debug.Log($"[{item.Key}:{item.Value}]");
        //}
       






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
