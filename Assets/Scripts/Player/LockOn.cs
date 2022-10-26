using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOn : MonoBehaviour
{

    public bool m_lockOn=false;
    public GameObject target;
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        if(m_lockOn)
        {
            this.transform.LookAt(target.transform);      // 목표의 방향 설정.
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (m_lockOn)
        {
            //ballrigid.velocity = transform.forward * ballVelocity;
            //var ballTargetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.8f) - transform.position);
            //ballrigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, ballTargetRotation, turn));

            if (target == null ||this.transform.position == target.transform.position)
                Destroy(this.gameObject);
            else
            {
                this.transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed);
            }

           

           

        }
        

    }
}
