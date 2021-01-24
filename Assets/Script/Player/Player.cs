using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float velocidade;
    public float jumpForce;

    bool isJumping;
    bool isAttacking;

    public Rigidbody2D rig;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        OnAttack();
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isJumping == false)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                isJumping = true;
            }
        }
    }

    void OnAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            isAttacking = true;
            anim.SetInteger("transition", 3);

            StartCoroutine(OnAttacking());
        }
    }

    IEnumerator OnAttacking()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false;
    }

    private void FixedUpdate()  
    {
        float direcao = Input.GetAxis("Horizontal");
        rig.velocity = new Vector2(direcao * velocidade, rig.velocity.y);
        if (direcao > 0 && !isJumping && !isAttacking)
        {
            transform.eulerAngles = new Vector2(0, 0);
            anim.SetInteger("transition", 1);
        }

        if (direcao < 0 && !isJumping && !isAttacking)
        {
            transform.eulerAngles = new Vector2(0, 180);
            anim.SetInteger("transition", 1);
        }

        if (direcao == 0 && !isJumping && !isAttacking)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            isJumping = false;
        }
    }
}
