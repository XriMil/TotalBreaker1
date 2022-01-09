using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DestroyBall : MonoBehaviour
{
    private int ballsDestroyed = 0;   //Counter of balls being destroyed
    Spawner3 spawner;

    private void Start()
    {
        //ballsDestroyed = 0;
        spawner = FindObjectOfType<Spawner3>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //If a ball collide with Floor, destroy it
        //and increment counter
        if (col.gameObject.tag == "Ball")
        {
            ballsDestroyed++;
            Destroy(col.gameObject);
        }
    }

    public void ChangeNextLevel()
    {
        Box[] boxes = FindObjectsOfType<Box>();
        foreach (Box box in boxes)
        {
            box.NextLevel();
        }
    }

    void Update()
    {
        //When all balls are destroyed, move downwards one level,
        //upadte the absolute grid position of all remaining boxes 
        //and reset counter.

        if (ballsDestroyed >= spawner.GetSpawnNum() && !spawner.IsBurstActive())
        {
            spawner.spawnerCollider.enabled = false;
            ChangeNextLevel();
            Spawner3.levelCount++;
            LevelNum.linesRemained--;
            spawner.ballsLeft.enabled = false;
            ballsDestroyed = 0;
        }
    }
}
