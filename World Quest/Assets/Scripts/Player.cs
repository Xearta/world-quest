using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField]
    private Stat mana;

    private float initMana = 50;

    [SerializeField]
    private Block[] blocks;

    [SerializeField]
    private Transform[] exitPoints; // exit points for spells

    private int exitIndex = 2;

    private SpellBook spellBook;

    private Vector3 min, max;

    public Transform MyTarget { get; set; }

    // Start is called before the first frame update
    protected override void Start()
    {
        spellBook = GetComponent<SpellBook>();
        mana.Initialize(initMana, initMana);

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, min.x, max.x), 
            Mathf.Clamp(transform.position.y, min.y, max.y), transform.position.z);

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

    public void SetLimits(Vector3 min, Vector3 max)
    {
        this.min = min;
        this.max = max;
    }

    private IEnumerator Attack(int spellIndex)
    {
        Transform currentTarget = MyTarget;


        Spell newSpell = spellBook.CastSpell(spellIndex);

        isAttacking = true;
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(newSpell.MyCastTime);

        if (currentTarget != null && InLineOfSight())
        {
            SpellScript s = Instantiate(newSpell.MySpellPrefab, exitPoints[exitIndex].position, Quaternion.identity).GetComponent<SpellScript>();

            s.Initialize(currentTarget, newSpell.MyDamage);
        }

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
        if (MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
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

    public override void StopAttack()
    {
        spellBook.StopCasting();
        base.StopAttack();
    }
}
