using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class LobbyGUIMgr : MonoBehaviour
    {
        [SerializeField]
        Button m_btn_make_room;
        [SerializeField]
        Button m_btn_enter_mainmenu;
        [SerializeField]
        Button m_btn_option;

        void Start()
        {
            m_btn_make_room.onClick.AddListener(OnClick_MakeRoom);
            m_btn_enter_mainmenu.onClick.AddListener(OnClick_MainMenu);
            m_btn_option.onClick.AddListener(OnClick_Option);
        }

        #region button click event
        void OnClick_Option()
        {
            Change_UI_Mgr.Instance.m_state_dic.TryGetValue(
                Change_UI_Mgr.UI_TYPE.LOBBY,
                out GameObject obj);
            obj.transform.Find("Option").gameObject.SetActive(true);
        }
        public void OnClick_MakeRoom()
        {
            GameObject.Find("Lobby").transform.Find("make room panel").gameObject.SetActive(true);
        }
        public void OnClick_MainMenu()
        {
            Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.LOBBY, Change_UI_Mgr.UI_TYPE.MENU);
            LobbyMgr.Instance.Enter_MainMenu();
        }
        #endregion
    }
}