using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner3 : MonoBehaviour
{
    [Header("Aiming Line Configuration")]
    [Range(1, 5)]
    [SerializeField] private int maxIterations = 2;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private int trailCount;
    [SerializeField] private LineRenderer trail;

    [Header("Spawner Configuration")]
    [SerializeField] private Transform ballTrail;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private int spawnNum;
    [SerializeField] private float spawnTime;
    [SerializeField] private float spawnDelay;
    [SerializeField] public TextMeshProUGUI ballsLeft;
    
    private Vector2 dir;
    private int spawnsDone = 0;
    public Collider2D spawnerCollider;
    public static bool destroyBalls = false;
    public static int levelCount = 0;
      

    private Vector2 GetMousePosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePostoWorld = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePostoWorld;
    }
    
    private void Start()
    {
        spawnerCollider.enabled = false;
        ballsLeft.text = spawnNum.ToString();
        ballsLeft.enabled = false;
    }

    public int GetSpawnNum() 
    {
        return spawnNum;
    }

    public bool IsBurstActive()
    {
        Ball[] balls = FindObjectsOfType<Ball>();
        //Debug.Log("balls left= " + balls.Length);
        return balls.Length > 0;
    }

    void NextLevelByPlayer()
    {
        //If middle mouse button pressed and there are still balls
        //on the scene, stop spawning, destroy balls and go to next level
        if (Input.GetMouseButtonDown(2) && IsBurstActive())
        {
            spawnsDone = 0;
            destroyBalls = true;

            //spawnerCollider.enabled = true;
            CancelInvoke("SpawnNextBurst");
            Box[] boxes = FindObjectsOfType<Box>();
            foreach (Box box in boxes)
                box.NextLevel();
            levelCount++;
            LevelNum.linesRemained--;

            ballsLeft.enabled = false;
            //spawnerCollider.enabled = false;
        }
    }

    void SpawnNextBurst()
    {
        ballsLeft.enabled = true;
        spawnerCollider.enabled = true;
        GameObject currentBall = Instantiate(ballPrefab, ballTrail.position, ballTrail.rotation);
        currentBall.GetComponent<Ball>().Fire = dir;
        ballsLeft.text = (GetSpawnNum() - spawnsDone).ToString();
        spawnsDone++;

        if (spawnsDone == GetSpawnNum() + 1)
        {
            ballsLeft.text = GetSpawnNum().ToString();
            CancelInvoke("SpawnNextBurst");
            spawnsDone = 0;
        }
    }
        
    public void OnCollisionEnter2D(Collision2D collision)
    {
        // When spawner level is hit by any block
        if (collision.gameObject.tag == "Block")
        {
            Debug.Log("G A M E  O V E R !!!!");
            SceneManager.LoadScene(4);
        }
    }

    private bool RayCast(Vector2 position, Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(position, direction, maxDistance);
        if (hit && trailCount <= maxIterations - 1)
        {
            trailCount++;
            var reflectAngle = Vector2.Reflect(direction, hit.normal);
            trail.positionCount = trailCount + 1;
            trail.SetPosition(trailCount, hit.point);
            RayCast(hit.point + reflectAngle, reflectAngle);
            return true;
        }

        if (hit == false)
        {
            trail.positionCount = trailCount + 2;
            trail.SetPosition(trailCount + 1, position + direction * maxDistance);
        }
        return false;
    }

    void Update()
    {
        NextLevelByPlayer();

        //When left mouse button is pressed
        if (Input.GetMouseButtonUp(0))
        {
            //and there is no active ball burst on the scene
            if (!IsBurstActive())
            {
                //spawn a new burst of balls
                destroyBalls = false;
                dir = (GetMousePosition() - (Vector2)ballTrail.position).normalized;
                InvokeRepeating("SpawnNextBurst", spawnDelay, spawnTime);
            }
        }

        if (!ballsLeft.enabled)
        {
            if (Input.GetMouseButton(0))
            {
                spawnerCollider.enabled = false;
                trailCount = 0;
                trail.positionCount = 1;
                trail.SetPosition(0, ballTrail.position);
                trail.enabled = true;
                RayCast(ballTrail.position, (GetMousePosition() - (Vector2)ballTrail.position).normalized);
            }
        }
        else
        {
            trail.enabled = false;
            spawnerCollider.enabled = true;
        }
    }
}
