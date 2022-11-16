using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PanelTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public void OnPointerDown(PointerEventData data)
    {
        Debug.Log("스크린 터치");
    }

    public void OnPointerUp(PointerEventData data)
    {
        SceneManager.LoadScene("UIScene");
    }
}
