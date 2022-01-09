using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTheme : MonoBehaviour
{
    public static MusicTheme MTInstance;
    void Awake()
    {
        if(MTInstance != null && MTInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        MTInstance = this;
        DontDestroyOnLoad(this);
    }
}
