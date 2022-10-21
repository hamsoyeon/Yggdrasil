using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ClassicUIKit
{

    public class TabUI : MonoBehaviour
    {
        public GameObject anotherGameObj00;
        public Sprite activeButtonSprite;
        public Sprite disableButtonSprite;
        public AudioClip audioClip;

        AudioSource audioSource;


        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void ClickTabActiveChange()
        {
            Image thisImage = GetComponent<Image>();
            Image anotherImage00 = anotherGameObj00.GetComponent<Image>();

            //Change active image if user clicked tab is disable image
            if(thisImage.sprite == disableButtonSprite)
            {

                //Play sound
                if(audioSource != null && audioSource.clip != null)
                { 
                    audioSource.PlayOneShot(audioClip);
                }

                //Change sprite
                thisImage.sprite = activeButtonSprite;
                anotherImage00.sprite = disableButtonSprite;

                //Change order of tabs
                transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
                anotherGameObj00.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
                //Change posY of tab
                float disablePosY = transform.localPosition.y;
                transform.localPosition = new Vector3(transform.localPosition.x, anotherGameObj00.transform.localPosition.y, transform.localPosition.z);
                anotherGameObj00.transform.localPosition = new Vector3(anotherGameObj00.transform.localPosition.x, disablePosY, anotherGameObj00.transform.localPosition.z);

            }
        }

    }
}