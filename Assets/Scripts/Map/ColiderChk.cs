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
            coliderchk = true;
            //mat.color = new Color(1, 0, 0);

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
