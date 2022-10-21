using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClassicUIKit
{
    public class DialogAnimator : MonoBehaviour
    {

        public AudioClip clip_WindowPop;
        public AudioClip clip_WindowClose;
        AudioSource _animAudiosource;

        private void Start()
        {
            _animAudiosource = GetComponent<AudioSource>();
        }

        //Play window audio
        public void PlayWindowAudio(string timing)
        {
            //Pop audio if the window is open. Close audio if the window is open.
            if (timing == "Pop")
            {
                _animAudiosource.clip = clip_WindowPop;
            }
            else if(timing=="Close")
            {
                _animAudiosource.clip = clip_WindowClose;
            }
            else
            {
                Debug.Log("No hit string of argument");
            }

            _animAudiosource.Play();
        }






    }
}
