using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class CharField : MonoBehaviour
    {
        string m_nick;
        Image m_image;

        public void Init(string _nick/*, Image _image*/)
        {
            m_nick = _nick; //m_image = _image;

            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            foreach (Transform child in allchildren)
            {
                switch (child.gameObject.name)
                {
                    case "char text field":
                        child.GetComponent<Text>().text = _nick;
                        break;
                    case "char image field":
                        //child.GetComponent<Image>().sprite = _image.sprite;
                        break;
                }
            }
        }
    }
}


