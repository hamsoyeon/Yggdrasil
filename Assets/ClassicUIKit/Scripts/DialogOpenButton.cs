using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ClassicUIKit
{
    public class DialogOpenButton : MonoBehaviour
    {

        public GameObject dialogWindow;


        public void DialogOpen()
        {
            dialogWindow.SetActive(true);
        }


    }
}
