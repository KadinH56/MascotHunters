using System.Collections;
using UnityEngine;

public class BatHit : WeaponBase
{
    //private CapsuleCollider hitCollider;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int numHits;
    [SerializeField] private GameObject hitRenderer;

    [SerializeField] private float hitCountdown = 5f;
    [SerializeField] private float batRadius = 1f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] private int numHitsLevel2;
    [SerializeField] private float cooldownLevel2;
    [SerializeField] private float cooldownLevel3;
    [SerializeField] private float sizeMultiplierLevel3 = 2f;
    [SerializeField] private AudioClip batSwing;

    Animator animator;

    bool attacking = false;
    int hitsRemaining = 0;

    void Start()
    {
        //hitCircle.GetComponent<SpriteRenderer>().enabled = false;
        //hitCircle.GetComponent<CapsuleCollider>().enabled = false;

        //hitCollider = GetComponent<CapsuleCollider>();
        //hitRenderer = GetComponent<SpriteRenderer>();

        //hitCollider.enabled = false;
        //hitRenderer.enabled = false;

        animator = GetComponent<Animator>();

        weaponLevel = 1;
    }

    private void OnEnable()
    {
        StartCoroutine(BattingTime());
    }

    private IEnumerator BattingTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(hitCountdown);
            hitsRemaining = numHits;

            Hit();

            while (attacking)
            {
                yield return null;
            }
        }
    }

    public override void LevelUpWeapon()
    {
        print("Upgraded");
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                numHits = numHitsLevel2;
                hitCountdown = cooldownLevel2;
                break;
            case 3:
                hitCountdown = cooldownLevel3;
                transform.localScale *= sizeMultiplierLevel3;
                batRadius *= sizeMultiplierLevel3;
                offset.x *= sizeMultiplierLevel3;
                maxDistance *= sizeMultiplierLevel3;
                break;
        }
    }

    public void Hit()
    {
        StartCoroutine(HitEnumerator());
    }

    private IEnumerator HitEnumerator()
    {
        if (hitsRemaining <= 0)
        {
            hitRenderer.SetActive(false); //This one goes out to you Ian
            attacking = false;
            yield break;
        }
        hitRenderer.SetActive(true);

        attacking = true;
        GameObject target = null;
        float distance = maxDistance;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < distance)
            {
                target = enemy;
                distance = Vector3.Distance(transform.position, enemy.transform.position);
            }
        }

        Vector3 velo = Vector3.zero;

        if (target == null)
        {
            velo = pMovement.Facing.normalized;
        }
        else
        {
            velo = target.transform.position - transform.position;
            velo.Normalize();
        }

        float rot = Mathf.Atan2(-velo.z, velo.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, rot, 0);

        hitsRemaining--;

        yield return new WaitForSeconds(0.25f);
        SFX.SpawnClip(batSwing, transform.position);

        hitRenderer.GetComponent<SpriteRenderer>().flipY = !hitRenderer.GetComponent<SpriteRenderer>().flipY;
        animator.Play("BatAttack");
    }

    public void Attack()
    {
        Collider[] enemies = Physics.OverlapSphere(hitRenderer.transform.position + (transform.rotation * offset), batRadius, enemyLayers);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitRenderer.transform.position + (transform.rotation * offset), batRadius);
    }
}
