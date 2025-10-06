using System.Collections;
using UnityEngine;

public class HammerHit : WeaponBase
{
    [SerializeField] private int hitCountdown = 15;
    //[SerializeField] private bool isHitting = false;
    //[SerializeField] private GameObject hitCircle;

    [SerializeField] private float circleDistance;
    [SerializeField] private float circleRadius = 3f;

    //private CapsuleCollider hitCollider;
    [SerializeField] private SpriteRenderer hitRenderer;
    [SerializeField] private LayerMask enemyLayers;


    void Start()
    {
        //hitCircle.GetComponent<SpriteRenderer>().enabled = false;
        //hitCircle.GetComponent<CapsuleCollider>().enabled = false;

        //hitCollider = GetComponent<CapsuleCollider>();
        //hitRenderer = GetComponent<SpriteRenderer>();

        //hitCollider.enabled = false;
        hitRenderer.enabled = false;

        StartCoroutine(HammerDamage());
    }

    private IEnumerator HammerDamage()
    {
        while (true)
        {
            //Debug.Log("Hammer activated!");
            //hitCircle.GetComponent<SpriteRenderer>().enabled = true;
            //hitCircle.GetComponent <CapsuleCollider>().enabled = true;
            ////hitCircle.GetComponent<HitCircle>().Damage = CalculateDamage; IDK
            //hitCircle.GetComponent<Rigidbody>().position = pMovement.Facing;
            //hitCountdown--;

            yield return new WaitForSeconds(hitCountdown);
            transform.rotation = Quaternion.Euler(0, Mathf.Atan2(-pMovement.Facing.z, pMovement.Facing.x) * Mathf.Rad2Deg, 0);

            //print("Hit");
            //hitCollider.enabled = true;
            hitRenderer.enabled = true;
            //yield return new WaitForSeconds(0.1f);
            Collider[] enemies = Physics.OverlapSphere(hitRenderer.transform.position, circleRadius, enemyLayers);
            yield return new WaitForFixedUpdate();
            foreach (Collider enemy in enemies)
            {
                if (enemy.gameObject.CompareTag("Enemy"))
                {
                    enemy.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
                }
            }
            //hitCollider.enabled = false;
            //yield return new WaitForSeconds(1f);
            hitRenderer.enabled = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print("Hit");
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        other.gameObject.GetComponent<EnemyScript>().TakeDamage(CalculateDamage());
    //    }
    //}
}
