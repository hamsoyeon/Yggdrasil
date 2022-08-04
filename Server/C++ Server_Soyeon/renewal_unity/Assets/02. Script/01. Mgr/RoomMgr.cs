using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace test_client_unity
{
    public class RoomMgr : Singleton<RoomMgr>
    {
        public List<GameObject> m_roomList = new List<GameObject>();
        public int m_curPageNum = new int();

        void Start()
        {
            m_curPageNum = 0;

            //UpdateRoom("test", 1, 5); // test��
            //UpdateRoom("test", 2, 5); 
            //UpdateRoom("test", 2, 6);

            //CreateRoom("test", 2, 5);
            //CreateRoom("test", 3, 5);
            //CreateRoom("test", 4, 5);
            //CreateRoom("test", 5, 5);
            //CreateRoom("test", 6, 5);
            //CreateRoom("test", 7, 5);
            //CreateRoom("test", 8, 5);
            //CreateRoom("test", 9, 5);

            //CreateRoom("test", 10, 5);
            //CreateRoom("test", 11, 5);

            //UpdatePage();
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

        public void UpdatePage() // ���� ������ 
        {
            for (int i = 0; i < m_roomList.Count / 9; i++)
            {
                GameObject page_obj = GameObject.Find("page_" + m_roomList.Count / 9);
                // ���� ������ object�� Ȱ��ȭ�ϰ�,
                // ���� �������� �ƴ� object�� ��Ȱ��ȭ.
                if (!page_obj.name.Contains(m_curPageNum.ToString()))
                {
                    page_obj.SetActive(false);
                }
            }
        }

        public void UpdateRoom(string _roomName, int _roomNum, int _roomCnt = 0)
        {
            GameObject room_obj = GameObject.Find("room_" + _roomNum.ToString());

            if (room_obj == null)
            {
                CreateRoom(_roomName, _roomNum, _roomCnt);
            }
            else // �̹� �ִ� ���
            {
                room_obj.transform.GetComponent<Room>().SetRoom(_roomName, _roomNum, _roomCnt); // update
            }
        }

        public void CreateRoom(string _roomName, int _roomNum, int _roomCnt = 0) // ���� �����ϴ� ��� �� ����
        {
            Debug.Log("Standalone Windows");

            // unity error : Setting the parent of a transform which resides in a Prefab Asset is disabled to prevent data corruption
            // �ڵ� �󿡼� �������� �ҷ����� parent�� ������, �ν��Ͻ�ȭ�� obj�� ������ �޾Ƽ� �����Ѵ�.
            GameObject room = Resources.Load("Prefab/room") as GameObject;
            //GameObject room_inst = PrefabUtility.InstantiatePrefab(room) as GameObject;
            GameObject room_inst = Instantiate(room) as GameObject;
            
            room_inst.transform.GetComponent<Room>().SetRoom(_roomName, _roomNum, _roomCnt);
            room_inst.name = "room_" + _roomNum.ToString();

            GameObject page_obj;
            page_obj = GameObject.Find("page_" + m_roomList.Count / 9);

            if (page_obj == null) // page_ obj�� ���ٸ� ����
            {
                page_obj = Resources.Load("Prefab/page") as GameObject;
                GameObject page_inst = Instantiate(page_obj) as GameObject; // ������ ������Ʈ�� �ҷ�����.
                
                // ������ ����
                int page_num = m_roomList.Count / 9; // ������ �ѹ�
                page_inst.transform.parent = GameObject.Find("room panel").transform;
                page_inst.name = "page_" + page_num.ToString();
                page_inst.transform.localPosition = new Vector3(0f, 0f, 0f);
                // �ش� �������� ���� �ִ´�. 
                room_inst.transform.parent = page_inst.transform;
            }
            else
            {
                // Lobby window ������Ʈ�� ã�Ƽ� ������ ���� ������ room ������Ʈ�� ���δ�. 
                room_inst.transform.parent = page_obj.transform;
            }

            m_roomList.Add(room_inst);
        }
    }
}

