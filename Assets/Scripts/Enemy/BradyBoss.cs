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

    [SerializeField] private LayerMask playerLayer;

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
        attacking = false;
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
            GetComponent<Rigidbody>().linearVelocity = dashSpeedModifier * enemyStats.Movement * direction;
            yield return new WaitForSeconds(timeBetweenChomps);
        }

        GetComponent<Rigidbody>().linearVelocity = Vector2.zero;

        attackCoroutine = null;
    }

    private void Follow()
    {
        if(target == null)
        {
            FindTarget();
            return;
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
