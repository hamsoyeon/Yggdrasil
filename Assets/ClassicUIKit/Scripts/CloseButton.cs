using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ClassicUIKit
{ 

public class CloseButton : MonoBehaviour {

        public GameObject closeObject;
        public Animator closeAnimator;
        public AnimationClip closeAnimationClip;

    public void GetCloseAnimation()
        {
          
           //Play close animation
            closeAnimator.Play(closeAnimationClip.name);

            //Close the dialog after the animation is over
            Invoke("DialogClose", closeAnimationClip.length);
           
               
        }

        //Close the dialog
        void DialogClose()
        {
            closeObject.SetActive(false);

        }



    }

}