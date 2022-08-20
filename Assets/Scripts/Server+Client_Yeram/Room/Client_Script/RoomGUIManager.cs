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
    
    private int[] test_slot = new int[3];

    #region ButtonClickEvent
    public void OnClick_Defence()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Defense);
    }
    public void OnClick_Attack()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Attack);
    }
    public void OnClick_Support()
    {
        RoomManager.Instance.CharacterSelectProcess(ECharacterType.Support);
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
        RoomManager.Instance.ReadyProcess(on_Ready);
    }
    public void OnClick_Start()
    {
        Debug.Log("GameStart");
        //메인씬으로 전환
    }
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    #endregion
    public void EnableStartBtn(bool _allready)
    {
        if (_allready)
        {
            start_Btn.interactable = true;
            //레디버튼 비활성화
            //스타트버튼 활성화
        }
        else
        {
            start_Btn.interactable = false;
        }
    }
    public void SettingSlotInfo(int _playerid, int _another1_id, int _another2_id)
    {
        test_slot[0] = _another1_id;
        test_slot[1] = _playerid;
        test_slot[2] = _another2_id;
    }
    private void Awake()
    {
        render_Char = new GameObject[3];
        for(int i=0;i<render_Char.Length;i++)
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

        for(int i=0;i<render_Char.Length;i++)
        {
            var tempobj = Instantiate(render_Char[i]);
            tempobj.transform.parent = parent_render_Char.transform;
        }
    }

    public void RenderCharImage(int _slotindex, ECharacterType _type)
    {
        switch (_type)
        {

            case ECharacterType.Attack:
                render_Char[_slotindex].transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{ECharacterType.Defense - 1}");
                break;

            case ECharacterType.Defense:
                render_Char[_slotindex].transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{ECharacterType.Attack - 1}");

                break;

            case ECharacterType.Support:
                render_Char[_slotindex].transform.GetChild(0).GetComponent<RawImage>().texture = Resources.Load<RenderTexture>($"RenderTexture/CharRenderTexture_{ECharacterType.Support - 1}");

                break;
        }

    }
}
