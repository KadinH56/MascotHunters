using System.Collections;
using UnityEngine;

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

    Animator animator;

    bool animating = false;


    void Start()
    {
        //hitCircle.GetComponent<SpriteRenderer>().enabled = false;
        //hitCircle.GetComponent<CapsuleCollider>().enabled = false;

        //hitCollider = GetComponent<CapsuleCollider>();
        //hitRenderer = GetComponent<SpriteRenderer>();

        //hitCollider.enabled = false;
        //hitRenderer.enabled = false;

        animator = GetComponent<Animator>();
        StartCoroutine(HammerDamage());
        StartCoroutine(HammerMovement());

        LevelupDescriptions.Add(1, "Get a Strength Hammer");
        levelupDescriptions.Add(2, "Increase Hammer Size");
        weaponLevel = 1;
    }

    private IEnumerator HammerDamage()
    {
        while (true)
        {
            //Debug.Log("Hammer activated!");
            //hitCircle.GetComponent<SpriteRenderer>().enabled = true;
            //hitCircle.GetComponent <CapsuleCollider>().enabled = true;
            //hitCircle.GetComponent<HitCircle>().Damage = CalculateDamage; IDK
            //hitCircle.GetComponent<Rigidbody>().position = pMovement.Facing;
            //hitCountdown--;
            yield return new WaitForSeconds(hitCountdown);
            animating = true;
            animator.SetBool("Hit", true);
            AudioSource.PlayClipAtPoint(hammerHitSound, transform.position);


            //hitCollider.enabled = false;
            //yield return new WaitForSeconds(1f);
            //hitRenderer.enabled = false;
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
        Collider[] enemies = Physics.OverlapSphere(hitRenderer.transform.position, circleRadius * circleRadiusModifier, enemyLayers);
        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitRenderer.transform.position, circleRadius * circleRadiusModifier);
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
                //speedModifier = 0.9f;
                circleRadiusModifier = 1.15f;
                break;
        }
    }
}
