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
            MainManager.Instance.GetStageManager().SetPlayerRowAndCoulmn(m_row, m_coulmn);
            MainManager.Instance.GetStageManager().GetBossAndPlayerRowBuyColumn();

            coliderchk = true;
        }

        if(other.gameObject.CompareTag("Mob") && !MainManager.Instance.GetStageManager().m_MapInfo[m_row, m_coulmn].IsUnWalkable)
        {
            other.GetComponent<Enemy>().isDie = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MainManager.Instance.GetStageManager().SetPlayerRowAndCoulmn(m_row, m_coulmn);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            coliderchk = false;
        }
    }

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }
}
