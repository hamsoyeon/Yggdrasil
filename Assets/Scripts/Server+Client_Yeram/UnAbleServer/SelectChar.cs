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
    [SerializeField]
    Image[] skill_Img;
    [SerializeField]
    int char_Count;
    [SerializeField]
    Image[] select_Img;
    private void Start()
    {
        Setting_Char(0);
    }
    
    public void Setting_Char(int target)
    {
        string temp = file_name + target;                                 //리소스 주소
        charView.texture = Resources.Load<RenderTexture>(temp);
        char_Name_Text.text = char_name[target];
        for (int i = 0; i < skill_Img.Length; i++)
        {
            string skillAds = "Imgae/Skill_" + target + "_" + i;
            skill_Img[i].sprite = Resources.Load<Sprite>(skillAds);
        }
        for (int i = 0; i < char_Count; i++)
        {
            if (i == target)
            {
                select_Img[i].color = new Color(125 / 255f, 125 / 255f, 125 / 255f, 255 / 255f);
                Debug.Log("change");
            }
            else
            {
                select_Img[i].color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            }
        }
    }
    public void OnClickRightBtn()
    {
        if (target < char_Count - 1)
        {
            target++;
            Setting_Char(target);
            
        }
        
    }
    public void OnClickLeftBtn()
    {
        if (target > 0)
        {
            target--;
            Setting_Char(target);
            
        }
        
    }
}
