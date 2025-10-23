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
    private Coroutine shooting = null;

    [SerializeField] private int numProjectilesInBigAttack;
    [SerializeField] private float timeBetweenBigShots;
    [SerializeField] private float timeBetweenSmallShots;

    [SerializeField] private float pauseTimeBeforeAttack;
    [SerializeField] private float pauseTimeAfterAttack;

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

        if (shooting == null)
        {
            shooting = StartCoroutine(Shoot());
        }

        agent.SetDestination(target.transform.position);
    }

    private IEnumerator BigAttack()
    {
        yield return null;

        StartCoroutine(PauseCoroutine(STATE_MACHINE.FOLLOW, pauseTimeAfterAttack));
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
        yield return null;
    }

    private void BigShoot(float angle)
    {

    }
}
