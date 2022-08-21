using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class MenuGUIMgr : MonoBehaviour
    {
        [SerializeField]
        Button m_btn_enter_single;
        [SerializeField]
        Button m_btn_enter_multi;
        [SerializeField]
        Button m_btn_enter_option;
        [SerializeField]
        Button m_btn_close;
        
        void Start()
        {
            m_btn_enter_single.onClick.AddListener(OnClick_EnterSingle);
            m_btn_enter_multi.onClick.AddListener(OnClick_EnterMulti);
            m_btn_enter_option.onClick.AddListener(OnClick_Option);
            m_btn_close.onClick.AddListener(OnClick_Close); // 게임종료
        }

        #region button click event
        public void OnClick_EnterSingle()
        {

        }
        public void OnClick_EnterMulti()
        {
            Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.MENU, Change_UI_Mgr.UI_TYPE.LOBBY);

            MenuMgr.Instance.Send_EnterMulti();
        }
        public void OnClick_Option()
        {

        }
        public void OnClick_Close()
        {

        }
        #endregion
    }
}
