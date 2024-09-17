using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private int power;
    private Vector2 fire;
    AudioSource BallHit;
    int cnt = 0;

    public Vector2 Fire
    {
        get { return fire; }
        set { fire = value; }
    }

    void PlayBallHit()
    {
        BallHit = gameObject.GetComponent<AudioSource>();
        BallHit.Play();
    }
    void Start()
    {
        gameObject.transform.position += new Vector3(0, 0, 1);
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(fire * power, ForceMode2D.Impulse);
    }

    private void OnDestroy()
    {
        //If a ball collide with Floor, destroy it
        //and increment counter
        cnt++;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        // When box or wall is hit
        if (col.gameObject.tag == "Block" || col.gameObject.tag == "Wall")
        {
            PlayBallHit();
        }
    }
    void Update()
    {
        if (Spawner3.destroyBalls)
            Destroy(gameObject);
    }
}