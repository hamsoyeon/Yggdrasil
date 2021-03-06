using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EWindowType = MenuGUIManager.EWindowType;
public class LoginGUIManager : Singleton_Ver2.Singleton<LoginGUIManager>
{

    [SerializeField]
    InputField[] m_input_id;
    [SerializeField]
    InputField[] m_input_pw;
    [SerializeField]
    InputField m_input_nick;

    #region button click event
    public void OnClick_Exit(bool _flag)
    {
        Close(_flag);
    }
    public void OnClick_OK(bool _flag)
    {
        if (_flag)//로그인
        {
            LoginManager.Instance.LoginProcess(GetInfo(_flag));
        }
        else//회원가입
        {
            LoginManager.Instance.JoinProcess(GetInfo(_flag));
        }

    }
    #endregion
    #region window close func
    private void Close(bool _flag)
    {
        if (_flag)
            MenuGUIManager.Instance.WindowActive(EWindowType.Login, false);
        else
            MenuGUIManager.Instance.WindowActive(EWindowType.Join, false);

        MenuGUIManager.Instance.WindowActive(EWindowType.Menu, true);
    }
    #endregion
    #region Input func
    private LoginInfo GetInfo(bool _flag)
    {
        if (_flag)
            return new LoginInfo(m_input_id[0].text, m_input_pw[0].text, string.Empty);

        return new LoginInfo(m_input_id[1].text, m_input_pw[1].text, m_input_nick.text);
    }
    #endregion
}
