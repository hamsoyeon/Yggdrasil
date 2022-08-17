using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class Room : MonoBehaviour
    {
        public string m_roomName { get; set; }
        public int m_roomNum { get; set; }
        public int m_roomCnt { get; set; }

        public string m_passWord { get; set; }

        public void SetRoom(string _roomName, string _passWord, int _roomNum, int _roomCnt = 0)
        {
            // 값세팅
            m_roomName = _roomName;
            m_passWord = _passWord;
            m_roomNum = _roomNum;
            m_roomCnt = m_roomCnt;

            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            // 현재 obj의 child를 탐색
            // - 버튼
            // - 방넘버
            // - 현재 방인원수
            foreach (Transform child in allchildren)
            {
                switch (child.gameObject.name)
                {
                    case "enter room button": // 방이름
                        child.GetComponent<Button>().onClick.AddListener(Click_EnterStageBtn);
                        child.GetComponentInChildren<Text>().text = _roomName;
                        break;
                    case "room num text": // 방넘버
                        child.GetComponent<Text>().text = _roomNum.ToString();
                        break;
                    case "room cnt text": // 방인원수
                        child.GetComponent<Text>().text = _roomCnt.ToString() + " / 3";
                        break;
                }
            }
        }

        public void Click_EnterStageBtn() // 방들어가기
        {
            // 비밀번호가 맞는 지 검수
            Debug.Log(m_passWord);
            GameObject.Find("lobby window").transform.Find("enter room panel").gameObject.SetActive(true);
            GameObject.Find("enter room panel").GetComponent<EnterRoomPanel>().m_selectedRoomNum = m_roomNum;
            GameObject.Find("enter room panel").GetComponent<EnterRoomPanel>().m_selectedRoomPassword = m_passWord;
        }

        // 이걸 매니저에다 두고... 흐음
        public int Packpacket(ref Byte[] _buf, int _protocol, int _data)
        {
            _buf = new Byte[4096];

            Byte[] data_buf = new Byte[4096];

            int len = 0;

            BitConverter.GetBytes(_data).CopyTo(data_buf, len);
            len = len + sizeof(int);

            return NetMgr.Instance.m_netWork.PackPacket(ref _buf, _protocol, data_buf, len);
        }
    }
}

