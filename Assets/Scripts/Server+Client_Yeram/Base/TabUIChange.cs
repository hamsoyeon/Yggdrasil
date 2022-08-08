using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;

public class TabUIChange : MonoBehaviour
{
    //EventSystem system;
    TMP_InputField[] allChildren;
    public TMP_InputField FocusInput;                                       //인풋필드 오브젝트

    public Button OkButton;                                             //확인버튼 오브젝트
    public Button BackButton;                                           //뒤로가기버튼 오브젝트

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        InputKeyboard();
    }

    public void Init()
    {
        allChildren = GetComponentsInChildren<TMP_InputField>();
        SetInputFocus(FocusInput);                  //처음은 ID입력
    }
    public void SetInputFocus(TMP_InputField _input_obj)
    {
        _input_obj.Select();
    }
    public void SetOkBtn(Button _ok_btn)
    {
        OkButton = _ok_btn;
    }
    public void SetCancleBtn(Button _cancle_btn)
    {
        BackButton = _cancle_btn;
    }
    public void InputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Tab))                             
        {
            if (!Input.GetKey(KeyCode.LeftShift))            //Tab하면 아래항목으로 이동
            {
                FocusInput = FocusInput.navigation.selectOnDown as TMP_InputField;
                SetInputFocus(FocusInput);
            }
            else                                              //LEFTShift + Tab하면 위에항목으로 이동
            {
                FocusInput = FocusInput.navigation.selectOnUp as TMP_InputField;
                SetInputFocus(FocusInput);                
            }
        }
        
        else if (Input.GetKeyDown(KeyCode.Return))                                                        //Enter 하면 로그인 버튼 클릭
        {
            // 엔터키를 치면 로그인 (제출) 버튼을 클릭
            OkButton.onClick.Invoke();
            Debug.Log("OKButton pressed!");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton.onClick.Invoke();
            Debug.Log("BackButton pressed!");
        }
    }
}
