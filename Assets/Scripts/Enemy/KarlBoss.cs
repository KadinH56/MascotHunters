using System.Collections;
using UnityEngine;

public class KarlBoss : EnemyScript
{
    private enum STATE_MACHINE
    {
        FOLLOW,
        PAUSE,
        ATTACK
    }

    private STATE_MACHINE state = STATE_MACHINE.FOLLOW;
    private Coroutine attacking = null;

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
        yield return null;
    }

    private IEnumerator PauseCoroutine(STATE_MACHINE newState, float timeToPause)
    {
        state = STATE_MACHINE.PAUSE;
        agent.isStopped = true;

        yield return new WaitForSeconds(timeToPause);

        state = newState;
    }
}
