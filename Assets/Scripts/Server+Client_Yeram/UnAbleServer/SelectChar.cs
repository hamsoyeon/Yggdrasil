using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectChar : MonoBehaviour
{
    public int target;
    [SerializeField]
    RawImage charView;
    string file_name = "RenderTexture/Char_";
    [SerializeField]
    string[] char_name;
    [SerializeField]
    TextMeshProUGUI char_Name_Text;
    private void Start()
    {
        target = 0;
        string temp = file_name + target;
        charView.texture = Resources.Load<RenderTexture>(temp);
        char_Name_Text.text = char_name[0];
    }
    public void OnClickChar_0()
    {
        target = 0;
        string temp = file_name + target;
        charView.texture = Resources.Load<RenderTexture>(temp);
        char_Name_Text.text = char_name[0];
    }
    public void OnClickChar_1()
    {
        target = 1;
        string temp = file_name + target;
        charView.texture = Resources.Load<RenderTexture>(temp);
        char_Name_Text.text = char_name[1];
    }
}
