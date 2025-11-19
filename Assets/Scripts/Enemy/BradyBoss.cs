using System.Collections;
using UnityEngine;

public class BradyBoss : EnemyScript
{
    private enum STATE_MACHINE
    {
        FOLLOW,
        ATTACK
    }

    [SerializeField] private float canAttackDistance;
    [SerializeField] private float timeBetweenAttacks = 1;
    [SerializeField] private float pauseTime = 1f;
    [SerializeField] private float dashSpeedModifier = 1.5f;

    [SerializeField] private float timeBetweenChomps = 0.5f;
    [SerializeField] private int numChomps = 3;
    [SerializeField] private int numAttacksPerChomp = 2;

    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float chompSize = 3f;
    [SerializeField] private GameObject chompCircle;

    [SerializeField] private GameObject dashTelegraphPrefab;
    [SerializeField] private float telegraphDuration = 1f;

    private STATE_MACHINE state = STATE_MACHINE.FOLLOW;

    bool attacking = false;
    int timeLeft = 0;

    Coroutine pauseCoroutine;
    Coroutine attackCoroutine;
    bool paused = false;

    public override void EnemyAI()
    {
        switch (state)
        {
            case STATE_MACHINE.FOLLOW:
                Follow();
                break;
            case STATE_MACHINE.ATTACK:
                Attack();
                break;
        }
    }

    private void Attack()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }

        if (paused && pauseCoroutine == null)
        {
            agent.isStopped = true;
            pauseCoroutine = StartCoroutine(StopMoving());
            return;
        }

        if(pauseCoroutine != null)
        {
            return;
        }

        if(attackCoroutine == null && !attacking)
        {
            attackCoroutine = StartCoroutine(ChompAttack());
            return;
        }

        if(attackCoroutine != null)
        {
            return;
        }

        timeLeft = Mathf.RoundToInt(timeBetweenAttacks * 60);
        state = STATE_MACHINE.FOLLOW;
        attacking = false;
    }

    private IEnumerator StopMoving()
    {
        yield return new WaitForSeconds(pauseTime);
        paused = false;
        pauseCoroutine = null;

    }

    private IEnumerator ChompAttack()
    {
        chompCircle.SetActive(true);
        chompCircle.transform.localScale = Vector3.one * chompSize;
        attacking = true;
        for (int i = 0; i < numChomps; i++)
        {
            if (target == null)
            {
                attackCoroutine = null;
                yield break;
            }

            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            Vector3 start = transform.position;
            Vector3 end = target.transform.position; // or start + (direction * 8f) if you prefer a fixed length

            GameObject telegraphObj = Instantiate(dashTelegraphPrefab);
            LineRenderer lr = telegraphObj.GetComponent<LineRenderer>();

            // Ensure LineRenderer exists
            lr.positionCount = 2;
            lr.useWorldSpace = true;

            //Make it thick
            lr.widthMultiplier = 0.5f;     
            lr.startWidth = 0.5f;
            lr.endWidth = 0.5f;

            Vector3 Begin = transform.position;
            Vector3 Stop = target.transform.position;

            lr.SetPosition(0, start);
            lr.SetPosition(1, end);

         
            Destroy(telegraphObj, telegraphDuration);

            yield return new WaitForSeconds(telegraphDuration);

            GetComponent<Rigidbody>().linearVelocity = dashSpeedModifier * enemyStats.Movement * direction;

            if (GetComponent<Rigidbody>().linearVelocity.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }

            StartCoroutine(Chomp());
            yield return new WaitForSeconds(timeBetweenChomps);
        }

        GetComponent<Rigidbody>().linearVelocity = Vector2.zero;
        chompCircle.SetActive(false);
        attackCoroutine = null;
    }

    private IEnumerator Chomp()
    {
        for (int i = 0; i < numAttacksPerChomp; i++)
        {
            animator.SetBool("Attack", true);
            Collider[] players = Physics.OverlapSphere(transform.position, chompSize, playerLayer, QueryTriggerInteraction.Collide);
            foreach(Collider player in players)
            {
                player.transform.parent.GetComponent<PlayerStatManager>().TakeDamage(enemyStats.Damage);
            }
            yield return new WaitForSeconds(timeBetweenChomps / numAttacksPerChomp);
            animator.SetBool("Attack", false);
        }
    }

    private void Follow()
    {
        //print(timeLeft);
        if(target == null)
        {
            FindTarget();
            return;
        }

        if (agent.velocity.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        //State Machine stuff
        if (Vector2.Distance(transform.position, target.transform.position) > canAttackDistance)
        {
            timeLeft = Mathf.RoundToInt(60 * timeBetweenAttacks);
        }

        timeLeft--;
        
        if(timeLeft <= 0)
        {
            paused = true;
            state = STATE_MACHINE.ATTACK;
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(target.transform.position);
    }
}
