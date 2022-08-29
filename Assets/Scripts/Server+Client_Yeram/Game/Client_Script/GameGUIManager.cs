using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject TestObject;
    [SerializeField]
    GameObject TestObject2;


    #region click event
    public void Click_ViewSector()
    {
        _GameManager.Instance.TestViewSector(TestObject.transform.position);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
