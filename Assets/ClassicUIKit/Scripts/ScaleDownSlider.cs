using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ClassicUIKit
{
    public class ScaleDownSlider : MonoBehaviour
    {

        public Transform ScrollView;
       

        public void ChangeScrollViewSize(float i)
        {
            float z = ScrollView.localScale.z;
            ScrollView.localScale = new Vector3(i, i, z);
        }



    }
}