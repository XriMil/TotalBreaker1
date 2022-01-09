using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SNDManager : MonoBehaviour
{
    public AudioSource source;

    public AudioClip click;

    public static SNDManager SNDMInstance;

    void Awake()
    {
        if (SNDMInstance != null && SNDMInstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        SNDMInstance = this;
        DontDestroyOnLoad(this);
    }

}
