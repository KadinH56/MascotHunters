using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Simple enemy AI
/// </summary>
public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Stats enemyStats;

    [SerializeField] private float distanceFromPlayer = 0f;
    [SerializeField] private bool isBoss = false;
    private NavMeshAgent agent;
    private Rigidbody erigidbody;

    private PlayerMovement target;

    private void Start()
    {
        enemyStats.Health = enemyStats.MaxHealth;
        agent = GetComponent<NavMeshAgent>();

        agent.stoppingDistance = distanceFromPlayer;
    }

    private void FixedUpdate()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            FindTarget();
            return;
        }

        agent.SetDestination(target.transform.position);

        erigidbody.linearVelocity = (agent.nextPosition - transform.position) * enemyStats.Movement;
    }

    /// <summary>
    /// Finds a target
    /// </summary>
    private void FindTarget()
    {
        foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            if(target == null || !target.gameObject.activeSelf)
            {
                target = player.GetComponent<PlayerMovement>();
                continue;
            }

            if(Vector3.Distance(transform.position, player.transform.position) < Vector3.Distance(transform.position, target.transform.position))
            {
                target = player.GetComponent<PlayerMovement>();
                continue;
            }
        }
    }
}
