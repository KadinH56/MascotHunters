using System.Collections;
using UnityEngine;

public class CorkGunWeapon : WeaponBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float initShootTime = 1.5f;
    [SerializeField] private float initShootSpeed = 16f;

    private float speedModifier = 1f;
    private float timeModifier = 1f;

    [SerializeField] private float maxDistance = 32;

    [SerializeField] private AudioClip shootSound;

    private void Start()
    {
        //StartCoroutine(Shoot());

        LevelupDescriptions.Add(1, "Get a Cork Gun");
        LevelupDescriptions.Add(2, "Reduce Reload Time (100% -> 75%)");
        weaponLevel = 1;
    }

    private void OnEnable()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(initShootTime * timeModifier);

            GameObject proj = Instantiate(projectile, transform.position, transform.rotation);
            AudioSource.PlayClipAtPoint(shootSound, transform.position);

            GameObject target = null;
            float distance = maxDistance;
            foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if(Vector2.Distance(transform.position, enemy.transform.position) < distance)
                {
                    target = enemy;
                    distance = Vector2.Distance(transform.position, enemy.transform.position);
                }
            }

            Vector3 velo = Vector3.zero;

            proj.GetComponent<CorkGunProjectile>().Damage = CalculateDamage();
            if(target == null)
            {
                velo = pMovement.Facing.normalized;
            }
            else
            {
                velo = target.transform.position - transform.position;
                velo.Normalize();
            }

            proj.GetComponent<Rigidbody>().linearVelocity = initShootSpeed * speedModifier * velo;
        }
    }

    public override void LevelUpWeapon()
    {
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                timeModifier = 0.75f;
                break;
        }

        StopAllCoroutines();
        StartCoroutine(Shoot());
    }
}
