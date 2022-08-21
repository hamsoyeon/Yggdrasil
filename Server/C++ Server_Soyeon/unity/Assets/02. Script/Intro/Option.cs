using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class Option : MonoBehaviour
    {
        void Start()
        {
            Transform[] allchildren = this.GetComponentsInChildren<Transform>();

            foreach (Transform child in allchildren)
            {
                switch (child.gameObject.name)
                {
                    case "Save Btn":
                        child.GetComponent<Button>().onClick.AddListener(OnClick_Save);
                        break;
                    case "Close Btn":
                        child.GetComponent<Button>().onClick.AddListener(onClick_Close);
                        break;
                }
            }
        }

        #region button click event
        void OnClick_Save()
        {
            this.gameObject.SetActive(false);
        }
        void onClick_Close()
        {
            this.gameObject.SetActive(false);
        }
        #endregion
    }
}