using System.Collections;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HammerHit : WeaponBase
{
    [SerializeField] private int hitCountdown = 15;
    //[SerializeField] private bool isHitting = false;
    //[SerializeField] private GameObject hitCircle;

    [SerializeField] private float circleDistance;
    [SerializeField] private float circleRadius = 3f;

    private float circleDistanceModifier = 1f;
    private float circleRadiusModifier = 1f;

    //private CapsuleCollider hitCollider;
    [SerializeField] private SpriteRenderer hitRenderer;
    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private AudioClip hammerHitSound;
    [SerializeField] private GameObject hitEffect;
    [SerializeField] private GameObject hitDamageEffect;

    [SerializeField] private Vector3 hitOffset = Vector3.zero;

    [SerializeField] private int maxHit = 40;

    [SerializeField] private float speedModifier = 1f;
    [SerializeField] private float speedLevel2Modifier = 0.9f;
    [SerializeField] private float speedLevel3Modifier = 0.75f;
    [SerializeField] private float level2SizeModifier = 1.5f;
    private Collider[] hits;

    Animator animator;

    bool animating = false;


    void Start()
    {
        animator = GetComponent<Animator>();
        hits = new Collider[maxHit];
        weaponLevel = 1;
    }

    private void OnEnable()
    {
        StartCoroutine(HammerDamage());
        StartCoroutine(HammerMovement());
    }

    private IEnumerator HammerDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(hitCountdown * speedModifier);
            animating = true;
            animator.speed = speedModifier;
            animator.SetBool("Hit", true);
        }
    }

    private IEnumerator HammerMovement()
    {
        while (true)
        {
            if (animating)
            {
                hitRenderer.transform.localPosition = new(circleDistance * circleDistanceModifier, 0);
                transform.rotation = Quaternion.Euler(0, Mathf.Atan2(-pMovement.Facing.z, pMovement.Facing.x) * Mathf.Rad2Deg, 0);
            }
            yield return null;
        }
    }

    public void HitEvent()
    {
        animator.SetBool("Hit", false);
        animating = false;
        //Collider[] enemies = Physics.OverlapSphere(hitRenderer.transform.position + hitOffset, circleRadius * circleRadiusModifier, enemyLayers);
        //foreach (Collider enemy in enemies)
        //{
        //    if (enemy.gameObject.CompareTag("Enemy"))
        //    {
        //        enemy.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
        //    }
        //}

        int enemies = Physics.OverlapSphereNonAlloc(hitRenderer.transform.position + (transform.rotation * hitOffset), circleRadius * circleRadiusModifier, hits, enemyLayers);
        for (int i = 0; i < enemies; i++)
        {
            if (hits[i].gameObject.CompareTag("Enemy"))
            {
                hits[i].gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
            }
        }

        if (weaponLevel < 3)
        {
            GameObject effect = Instantiate(hitEffect, hitRenderer.transform.position + (Vector3.down * 1.40f), Quaternion.identity);
            effect.transform.localScale *= circleRadiusModifier;
        }
        else
        {
            GameObject effect = Instantiate(hitDamageEffect, hitRenderer.transform.position + (Vector3.down * 1.40f), Quaternion.identity);
            effect.transform.localScale *= circleRadiusModifier;
            effect.GetComponent<PermenentBlood>().Damage = CalculateDamage() / 2;
        }
        SFX.SpawnClip(hammerHitSound, transform.position);
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitRenderer.transform.position + hitOffset, circleRadius * circleRadiusModifier);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("Hit");
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
    //    }
    //}

    public override void LevelUpWeapon()
    {
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                speedModifier = speedLevel2Modifier;
                circleRadiusModifier = level2SizeModifier;
                transform.localScale = 6 * circleRadiusModifier * Vector3.one;
                break;
            case 3:
                speedModifier = speedLevel3Modifier;
                break;
        }
    }
}
