using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlightEnemy : MonoBehaviour
{
    public int health;

    public float speed;
    float initialSpeed;
    public bool isRight;
    public float stopDistance;

    public Rigidbody2D rig;
    public Animator anim;

    Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        float playerPos = transform.position.x - player.position.x;

        if (playerPos > 0)
        {
            isRight = false;
        }
        else
        {
            isRight = true;
        }


        if (distance <= stopDistance)
        {
            speed = 0f;
            player.GetComponent<Player>().OnHit();
        }
        else
        {
            speed = initialSpeed; 
        }
    }

    private void FixedUpdate()
    {
        if (isRight)
        {
            rig.velocity = new Vector2(speed, rig.velocity.y);
            transform.eulerAngles = new Vector2(0, 0);
        }
        else
        {
            rig.velocity = new Vector2(-speed, rig.velocity.y);
            transform.eulerAngles = new Vector2(0, 180);
        }
    }

    public void OnHit()
    {

        health--;
        if (health <= 0)
        {
            speed = 0f;
            anim.SetTrigger("death");
            Destroy(gameObject, 1f);
        }
        else
        {
            anim.SetTrigger("hit");
        }
    }

}
