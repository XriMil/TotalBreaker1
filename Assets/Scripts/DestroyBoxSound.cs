using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBoxSound : MonoBehaviour
{
    public static int destroyedBoxesCount = 0;
    AudioSource BoxBang;
    int prevCount;
    void Start()
    {
        prevCount = destroyedBoxesCount;
    }
    
    void PlayBoxBang()
    {
        BoxBang = gameObject.GetComponent<AudioSource>();
        BoxBang.Play();
    }
    

    void Update()
    {
        if (destroyedBoxesCount > prevCount)
        {
            PlayBoxBang();
            prevCount = destroyedBoxesCount;
        }
        else
            return;
            
    }
}
