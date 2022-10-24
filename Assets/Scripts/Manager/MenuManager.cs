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

    public void ShowLoseMenu()
    {
        if (WinPanel.activeSelf)
            return;

        //LosePanel = GameObject.Find("UICanvas").transform.Find("LosePanel").gameObject;
        //Time.timeScale = 0f;
        LosePanel.SetActive(true);  //패배했을때의 패널을 보여주고
        MinimapPanel.SetActive(false);//미니맵 끄기
    }

    public void ShowWinMenu()
    {
        if (LosePanel.activeSelf)
            return;

        Time.timeScale = 0f;

        //WinPanel = GameObject.Find("UICanvas").transform.Find("WinPanel").gameObject;
        WinPanel.SetActive(true);
        MinimapPanel.SetActive(false);//미니맵 끄기
    }

    public void ReStart()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadSceneAsync(thisScene);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextStage()
    {
        SceneManager.LoadSceneAsync(endingScene);
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
