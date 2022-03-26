using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat health;

    [SerializeField]
    private Stat mana;

    private float initHealth = 100;
    private float initMana = 50;

    // Start is called before the first frame update
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        base.Update();  // Execute the Character.cs Update function
    }

    private void GetInput()
    {   

        //! ===TESTING ONLY===
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }

        //! ===END OF TESTING===


        direction = Vector2.zero;

        // Movement
        if (Input.GetKey(KeyCode.W))
        {
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction += Vector2.right;
        }

        // Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            attackRoutine = StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (!isAttacking && !isMoving)
        {
            isAttacking = true;
            animator.SetBool("isAttacking", true);
            yield return new WaitForSeconds(3); //! Hard-coded cast time for testing

            StopAttack();
            Debug.Log("Done casting");
        }
    }
}
