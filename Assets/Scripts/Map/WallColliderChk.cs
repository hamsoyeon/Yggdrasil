using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallColliderChk : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어가 벽에 부딪힘");
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().isWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerManager>().isWall = false;
        }
    }
}
