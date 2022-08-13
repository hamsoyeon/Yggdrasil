using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager: MonoBehaviour
{
    //사용범 : 타 스크립트에서 개체 참조 없이 AnimationManager.GetInstance()로 접근해서
    //PlayAnimation() 함수를 실행합니다. 
    //애니메이션을 재생 시킬 모델에 있는 Animator와 그 state 이름을 매개변수에 기입합니다.
    //스킬 사용시 출력하는 애니메이션 등 간단한 사용에 적합하며
    //플레이어 이동, 죽음 등 상태와 긴밀하게 연결된 애니메이션 사용엔 부적합 합니다.

    private static AnimationManager instance;

    public static AnimationManager GetInstance()
    {
        if(instance == null)
        {
            instance = FindObjectOfType<AnimationManager>();
            if(instance == null)
            {
                GameObject container = new GameObject("AnimationManager");
                instance = container.AddComponent<AnimationManager>();
            }
        }
        return instance;
    }

    public void PlayAnimation(Animator _ani, string name)
    {
        _ani.Play(name);
    }
    public void PlayAnimation(Animator _ani, string name, float speed)
    {
        _ani.speed = speed;
        _ani.Play(name);
    }



















    public void ChangeAnimationState(Animator animator, string newState, string currentState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }






}
