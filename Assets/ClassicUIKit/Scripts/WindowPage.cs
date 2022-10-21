　using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ClassicUIKit
{
    public class WindowPage : MonoBehaviour
    {

        public List<GameObject> windowPage;
        public Button buttonNext;
        public Button buttonBack;

        int nowPageNumber;



        private void Start()
        {
            nowPageNumber = 0;
            buttonBack.interactable = false;
            buttonNext.interactable = true;


            if (nowPageNumber == 1)
            {
                buttonNext.interactable = false;
            }

        }

        public void TurnAPage(string calculation)
        {
           

             if (calculation == "Plus")
            {

                //Check active button
                if (nowPageNumber == windowPage.Count - 1)
                {
                    return;
                }

                //Show page
                buttonBack.interactable = true;
                windowPage[nowPageNumber].SetActive(false);
                nowPageNumber++;
                windowPage[nowPageNumber].SetActive(true);

                //Check arrow button
                if (nowPageNumber >= windowPage.Count - 1)
                {
                    buttonNext.interactable = false;
                }

            }

            else if (calculation == "Minus")
            {
                //Check active button
                if (nowPageNumber == 0)
                {
                    return;
                }

                //Show page
                buttonNext.interactable = true;
                windowPage[nowPageNumber].SetActive(false);
                nowPageNumber--;
                windowPage[nowPageNumber].SetActive(true);

                //Check arrow button
                if (nowPageNumber == 0)
                {
                    buttonBack.interactable = false;
                }

            }
            else
            {
                Debug.Log("Argument error");
            }


        }

    }
}
