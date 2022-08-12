using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager: MonoBehaviour
{
    
    //Animator ani;
    //AnimatorController  

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




















    public void ChangeAnimationState(Animator animator, string newState, string currentState)
    {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }






}
