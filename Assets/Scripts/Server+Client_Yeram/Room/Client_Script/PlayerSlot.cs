using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ECharacterType = CharacterInfo.ECharacterType;
public class PlayerSlot : MonoBehaviour
{
    [ReadOnly]
    private int m_id;

    [SerializeField]
    private Transform m_parent;

    private Dictionary<ECharacterType, GameObject> m_CharacterObject;
    private GameObject m_curRender;
    static int m_slotcount = 0;
    #region property
    public int ID
    {
        get => m_id;
        set => m_id = value;
    }
    #endregion
    public void Render(ECharacterType _type)
    {
        if(m_curRender!=null)
        m_curRender.SetActive(false);
        m_curRender = m_CharacterObject[_type];
        m_curRender.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_CharacterObject = new Dictionary<ECharacterType, GameObject>();
        this.GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{m_slotcount++}");
        for (int i = 1; i < (int)ECharacterType.Max;i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>($"RenderTexture/TestObject{i - 1}"), m_parent);
            m_CharacterObject.Add((ECharacterType)i,obj);
            obj.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
