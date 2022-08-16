using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageTxt : MonoBehaviour
{
    private Transform target;
    private float moveSpeed;                                    //데미지 텍스트 움직이는 속도
    private float alphaSpeed;                                   //알파값이 변경되는 속도
    private float destroyTime;                                  //텍스트가 사라지는 시간
    TextMeshPro text;                                           //텍스트메쉬 프로
    Color alpha;                                                //알파값
    public int damage;                                          //데미지

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 2.0f;
        alphaSpeed = 2.0f;
        destroyTime = 2.0f;

        text = GetComponent<TextMeshPro>();
        alpha = text.color;
        text.text = damage.ToString();
        Invoke("DestroyObject", destroyTime);                     //destroyTime 만큼 시간이 지난뒤 DestroyObject함수 실행
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(new Vector3(0, moveSpeed * Time.deltaTime, 0)); // 텍스트 위치
        alpha.a = Mathf.Lerp(alpha.a, 0, Time.deltaTime * alphaSpeed); ; // 텍스트 알파값
        text.color = alpha;
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

}
