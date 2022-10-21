using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClassicUIKit
{
    public class ResultEffectManager : MonoBehaviour
    {
        public Animator animator_Stars00;
        public Animator animator_Stars01;
        public Animator animator_Stars02;

        public GameObject gameobj_Radiation;
        public AnimationClip starAnimClip00;
        public AnimationClip starAnimClip01;
        public AnimationClip starAnimClip02;
        
        float _nextEffectWaitTime = -0.1f;

        private void Start()
        {
            PlayAllEffectAnimation();
        }

        //Initialize
        void CleanAllAnimator()
        {
            CancelInvoke();
            animator_Stars00.Play("Stay");
            animator_Stars01.Play("Stay");
            animator_Stars02.Play("Stay");
            gameobj_Radiation.SetActive(false);

        }

        //Play star animations in sequence using Invoke
        public void PlayAllEffectAnimation()
        {
            CleanAllAnimator();
            Invoke("PlayStarAnim00", 0.5f);
        }




        void PlayStarAnim00()
        {
            
            animator_Stars00.Play(starAnimClip00.name);
            Invoke("PlayStarAnim01", starAnimClip00.length + _nextEffectWaitTime);

        }

        void PlayStarAnim01()
        {
            animator_Stars01.Play(starAnimClip01.name);
            Invoke("PlayStarAnim02", starAnimClip01.length + _nextEffectWaitTime);
        }

        void PlayStarAnim02()
        {
            animator_Stars02.Play(starAnimClip02.name);
            Invoke("PlayRadiation", starAnimClip02.length + _nextEffectWaitTime);
        }

        void PlayRadiation()
        {
            gameobj_Radiation.SetActive(true);
        }


  



    }
}