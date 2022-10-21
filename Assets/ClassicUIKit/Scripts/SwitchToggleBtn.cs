using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ClassicUIKit
{ 
public class SwitchToggleBtn : MonoBehaviour {

        public Animator switchAnimator;
        public AnimationClip animClip_PressedOnToOff;
        public AnimationClip animClip_PressedOffTo0n;

        Toggle _thisToggle;


        public void OnSwitchButtonPush()
        {

            _thisToggle = GetComponent<Toggle>();
            Debug.Log("ifBefore= " + _thisToggle.isOn);

            //Move to False if Toggle isOn is True. Move to True if False.
            if (_thisToggle.isOn)
            {
                switchAnimator.Play(animClip_PressedOnToOff.name);
                return;

            }
            else 
            {
                switchAnimator.Play(animClip_PressedOffTo0n.name);
                return;
            }


        }



}
}
