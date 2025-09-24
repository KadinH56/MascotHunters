using System.Collections;
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
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootTimer;

    [SerializeField] private bool isMelee = true;
    [SerializeField] private float meleeTime = 5f;

    [SerializeField] private Collider meleeBox;

    private Coroutine shootCoroutine;

    private PlayerMovement target;

    private void Start()
    {
        enemyStats.Health = enemyStats.MaxHealth;
        agent = GetComponent<NavMeshAgent>();
        //erigidbody = GetComponent<Rigidbody>();

        agent.stoppingDistance = distanceFromPlayer;
        agent.speed = enemyStats.Movement;

        if (isMelee)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    private void FixedUpdate()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            FindTarget();
            return;
        }

        agent.SetDestination(target.transform.position);

        if(shootCoroutine != null && projectile != null)
        {
            shootCoroutine = StartCoroutine(Shoot());
        }
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

    public void TakeDamage(int damage)
    {
        enemyStats.Health -= damage;

        if(enemyStats.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator Shoot()
    {
        //Projectile code
        yield return new WaitForSeconds(shootTimer);
        shootCoroutine = null;
    }

    public IEnumerator MeleeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(meleeTime);
            print("Melee Attack!");
            meleeBox.enabled = true;
            yield return new WaitForFixedUpdate();
            meleeBox.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerStatManager>().TakeDamage(enemyStats.Damage);
        }
    }
}
