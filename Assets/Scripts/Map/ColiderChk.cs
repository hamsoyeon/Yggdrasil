using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderChk : MonoBehaviour
{
    public bool coliderchk = false;
    MeshRenderer mesh;
    //Material mat;

    public int m_row;
    public int m_coulmn;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.CompareTag("Player"))
        {

            Debug.Log($"Player (row:{m_row} / coulmn:{m_coulmn})");
               
            MainManager.Instance.GetStageManager().SetPlayerRowAndCoulmn(m_row, m_coulmn);

            MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.PlayerPos = other.transform.position;

            Debug.Log($"이동한 타일의 플레이어 포지션 { MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.PlayerPos}");
            coliderchk = true;
            //mat.color = new Color(1, 0, 0);
        }

        //보스는 타일 이동을 하기에 타일을 들어간 순간의 row column값을 받아와서 그 위치 값만 한번씩 던져준다.
        if(other.gameObject.CompareTag("Boss"))
        {
            Debug.Log($"Boss (row : {m_row} / column : {m_coulmn})");

            MainManager.Instance.GetStageManager().SetBossRowAndCoulmn(m_row, m_coulmn);
            MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.bossPos = other.transform.position;
            Debug.Log($"이동한 타일의 보스 포지션 { MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.bossPos}");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        //플레이어는 타일의 영향을 안받기에 계속적으로 포지션값을 보내준다.
        if (other.gameObject.CompareTag("Player"))
        {
            MainManager.Instance.GetStageManager().SetPlayerRowAndCoulmn(m_row, m_coulmn);

            MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.PlayerPos = other.transform.position;

            //Debug.Log($"머물고 있는 타일의 플레이어 포지션 { MainManager.Instance.GetStageManager().m_GetWorldPosByObjects.PlayerPos}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coliderchk = false;
            //mat.color = new Color(1, 1, 1);
        }
    }



    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        //mat = mesh.material;
    }

    void Update()
    {

    }
}
