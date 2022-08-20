using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ECharacterType = CharacterInfo.ECharacterType;

public class RoomGUIManager : Singleton_Ver2.Singleton<RoomGUIManager>
{
  

    public GameObject MapPannel;

    public bool is_Leader;

    public bool on_Ready;

    [SerializeField]
    private Button start_Btn;
    [SerializeField]
    private Button ready_Btn;

    [SerializeField]
    private ECharacterType select_Type;

    [SerializeField]
    private GameObject[] render_Char;
    [SerializeField]
    private GameObject parent_render_Char;

    [SerializeField]
    private Canvas m_canvas;

    #region ButtonClickEvent
    public void OnClick_Defence()
    {
        select_Type = ECharacterType.Defense;
        RoomManager.Instance.CharacterSelectProcess(select_Type);
    }
    public void OnClick_Attack()
    {
        select_Type = ECharacterType.Attack;
        RoomManager.Instance.CharacterSelectProcess(select_Type);
    }
    public void OnClick_Support()
    {
        select_Type = ECharacterType.Support;
        RoomManager.Instance.CharacterSelectProcess(select_Type);
    }
    public void OnClick_Map()
    {
        MapPannel.SetActive(true);
    }
    public void OnClick_Lobby()
    {
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Lobby, true);
        MenuGUIManager.Instance.WindowActive(MenuGUIManager.EWindowType.Room, false);
        LobbyGUIManager.Instance.ClearChat();
    }
    public void OnClick_Ready()
    {
        on_Ready = !on_Ready;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        if (is_Leader)
        {
            start_Btn.interactable = on_Ready;
        }
        
    }
    public void OnClick_Start()
    {
        if (on_Ready)
        {
            Debug.Log("GameStart");
            //전부다 Ready일때 시작, 3명의 캐릭터가 안겹칠때 시작?(이건 기획분께 물어보기)
            //메인씬으로 전환
        }
    }
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    #endregion
    private void Awake()
    {
        render_Char = new GameObject[3];
        for (int i = 0; i < render_Char.Length; i++)
        {
            render_Char[i] = Resources.Load<GameObject>($"RenderTexture/CharRenderPanel_{i}");
        }
    }
    private void Start()
    {
        
        MapPannel.SetActive(false);
        on_Ready = false;
        select_Type = ECharacterType.Attack;
        start_Btn.gameObject.SetActive(on_Ready);
        ready_Btn.gameObject.SetActive(!on_Ready);
        start_Btn.interactable = on_Ready;
        for(int i = 0; i < render_Char.Length; i++)
        {
            var tmpObj = Instantiate(render_Char[i]);
            tmpObj.transform.parent = parent_render_Char.transform;
        }
        
    }

    public void RenderCharImage(int _slotindex,ECharacterType _type)
    {
        switch (_type)
        {
            case ECharacterType.Defense:
                render_Char[_slotindex].transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{ECharacterType.Defense - 1}");
                break;

            case ECharacterType.Attack:
                render_Char[_slotindex].transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{ECharacterType.Attack - 1}");
                break;

            case ECharacterType.Support:
                render_Char[_slotindex].transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{ECharacterType.Support - 1}");
                break;
        }

    }
}
