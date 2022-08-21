using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class RoomMgr : Singleton<RoomMgr>
    {
        public List<GameObject> m_roomList = new List<GameObject>();
        public int m_curPageNum = new int();

        public Button m_Leftbutton;

        void Start()
        {
            m_curPageNum = 0;

            for (int i = 0; i < 12; i++)
            {
                CreateRoom("", "", i);
                m_roomList[i].SetActive(false);
            }
        }

        public void Click_LeftBtn()
        {
            if (m_curPageNum == 0)
            {
                return;
            }

            m_curPageNum--;

            Debug.Log(m_curPageNum);
        }

        public void Click_RightBtn()
        {
            if (m_curPageNum == m_roomList.Count / 9)
            {
                return;
            }

            m_curPageNum++;

            Debug.Log(m_curPageNum);
        }

        public void UpdatePage() // 현재 페이지 
        {
            for (int i = 0; i < m_roomList.Count / 9; i++)
            {
                GameObject page_obj = GameObject.Find("page_" + m_roomList.Count / 9);
                // 현재 페이지 object만 활성화하고,
                // 현재 페이지가 아닌 object는 비활성화.
                if (!page_obj.name.Contains(m_curPageNum.ToString()))
                {
                    page_obj.SetActive(false);
                }
            }
        }

        public void UpdateRoom(string _roomName, string _passWord, int _roomNum, int _roomCnt = 0)
        {
            GameObject room_obj = GameObject.Find("room_" + _roomNum.ToString());

            if (room_obj == null)
            {
                CreateRoom(_roomName, _passWord, _roomNum, _roomCnt);
            }
            else // 이미 있는 경우
            {
                room_obj.transform.GetComponent<Room>().SetRoom(_roomName, _passWord, _roomNum, _roomCnt); // update
            }
        }

        public void AtiveRoom(string _roomName, string _passWord, int _roomNum, int _roomCnt = 0)
        {
            foreach (GameObject obj in m_roomList)
            {
                if (obj.transform.GetComponent<Room>().roomNum == _roomNum)
                {
                    obj.transform.GetComponent<Room>().SetRoom(_roomName, _passWord, _roomNum, _roomCnt);
                    obj.SetActive(true);
                    break;
                }
            }
        }

        public void CreateRoom(string _roomName, string _passWord, int _roomNum, int _roomCnt = 0) // 현재 존재하는 모든 방 생성
        {
            // unity error : Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption
            // 코드 상에서 프리팹을 불러오고 parent를 수정시, 인스턴스화된 obj를 변수로 받아서 세팅한다.
            GameObject room = Resources.Load("Prefab/room") as GameObject;
            //GameObject room_inst = PrefabUtility.InstantiatePrefab(room) as GameObject;
            GameObject room_inst = Instantiate(room) as GameObject;

            room_inst.transform.GetComponent<Room>().SetRoom(_roomName, _passWord, _roomNum, _roomCnt);
            room_inst.name = "room_" + _roomNum.ToString();

            GameObject page_obj;
            page_obj = GameObject.Find("page_" + m_roomList.Count / (10 + 1));

            if (page_obj == null) // page_ obj가 없다면 생성
            {
                page_obj = Resources.Load("Prefab/page") as GameObject;
                GameObject page_inst = Instantiate(page_obj) as GameObject; // 프리팹 오브젝트로 불러오기.

                // 페이지 생성
                int page_num = m_roomList.Count / (10 + 1); // 페이지 넘버
                page_inst.transform.parent = GameObject.Find("room panel").transform;
                page_inst.name = "page_" + page_num.ToString();
                page_inst.transform.localPosition = new Vector3(0f, 0f, 0f);
                // 해당 페이지에 방을 넣는다. 
                room_inst.transform.parent = page_inst.transform;
            }
            else
            {
                // Lobby window 오브젝트를 찾아서 하위에 현재 생성할 room 오브젝트를 붙인다. 
                room_inst.transform.parent = page_obj.transform;
            }

            m_roomList.Add(room_inst);
        }
    }
}

