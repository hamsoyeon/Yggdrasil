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
        public Text roomNameAndNum;
        public Text roomCnt;
        [HideInInspector] public int roomNum;

        public Button EnterBtn;

        public string m_passWord { get; set; }

        public void SetRoom(string _roomName, string _passWord, int _roomNum, int _roomCnt = 0)
        {
            // 값세팅
            roomNameAndNum.text = _roomNum.ToString() + ". " + _roomName;
            m_passWord = _passWord;
            roomNum = _roomNum;
            roomCnt.text = _roomCnt.ToString() + " / 3";

            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            // 현재 obj의 child를 탐색
            // - 버튼
            // - 방넘버
            // - 현재 방인원수
            EnterBtn.onClick.AddListener(Click_EnterStageBtn);
        }

        public void Click_EnterStageBtn() // 방들어가기
        {
            // 비밀번호가 맞는 지 검수
            Debug.Log(m_passWord);
            GameObject.Find("Lobby").transform.Find("enter room panel").gameObject.SetActive(true);
            GameObject.Find("enter room panel").GetComponent<EnterRoomPanel>().m_selectedRoomNum = roomNum;
            GameObject.Find("enter room panel").GetComponent<EnterRoomPanel>().m_selectedRoomPassword = m_passWord;
        }
    }
}

