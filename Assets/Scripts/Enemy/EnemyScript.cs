using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements.Experimental;

/// <summary>
/// Simple enemy AI
/// </summary>
public class EnemyScript : MonoBehaviour
{
    [SerializeField] protected Stats enemyStats;

    [SerializeField] private float distanceFromPlayer = 0f;
    //[SerializeField] private bool isBoss = false;
    protected NavMeshAgent agent;
    [SerializeField] protected float projectileVelocity = 0f;
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float shootTimer;

    [SerializeField] private bool isMelee = true;
    [SerializeField] private float meleeTime = 5f;

    [SerializeField] private Collider meleeBox;

    [SerializeField] private int cost = 1;
    [SerializeField] private int startingWave = 1;
    [SerializeField] private EnemyHealthBar healthBar;

    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    [SerializeField] private AudioClip enemyDeath;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private GameObject bloodPrefab;
    [SerializeField] private GameObject permanentBloodPrefab;

    private bool dead = false;

    /// <summary>
    /// Bigger numbers mean less likely to drop an item
    /// </summary>
    [SerializeField] private int itemDropChance = 100;

    //[SerializeField] private float size = 2f;

    protected Coroutine shootCoroutine;

    protected PlayerMovement target;

    public int Cost { get => cost; set => cost = value; }
    public int StartingWave { get => startingWave; }

    //public float Size { get => size; set => size = value; }

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
            animator.SetBool("Attack", true);
        }

        if(healthBar != null)
        {
            healthBar.MaxHealth = enemyStats.MaxHealth;
        }
    }

    private void FixedUpdate()
    {
        EnemyAI();
    }

    public virtual void EnemyAI()
    {
        if (target == null || !target.gameObject.activeSelf)
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

        agent.SetDestination(target.transform.position);

        if(shootCoroutine == null && projectile != null)
        {
            shootCoroutine = StartCoroutine(Shoot());
        }
    }

    /// <summary>
    /// Finds a target
    /// </summary>
    protected void FindTarget()
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

    public virtual void TakeDamage(int damage)
    {
        enemyStats.Health -= damage;

        if (healthBar != null)
        {
            healthBar.UpdateHealthbar(enemyStats.Health);
        }

        if (enemyStats.Health <= 0)
        {
            Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            Instantiate(permanentBloodPrefab, transform.position + Vector3.down, Quaternion.Euler(90,0,0));
            KillEnemy();
            return;
        }

        StartCoroutine(HitFX());
    }

    public virtual void KillEnemy()
    {
        if (dead)
        {
            return;
        }
        SFX.SpawnClip(enemyDeath, transform.position);
        dead = true;
        DropItem();
        AudioSource.PlayClipAtPoint(enemyDeath, transform.position, 1.0f);
        //FindFirstObjectByType<EnemyWaveBar>().ApplyEnemyCount();
        FindFirstObjectByType<TopBar>().EnemyDied();
        Destroy(gameObject);
    }

    private void DropItem()
    {
        if(Random.Range(0, itemDropChance) != 0)
        {
            return;
        }

        Object[] items = Resources.LoadAll("ItemDrops", typeof(GameObject));

        GameObject item = (GameObject)items[Random.Range(0, items.Length)];

        Instantiate(item, transform.position, Quaternion.identity);
    }

    public virtual IEnumerator Shoot()
    {
        //Projectile code
        Vector3 velocity = target.transform.position - transform.position;
        velocity.Normalize();
        GameObject proj = Instantiate(projectile, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody>().linearVelocity = velocity * projectileVelocity;
        proj.GetComponent<Projectile>().Damage = enemyStats.Damage;

        yield return new WaitForSeconds(shootTimer);
        shootCoroutine = null;
    }

    public virtual IEnumerator MeleeAttack()
    {
        while (true)
        {
            yield return new WaitForSeconds(meleeTime);
            meleeBox.enabled = true;
            yield return new WaitForFixedUpdate();
            meleeBox.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerHurt"))
        {
            other.transform.parent.GetComponent<PlayerStatManager>().TakeDamage(enemyStats.Damage);
        }
    }

    private IEnumerator HitFX()
    {
        SFX.SpawnClip(enemyHit, transform.position);
        spriteRenderer.material.SetFloat("_HitFlash", 1);
        Instantiate(bloodPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material.SetFloat("_HitFlash", 0);
    }

    /// <summary>
    /// Upgrades the enemy
    /// </summary>
    public void Upgrade(string stat, float amount)
    {
        switch (stat)
        {
            case "Damage":
                enemyStats.DamageModifierMultiplicitive += amount;
                break;
            case "Health":
                enemyStats.HealthModifierMultiplicitive += amount;
                enemyStats.Health = enemyStats.MaxHealth;
                break;
            case "Speed":
                enemyStats.MovementModifierMultiplicitive += amount;
                break;
        }
    }

    public void UpgradeMult(string stat, float amount)
    {
        switch (stat)
        {
            case "Damage":
                enemyStats.DamageModifierMultiplicitive *= amount;
                break;
            case "Health":
                enemyStats.HealthModifierMultiplicitive *= amount;
                enemyStats.Health = enemyStats.MaxHealth;
                break;
            case "Speed":
                enemyStats.MovementModifierMultiplicitive *= amount;
                break;
        }
    }
}
