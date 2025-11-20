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
    [SerializeField] private float timeDuringBigAttack = 10f;
    [SerializeField] private float timeAfterBigAttack = 1f;

    private int timeLeft = 60;
    [SerializeField] private float maxTargetDistance = 7f;

    [SerializeField] private float distanceFromKarl = 3f;
    [SerializeField] private float hitboxOffset = 0.25f;

    [SerializeField] private float maxDistanceFromKarl = 10f;
    [SerializeField] private float minDistanceFromKarl = 3f;

    [SerializeField] private Transform claws;
    [SerializeField] private LineRenderer[] clawRenderers;
    [SerializeField] private CapsuleCollider clawsCollider;

    private float currentRotation = 15f;
    [SerializeField] private float bigRotation = 7.5f;
    [SerializeField] private float followRotation = 15f;

    [SerializeField] private GameObject bigAttackCircle;
    [SerializeField] private float telegraphDuration = 0.1f;

    private bool isWaiting = false;

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

        HandleClaws();
    }

    private void Attack()
    {
        if(attacking == null)
        {
            attacking = StartCoroutine(BigAttack());
        }
    }

    private void HandleClaws()
    {
        claws.transform.Rotate(0, currentRotation, 0);
        for (int i = 0; i < claws.childCount; i++)
        {
            claws.GetChild(i).localPosition = new(distanceFromKarl / 2 * (i == 0 ? 1 : -1), 0, 0);
        }

        clawsCollider.height = distanceFromKarl + hitboxOffset;
    }

    private void Update()
    {
        foreach(LineRenderer lineRenderer in clawRenderers)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, lineRenderer.gameObject.transform.position);
        }
    }

    private void Follow()
    {
        agent.speed = enemyStats.Movement;
        currentRotation = followRotation;
        distanceFromKarl = minDistanceFromKarl;
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
        for (int i = 0; i < 5; i++) 
        {
            GameObject circle = Instantiate(bigAttackCircle, transform.position, Quaternion.identity);

            yield return new WaitForSeconds(telegraphDuration);

            Destroy(circle);

            yield return new WaitForSeconds(0.1f); 
        }

        agent.speed = enemyStats.Movement / 2f;
        currentRotation = bigRotation;
        while (distanceFromKarl < maxDistanceFromKarl)
        {
            distanceFromKarl = Mathf.MoveTowards(distanceFromKarl, maxDistanceFromKarl, 1f);
            yield return null;
        }
        distanceFromKarl = maxDistanceFromKarl;

        agent.isStopped = false;
        isWaiting = true;
        StartCoroutine(WaitTime(timeDuringBigAttack));
        while (isWaiting)
        {
            agent.SetDestination(target.transform.position);
            yield return null;
        }
        agent.isStopped = true;

        while (distanceFromKarl > minDistanceFromKarl)
        {
            distanceFromKarl = Mathf.MoveTowards(distanceFromKarl, minDistanceFromKarl, 1f);
            yield return null;
        }
        distanceFromKarl = minDistanceFromKarl;

        StartCoroutine(PauseCoroutine(STATE_MACHINE.FOLLOW, timeAfterBigAttack));
        attacking = null;
    }

    private IEnumerator WaitTime(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
    }

    private IEnumerator PauseCoroutine(STATE_MACHINE newState, float timeToPause)
    {
        state = STATE_MACHINE.PAUSE;
        agent.isStopped = true;

        yield return new WaitForSeconds(timeToPause);

        state = newState;
    }

    //private void OnDrawGizmos()
    //{
    //    foreach (LineRenderer lineRenderer in clawRenderers)
    //    {
    //        lineRenderer.SetPosition(0, transform.position);
    //        lineRenderer.SetPosition(1, lineRenderer.gameObject.transform.position);
    //    }

    //    //claws.transform.Rotate(0, currentRotation, 0);
    //    for (int i = 0; i < claws.childCount; i++)
    //    {
    //        claws.GetChild(i).localPosition = new(distanceFromKarl / 2 * (i == 0 ? 1 : -1), 0, 0);
    //    }

    //    clawsCollider.height = distanceFromKarl + hitboxOffset;
    //}
}
