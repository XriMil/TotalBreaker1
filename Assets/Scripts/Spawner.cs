using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private int spawnNum;
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnDelay;

    private int spawnedBalls = 0;
    private Vector2 dir;

    private Vector2 GetMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePostoWorld = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePostoWorld;
    }
    
    // Start is called before the first frame update
    private void Start()
    {
       
    }

    public int GetSpawnNum()
    {
        return spawnNum;
    }

    public bool IsBurstActive()
    {
        Ball[] balls = FindObjectsOfType<Ball>();
        Debug.Log("balls left= " + balls.Length);
        return balls.Length > 0;
    }

    //Spawns next burst
    public void SpawnNextBurst()
    {
        GameObject ball = Instantiate(ballPrefab, transform.position, transform.rotation);
        ball.GetComponent<Ball>().Fire = dir;
        spawnedBalls++;
        if (spawnedBalls == spawnNum)
        {
            spawnedBalls = 0;
            CancelInvoke("SpawnNextBurst");
        }
    }
        
    //Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!IsBurstActive())
            {
                dir = (GetMousePosition() - (Vector2)transform.position).normalized;
                InvokeRepeating("SpawnNextBurst", spawnDelay, spawnTime);
            }
        }
    }
}
