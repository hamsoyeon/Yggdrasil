using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace test_client_unity
{
    public class LoginGUIMgr : Singleton<LoginGUIMgr>
    {
        [SerializeField]
        TMP_InputField m_input_id;
        [SerializeField]
        TMP_InputField m_input_pw;

        [SerializeField]
        Button m_btn_enter_join;
        [SerializeField]
        Button m_btn_login;

        void Start()
        {
            // 이벤트 등록
            m_btn_enter_join.onClick.AddListener(OnClick_EnterJoin);
            m_btn_login.onClick.AddListener(OnClick_Login);
        }

        #region button click event
        public void OnClick_EnterJoin()
        {
            Change_UI_Mgr.Instance.ChangeUI(Change_UI_Mgr.UI_TYPE.LOGIN, Change_UI_Mgr.UI_TYPE.JOIN);
        }
        public void OnClick_Login()
        {
            LoginMgr.Instance.LoginProcess(GetInfo());
            m_input_id.text = "";
            m_input_pw.text = "";
        }
        #endregion

        #region Input func
        private LoginInfo GetInfo()
        {
            return new LoginInfo(m_input_id.text, m_input_pw.text, string.Empty);
        }
        #endregion
    }
}