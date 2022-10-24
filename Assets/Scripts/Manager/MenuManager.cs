using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    //public static MenuManager instance;

    public class GlobalValue
    {
        public enum Transition
        {
            Fade,
        }
    }


    enum ShowMenu { WIN = 1, LOSE }


    // 패널이 보이는 경우 메뉴를 눌렀을때,졌을때,스테이지를 클리어했을때(이겼을때)
    public GameObject LosePanel;  //게임이 졌을 때
    public GameObject WinPanel;  //게임에 이겼을 때
    public GameObject MinimapPanel;
    public int m_menu = 0;

    public string thisScene;
    public string endingScene = "EndingScene";

    //private void Awake()
    //{
    //    if (null == instance)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(this.gameObject);

    //    }
    //    else
    //    {
    //        Destroy(this.gameObject);
    //    }

    //}


    //public static MenuManager Instance
    //{
    //    get
    //    {
    //        if (null == instance)
    //        {
    //            return null;
    //        }
    //        return instance;
    //    }

    //}


    // Start is called before the first frame update
    void Start()
    {
        thisScene = SceneManager.GetActiveScene().name; //현재 씬 이름을 가져옴
        LosePanel = GameObject.Find("UICanvas").transform.Find("LosePanel").gameObject;



    }





    // Update is called once per frame
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


        //LosePanel = GameObject.Find("UICanvas").transform.Find("LosePanel").gameObject;
        //Time.timeScale = 0f;
        LosePanel.SetActive(true);  //패배했을때의 패널을 보여주고



        rt = LosePanel.GetComponent<RectTransform>();
        rt.DOScale(1, 2f);

        img = LosePanel.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(1, 2f)
            //.SetUpdate(true);   // Unity TimeScale에 영향받지않고 Dotween 내부적으로 체크할수있게 설정을 변경하는 기능... 이렇게 사용하면 위에서 Time.timeScale이 0이 되어도 Dotween이 돌아간다!!!   
            .OnComplete(TweenComplete);  //위에 DOFade가 완료되면 실행될 함수. 

        MinimapPanel.SetActive(false);//미니맵 끄기
    }

    void TweenComplete()
    {
        Debug.Log("트윈함수 완료");
        Time.timeScale = 0f;

    }



    public void ShowWinMenu()
    {
        if (LosePanel.activeSelf)
            return;

        Time.timeScale = 0f;
        WinPanel.SetActive(true);


        rt = LosePanel.GetComponent<RectTransform>();
        rt.DOScale(1, 2f);

        //CanvasGroup 넣어주기...
        var img = LosePanel.GetComponent<CanvasGroup>();//.alpha;
        img.DOFade(1, 2f)
            .SetUpdate(true); 

        //WinPanel = GameObject.Find("UICanvas").transform.Find("WinPanel").gameObject;

        MinimapPanel.SetActive(false);//미니맵 끄기
    }

    public void ReStart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(thisScene);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextStage()
    {
        SceneManager.LoadScene(endingScene);
    }

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
