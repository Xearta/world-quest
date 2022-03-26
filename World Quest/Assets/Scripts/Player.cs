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

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private GameObject[] spellPrefabs;

    [SerializeField]
    private Transform[] exitPoints; // exit points for spells

    private int exitIndex = 2;

    public Transform MyTarget { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        health.Initialize(initHealth, initHealth);
        mana.Initialize(initMana, initMana);

        //TODO Just for testing
        MyTarget = GameObject.Find("Target").transform;

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

        //TODO ===TESTING ONLY===
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
            exitIndex = 0;
            direction += Vector2.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            exitIndex = 3;
            direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
         
            exitIndex = 2;
            direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.D))
        {
            exitIndex = 1;
            direction += Vector2.right;
        }
    }

    private IEnumerator Attack(int spellIndex)
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(1); //TODO Hard-coded cast time for testing

        Spell s = Instantiate(spellPrefabs[spellIndex], exitPoints[exitIndex].position, Quaternion.identity).GetComponent<Spell>();

        s.MyTarget = MyTarget;

        StopAttack();
    }

    public void CastSpell(int spellIndex)
    {
         Block();

        if (MyTarget != null && !isAttacking && !isMoving && InLineOfSight())
            attackRoutine = StartCoroutine(Attack(spellIndex));
    }

    private bool InLineOfSight()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

        if (hit.collider == null)
        {
            return true;
        }

        return false;
    }

    private void Block()
    {
        foreach (Block b in blocks)
        {
            b.Deactivate();
        }

        blocks[exitIndex].Activate();
    }
}
