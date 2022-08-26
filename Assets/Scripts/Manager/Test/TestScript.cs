using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    //1. 오브젝트(모델을)개체참조(에디터에서붙이기)해서 가져오기
    //2. 스크립트를 가진 모델을 스크립트컴포넌트 참조로 가져오기
    //3. 아무것도 가지지 않은 오브젝트를 가져오기


    //오브젝트(모델) 가져오기
    public GameObject obj;
    public GameObject fireDemon;
    public GameObject player;
    Animator anim;

    //오브젝트의 애니메이터
    private void Awake()
    {
        anim = obj.GetComponentInChildren<Animator>();
        fireDemon = GameObject.Find("Fire Demon-Orange");
        player = GameObject.Find("FallenAngel02");
        //awake 밖에서 하면 오류난다. 유니티 문제는 아니고 C# 문법
    }
    private void FixedUpdate()
    {
        MonsterDirection(fireDemon, player);

    }
    public float rotateSpeed = 10.0f;
    void MonsterDirection(GameObject obj)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation,
            Quaternion.LookRotation(obj.transform.position),
            Time.deltaTime * rotateSpeed);
    }
    void MonsterDirection(GameObject obj, GameObject plr)
    {
        obj.transform.rotation = Quaternion.Lerp(obj.transform.rotation,
            Quaternion.LookRotation(plr.transform.position),
            Time.deltaTime * rotateSpeed);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AnimationManager.GetInstance().PlayAnimation(anim, "Skill01");
        }

    }


}
