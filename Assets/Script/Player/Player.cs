using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float health;
    public float velocidade;
    public float jumpForce;
    public float atkRadius;

    public float recoveryTime;
    float recoveryCount;

    bool isJumping;
    bool isAttacking;
    bool isDead;

    public Rigidbody2D rig;
    public Animator anim;
    public Transform firepoint;
    public LayerMask enemyLayer;
    public Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            Jump();
            OnAttack();
        }
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

            Collider2D hit = Physics2D.OverlapCircle(firepoint.position, atkRadius, enemyLayer);

            if (hit != null)
            {
                hit.GetComponent<FlightEnemy>().OnHit();
            }

            StartCoroutine(OnAttacking());
        }
    }

    IEnumerator OnAttacking()
    {
        yield return new WaitForSeconds(0.35f);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(firepoint.position, atkRadius);
    }
    public void OnHit(float damage)
    {

        recoveryCount += Time.deltaTime;

        if (recoveryCount >= recoveryTime && isDead == false)
        {
            anim.SetTrigger("hit");
            health -= damage;
            healthBar.fillAmount = health/100;


            GameOver();

            recoveryCount = 0f;
        }
    }

    void GameOver()
    {
        if (health <= 0)
        {
            anim.SetTrigger("die");
            isDead = true;
        }
    }

    void OnMove()
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

    private void FixedUpdate()
    {
        if (isDead == false) {
            OnMove();
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
