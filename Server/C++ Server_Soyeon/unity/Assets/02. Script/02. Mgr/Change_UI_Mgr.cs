using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_client_unity
{
    public class Change_UI_Mgr : Singleton<Change_UI_Mgr>
    {
        public enum UI_TYPE
        {
            INTRO,
            LOGIN,
            JOIN,
            MENU,
            LOBBY,
            STAGE,
        };

        [System.Serializable]
        public class SerializeStateDic : test_client_unity.SerializableDictionary<UI_TYPE, GameObject> { }
        public SerializeStateDic m_state_dic = new SerializeStateDic();

        public void ChangeUI(UI_TYPE _pre, UI_TYPE _cur) // state 전환
        {
            m_state_dic.TryGetValue(_pre, out GameObject pre_obj);
            pre_obj.SetActive(false);
            m_state_dic.TryGetValue(_cur, out GameObject cur_obj);
            cur_obj.SetActive(true);
        }

        public void OnUI(UI_TYPE _obj) // state 켜주기
        {
            m_state_dic.TryGetValue(_obj, out GameObject obj);
            obj.SetActive(true);
        }

        public void OffUI(UI_TYPE _obj) // state 꺼주기
        {
            m_state_dic.TryGetValue(_obj, out GameObject obj);
            obj.SetActive(false);
        }

        public GameObject GetUI(UI_TYPE _type)
        {
            m_state_dic.TryGetValue(_type, out GameObject obj);
            return obj;
        }
    }
}