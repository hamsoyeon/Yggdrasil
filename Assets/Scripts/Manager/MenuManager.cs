using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //public static MenuManager instance;

    enum ShowMenu { WIN = 1, LOSE }


    // 패널이 보이는 경우 메뉴를 눌렀을때,졌을때,스테이지를 클리어했을때(이겼을때)
    public GameObject LosePanel;  //게임이 졌을 때
    public GameObject WinPanel;  //게임에 이겼을 때
    public GameObject MinimapPanel;
    public int m_menu = 0;

    public string thisScene;
    public string endingScene = "EndingScene";

    GameObject blank;

    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name; //현재 씬 이름을 가져옴
        WinPanel = GameObject.Find("UICanvas").transform.Find("Victory_Image").gameObject;
        LosePanel = GameObject.Find("UICanvas").transform.Find("Failed_Image").gameObject;

        blank = GameObject.Find("UICanvas").transform.Find("Blank").gameObject;
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 메뉴버튼을 보여준다.
        }
    }

    CanvasGroup img;
    RectTransform rt;

    public void ShowLoseMenu()
    {
        if (WinPanel.activeSelf)
            return;

        if (SceneManager.GetActiveScene().name == "MainScene")
            SEManager.instance.StopSE("Stage1");
        if (SceneManager.GetActiveScene().name == "Stage2")
            SEManager.instance.StopSE("Stage2");

        SEManager.instance.PlaySE("Fail");

        LosePanel.SetActive(true);  //패배했을때의 패널을 보여주고


        rt = LosePanel.GetComponent<RectTransform>();
        rt.DOScale(1, 2f);

        img = LosePanel.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(1, 2f)
            //.SetUpdate(true);   // Unity TimeScale에 영향받지않고 Dotween 내부적으로 체크할수있게 설정을 변경하는 기능... 이렇게 사용하면 위에서 Time.timeScale이 0이 되어도 Dotween이 돌아간다!!!   
            .OnComplete(TimeStop);  //위에 DOFade가 완료되면 실행될 함수. 

        MinimapPanel.SetActive(false);//미니맵 끄기
    }

    void TimeStop()
    {
        
        Time.timeScale = 0f;

    }


    public void ShowWinMenu()
    {
        if (LosePanel.activeSelf)
            return;

        if (SceneManager.GetActiveScene().name == "MainScene")
            SEManager.instance.StopSE("Stage1");
        if (SceneManager.GetActiveScene().name == "Stage2")
            SEManager.instance.StopSE("Stage2");

        SEManager.instance.PlaySE("Victory");

        WinPanel.SetActive(true);

        rt = WinPanel.GetComponent<RectTransform>();
        rt.DOScale(1, 2f);
            

        //CanvasGroup 넣어주기...
        var img = WinPanel.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(1, 2f)
            .OnComplete(TimeStop);

        MinimapPanel.SetActive(false);//미니맵 끄기
    }

    public void ReStart()
    {

        blank = GameObject.Find("UICanvas").transform.Find("Blank").gameObject;

        blank.SetActive(true);

        rt = blank.GetComponent<RectTransform>();
        rt.DOScale(1, 1f)
            .SetUpdate(true);

        img = blank.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(1, 1f)
            .SetUpdate(true)
            .OnComplete(ThisSceneFade);
            
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

   

    void ThisSceneFade()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(thisScene);
    }

    void NextSceneFade()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(endingScene);
    }

    public void NextStage()
    {
        blank = GameObject.Find("UICanvas").transform.Find("Blank").gameObject;

        blank.SetActive(true);

        rt = blank.GetComponent<RectTransform>();
        rt.DOScale(1, 1f)
            .SetUpdate(true);

        img = blank.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(1, 1f)
            .SetUpdate(true)
            .OnComplete(NextSceneFade);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("UIScene");
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
    }
}
