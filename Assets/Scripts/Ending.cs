using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{

    GameObject blank;
    CanvasGroup img;
    RectTransform rt;

    public Text text;

    void Start()
    {
        blank = GameObject.Find("Canvas").transform.Find("Blank").gameObject;
        blank.SetActive(true);
        rt = blank.GetComponent<RectTransform>();
        rt.DOScale(0, 1f);

        img = blank.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(0, 1f)
            .OnComplete(BlankActive);


        


    }

    void BlankActive()
    {
        blank.SetActive(false);
        //Tweener tTweener = text.DOText("Thank you...", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
