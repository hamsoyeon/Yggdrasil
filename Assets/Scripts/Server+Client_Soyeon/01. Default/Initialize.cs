using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace test_client_unity
{
    public class Initialize : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(GameObject.Find("Mgr"));
        }
    }
}


