using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNum : MonoBehaviour
{
    public static int linesRemained;
    public TextMeshProUGUI linesToShow;

    // Start is called before the first frame update
    void Start()
    {
        linesToShow.text = linesRemained.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (linesRemained > 0)
            linesToShow.text = linesRemained.ToString();
        else
            linesToShow.text = 0.ToString();
    }
}
