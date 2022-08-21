using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class JoinGUIMgr : MonoBehaviour
    {
        [SerializeField]
        TMP_InputField m_input_nick;
        [SerializeField]
        TMP_InputField m_input_id;
        [SerializeField]
        TMP_InputField m_input_pw;

        [SerializeField]
        Button m_btn_join;
        [SerializeField]
        Button m_btn_close;

        void Start()
        {
            m_btn_join.onClick.AddListener(OnClick_Join);
            m_btn_close.onClick.AddListener(OnClick_Close);
        }

        #region button click event
        public void OnClick_Join()
        {
            LoginMgr.Instance.JoinProcess(GetInfo());
            m_input_id.text = "";
            m_input_pw.text = "";
            m_input_nick.text = "";
        }
        public void OnClick_Close()
        {
            Change_UI_Mgr.Instance.OffUI(Change_UI_Mgr.UI_TYPE.JOIN);
        }
        #endregion

        #region Input func
        private LoginInfo GetInfo()
        {
            return new LoginInfo(m_input_id.text, m_input_pw.text, m_input_nick.text);
        }
        #endregion
    }
}
