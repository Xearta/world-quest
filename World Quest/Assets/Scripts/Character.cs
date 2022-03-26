using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    protected Animator animator;

    protected Vector2 direction;

    private Rigidbody2D rb;

    protected bool isAttacking = false;

    protected Coroutine attackRoutine;

    public bool isMoving
    {
        get
        {
            return direction.x != 0 || direction.y != 0;
        }
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        HandleAnimation();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        rb.velocity = direction.normalized * speed;
    }

    public void HandleAnimation()
    {
        // Check for movement
        if (isMoving)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);

            StopAttack();
        }
        else if (isAttacking)
        {
            animator.SetBool("isAttacking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }

    public virtual void StopAttack()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
        }
    }
}
