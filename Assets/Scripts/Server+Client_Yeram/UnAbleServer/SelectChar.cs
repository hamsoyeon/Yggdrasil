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
    Image select_Img;
    private void Start()
    {
        Setting_Char(0);
    }
    
    public void Setting_Char(int target)
    {
        string temp = file_name + target;
        charView.texture = Resources.Load<RenderTexture>(temp);
        char_Name_Text.text = char_name[target];
        string temp2 = "Character_" + target + "_Img";
        select_Img = GameObject.Find(temp2).GetComponent<Image>();
        select_Img.color = Color.gray;
        for (int i = 0; i < skill_Img.Length; i++)
        {
            string skillAds = "Imgae/Skill_" + target + "_" + i;
            skill_Img[i].sprite = Resources.Load<Sprite>(skillAds);
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
