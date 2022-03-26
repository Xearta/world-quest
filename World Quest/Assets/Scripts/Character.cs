using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Animator animator;

    protected Vector2 direction;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);

        // Check for movement
        if (direction.x != 0 || direction.y != 0)
        {
            AnimateMovement(direction);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

    }

    public void AnimateMovement(Vector2 direction)
    {
        animator.SetBool("isWalking", true);
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);
    }
}
