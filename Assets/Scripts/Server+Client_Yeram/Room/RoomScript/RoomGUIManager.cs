using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGUIManager : Singleton_Ver2.Singleton<RoomGUIManager>
{
    public GameObject MapPannel;

    #region ButtonClickEvent
    public void OnClick_Attack()
    {

    }
    public void OnClick_Defence()
    {

    }
    public void OnClick_Support()
    {

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
    public void OnClick_ClosedMap()
    {
        MapPannel.SetActive(false);
    }
    #endregion

    private void Start()
    {
        MapPannel.SetActive(false);
    }
}
