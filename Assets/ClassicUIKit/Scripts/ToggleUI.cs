using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClassicUIKit
{

    public class ToggleUI : MonoBehaviour {

        public AudioClip audioClip;
        public AudioSource audioSource;
        public Toggle toggle;


        public void ToggleisOnPlaySoundEffect()
        {
            //Sound the audio if Toddle isOn is True.
            if (toggle.isOn)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }



    }
}
