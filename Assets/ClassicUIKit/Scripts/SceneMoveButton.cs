using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace ClassicUIKit
{
    public class SceneMoveButton : MonoBehaviour
    {
        public void SceneMove(string sceneName)
        {

            if (sceneName == null)
            {
                Debug.Log("No arguments");
                return; 
            }
            SceneManager.LoadScene(sceneName);
            
        }

    }

}
