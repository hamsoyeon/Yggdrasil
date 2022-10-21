using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClassicUIKit
{
    public class DialogWindow : MonoBehaviour
    {
        public Animator popAnimator;
        public AnimationClip popAnimationclip;


        //Play animation when activated
        void OnEnable()
        {
            popAnimator.Play(popAnimationclip.name);
        }


    }
}