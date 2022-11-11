using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SEManager.instance.LoopPlaySE("Intro");
    }

    void Update()
    {

    }
}
