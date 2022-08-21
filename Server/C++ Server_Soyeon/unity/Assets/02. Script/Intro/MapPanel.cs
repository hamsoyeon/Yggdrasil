using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace test_client_unity
{
    public class MapPanel : MonoBehaviour
    {
        [SerializeField] private Button Select_Btn;

        void Start()
        {
            Select_Btn.onClick.AddListener(OnClick_SelectMap);
        }

        void OnClick_SelectMap()
        {
            this.gameObject.SetActive(false);
        }
    }
}