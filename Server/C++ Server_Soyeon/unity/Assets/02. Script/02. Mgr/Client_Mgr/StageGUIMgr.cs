using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class StageGUIMgr : Singleton<StageGUIMgr>
    {
        public enum CHAR_TYPE { NON = -1, ATTACK=0, SHIELD, SUPPORT }

        public Button[] m_ChatType_Btn = new Button[3];
        public Button m_StartAndReady_Btn;
        public Button m_MapSelect_Btn;
        [SerializeField] private GameObject Map;

        [HideInInspector] public int m_selected_charType = new int();
        [HideInInspector] public List<int> m_selected_charTypes = new List<int>();

        void Start()
        {
            m_ChatType_Btn[(int)CHAR_TYPE.ATTACK].onClick.AddListener(() => OnClick_CharType((int)CHAR_TYPE.ATTACK));
            m_ChatType_Btn[(int)CHAR_TYPE.SHIELD].onClick.AddListener(() => OnClick_CharType((int)CHAR_TYPE.SHIELD));
            m_ChatType_Btn[(int)CHAR_TYPE.SUPPORT].onClick.AddListener(() => OnClick_CharType((int)CHAR_TYPE.SUPPORT));
            m_StartAndReady_Btn.onClick.AddListener(OnClick_StartAndReady);
            m_MapSelect_Btn.onClick.AddListener(OnClick_MapSelect);
        }

        #region button click event
        public void OnClick_MapSelect()
        {
            Map.gameObject.SetActive(true);
        }
        void OnClick_CharType(int _type)
        {
            // 해당 캐릭터를 선택
            m_selected_charType = _type;

            // 해당 charTypeBtn을 비활성화시키고
            // 다른 charTypeBtn을 활성화한다.
            m_ChatType_Btn[_type].interactable = false;

            switch (_type)
            {
                case (int)CHAR_TYPE.ATTACK:
                    Interactable_Btn((int)CHAR_TYPE.SHIELD, true);
                    Interactable_Btn((int)CHAR_TYPE.SUPPORT, true);
                    break;
                case (int)CHAR_TYPE.SHIELD:
                    Interactable_Btn((int)CHAR_TYPE.ATTACK, true);
                    Interactable_Btn((int)CHAR_TYPE.SUPPORT, true);
                    break;
                case (int)CHAR_TYPE.SUPPORT:
                    Interactable_Btn((int)CHAR_TYPE.ATTACK, true);
                    Interactable_Btn((int)CHAR_TYPE.SHIELD, true);
                    break;
            }

            // 이미 선택된 charType은 비활성화한다.
            foreach (int type in m_selected_charTypes)
            {
                Interactable_Btn(type, false);
            }
        }
        void OnClick_StartAndReady()
        {
            string text = m_StartAndReady_Btn.transform.Find("text").GetComponent<Text>().text;

            switch (text)
            {
                case "ready":
                    // 선택되지 않은 경우 무시한다.
                    if (m_selected_charType == (int)CHAR_TYPE.NON)
                        return;
                    // 준비를 누르면 비활성화 시킨다.
                    m_StartAndReady_Btn.interactable = false;
                    StageMgr.Instance.Send_Ready();
                    break;
                case "start":
                    StageMgr.Instance.Send_Start();
                    break;
            }
        }
        #endregion

        public void Interactable_Btn(int _type, bool _falg)
        {
            m_ChatType_Btn[_type].interactable = _falg;
        }
    }
}