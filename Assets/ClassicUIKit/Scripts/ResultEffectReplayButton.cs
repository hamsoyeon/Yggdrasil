using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClassicUIKit
{
    public class ResultEffectReplayButton : MonoBehaviour
    {
        public ResultEffectManager resultEffect;

        public void ReplayEffect()
        {

            resultEffect.PlayAllEffectAnimation();
        }


    }
}