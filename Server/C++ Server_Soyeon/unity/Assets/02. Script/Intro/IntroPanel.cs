using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace test_client_unity
{
    public class IntroPanel : MonoBehaviour, IPointerClickHandler
    {
        private void Awake()
        {
            DontDestroyOnLoad(GameObject.Find("Mgr"));
        }

        // 화면 클릭시 로그인창이 팝업
        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.transform.Find("Login").gameObject.active == true)
                return;

            Change_UI_Mgr.Instance.OnUI(Change_UI_Mgr.UI_TYPE.LOGIN);

            //if (this.transform.Find("login panel").gameObject.active == true)
            //    return;

            //Debug.Log("screen click");
            //Change_UI_Mgr.Instance.m_state_dic.TryGetValue(Change_UI_Mgr.UI_TYPE.INTRO, out GameObject intro);
            //intro.transform.Find("login panel").gameObject.SetActive(true);
        }
    }
}

