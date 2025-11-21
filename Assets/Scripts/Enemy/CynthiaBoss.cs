using System.Collections;
using UnityEngine;

public class CynthiaBoss : EnemyScript
{
    private enum STATE_MACHINE
    {
        FOLLOW,
        PAUSE,
        ATTACK
    }

    private STATE_MACHINE state = STATE_MACHINE.FOLLOW;
    private Coroutine attacking = null;

    [SerializeField] private int numProjectilesInBigAttack;
    [SerializeField] private int numBigAttacks = 3;
    [SerializeField] private float timeBetweenBigShots;

    [SerializeField] private float pauseTimeBeforeAttack;
    [SerializeField] private float pauseTimeAfterAttack;

    [SerializeField] private float timeBeforeBigAttack = 1f;

    private int timeLeft = 60;
    [SerializeField] private float maxTargetDistance = 7f;

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
        if(attacking == null)
        {
            attacking = StartCoroutine(BigAttack());
        }
    }

    private void Follow()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            FindTarget();
            return;
        }

        if (shootCoroutine == null)
        {
            shootCoroutine = StartCoroutine(Shoot());
        }

        agent.SetDestination(target.transform.position);
        agent.isStopped = false;

        if(Vector2.Distance(transform.position, target.transform.position) > maxTargetDistance)
        {
            timeLeft = Mathf.RoundToInt(timeBeforeBigAttack * 60f);
        }
        else
        {
            timeLeft--;
            if(timeLeft <= 0)
            {
                StartCoroutine(PauseCoroutine(STATE_MACHINE.ATTACK, pauseTimeBeforeAttack));
                timeLeft = Mathf.RoundToInt(timeBeforeBigAttack * 60f);
            }
        }
    }

    private IEnumerator BigAttack()
    {
        float angle = 0;
        for (int i = 0; i < numBigAttacks; i++)
        {
            animator.SetBool("Attack", true);
            angle = Random.Range(0, 360f / numProjectilesInBigAttack);

            for (int j = 0; j < numProjectilesInBigAttack; j++)
            {
                Vector3 velocity = Quaternion.Euler(0, angle + (j * (360f / numProjectilesInBigAttack)), 0) * Vector3.forward;
                velocity.Normalize();
                GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
                proj.GetComponent<Rigidbody>().linearVelocity = velocity * projectileVelocity;
                proj.GetComponent<Projectile>().Damage = enemyStats.Damage;
            }

            yield return new WaitForSeconds(timeBetweenBigShots);
        }
        StartCoroutine(PauseCoroutine(STATE_MACHINE.FOLLOW, pauseTimeAfterAttack));
        attacking = null;
        animator.SetBool("Attack", false);
    }

    private IEnumerator PauseCoroutine(STATE_MACHINE newState, float timeToPause)
    {
        state = STATE_MACHINE.PAUSE;
        agent.isStopped = true;

        yield return new WaitForSeconds(timeToPause);

        state = newState;
    }

    public override IEnumerator Shoot()
    {
        {
            animator.SetBool("Shoot", true);
            //Projectile code
            Vector3 velocity = target.transform.position - transform.position;
            velocity.Normalize();
            GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody>().linearVelocity = velocity * projectileVelocity;
            proj.GetComponent<Projectile>().Damage = enemyStats.Damage;

            yield return new WaitForSeconds(shootTimer);
            shootCoroutine = null;
            animator.SetBool("Shoot", false);
        }
    }
}
