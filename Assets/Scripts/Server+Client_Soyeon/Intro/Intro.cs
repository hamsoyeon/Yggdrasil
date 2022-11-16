using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace test_client_unity
{
    public class Intro : MonoBehaviour, IPointerClickHandler
    {
        // 화면 클릭시 로그인창이 팝업
        public void OnPointerClick(PointerEventData eventData)
        {
            if (this.transform.Find("login panel").gameObject.active == true)
                return;

            WindowMgr.Instance.m_state_dic.TryGetValue(WindowMgr.WINDOW_TYPE.INTRO, out GameObject intro);
            intro.transform.Find("login panel").gameObject.SetActive(true);
        }
    }
}

