using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ECharacterType = CharacterInfo.ECharacterType;
public class PlayerSlot : MonoBehaviour
{
    [ReadOnly]
    private PlayerInfo m_player;
    private int m_id;

    [SerializeField]
    private Transform m_parent;

    [SerializeField]
    private TMP_Text m_txt_ready;

    private Dictionary<ECharacterType, GameObject> m_CharacterObject;
    private GameObject m_curRender;
    static int m_slotcount = 0;
    #region property
    public PlayerInfo Player
    {
        get
        {
            if (m_player != null)
                return m_player;
            else return null;
        }
        set => m_player = value;
    }
    public int ID
    {
        get
        {
            if (m_player != null)
                return m_player.GetID;
            else return -1;
        }
    }
    #endregion
    public void Enable_ReadyText(bool _ready)
    {
        m_txt_ready.gameObject.SetActive(_ready);
    }
    public void Render()
    {
        if (m_curRender != null)
        {
            m_curRender.SetActive(false);
        }
        if (m_player.GetCharacterInfo.CharacterType != ECharacterType.None)
        {
            m_curRender = m_CharacterObject[m_player.GetCharacterInfo.CharacterType];
            m_curRender.SetActive(true);
        }

    }
    public void Render(ECharacterType _type)
    {
        if (m_curRender != null)
            m_curRender.SetActive(false);
        m_curRender = m_CharacterObject[_type];
        m_curRender.SetActive(true);
    }
    public void __Initialize()
    {
        m_CharacterObject = new Dictionary<ECharacterType, GameObject>();
        this.GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{m_slotcount++}");
        for (int i = 1; i < (int)ECharacterType.Max; i++)
        {
            GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>($"RenderTexture/TestObject{i - 1}"), m_parent);
            m_CharacterObject.Add((ECharacterType)i, obj);
            obj.SetActive(false);
        }
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
