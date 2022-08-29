using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirection : MonoBehaviour
{
    //증가하는 방향값으로 바라봄
    //이거 쓰지 마세요 안 돼서 플레이어 매니저에 아예 넣어버림
    public float rotateSpeed = 10.0f;
    float h, v;

    private static PlayerDirection instance;

    public static PlayerDirection GetInstance()
    {
        if (instance == null)
        {
            instance = FindObjectOfType<PlayerDirection>();
            if (instance == null)
            {
                GameObject container = new GameObject("PlayerDirection");
                instance = container.AddComponent<PlayerDirection>();
            }
        }
        return instance;
    }

    public void ChangePlayerDirection(GameObject obj)
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            obj.transform.rotation = Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * rotateSpeed);
        }
    }

    private void FixedUpdate()
    {
        
    }
}
