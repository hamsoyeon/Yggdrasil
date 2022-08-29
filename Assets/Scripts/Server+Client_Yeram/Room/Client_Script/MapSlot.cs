using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSlot : MonoBehaviour
{
    [SerializeField]
    int mode;

    public int Mode
    {
        get => mode;
    }
    
    public void __Initialize(int _mode)
    {
        mode = _mode;
    }
}
