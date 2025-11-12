using System.Collections;
using UnityEngine;

public class CorkGunWeapon : WeaponBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float initShootTime = 1.5f;
    [SerializeField] private float initShootSpeed = 16f;
    [SerializeField] private float explosionSize = 0f;

    private float speedModifier = 1f;
    private float timeModifier = 1f;

    [SerializeField] private float maxDistance = 32;

    [SerializeField] private AudioClip shootSound;

    [SerializeField] private float level2Speed = 0.75f;
    [SerializeField] private float level2ExplosionSize = 8f;
    [SerializeField] private float level3Speed = 0.5f;
    [SerializeField] private float level3ExplosionSize = 16f;

    private void Start()
    {
        //StartCoroutine(Shoot());

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
                if(Vector3.Distance(transform.position, enemy.transform.position) < distance)
                {
                    target = enemy;
                    distance = Vector3.Distance(transform.position, enemy.transform.position);
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
            proj.GetComponent<CorkGunProjectile>().ExplosionSize = explosionSize;
        }
    }

    public override void LevelUpWeapon()
    {
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                timeModifier = level2Speed;
                explosionSize = level3ExplosionSize;
                break;
            case 3:
                timeModifier = level3Speed;
                explosionSize = level3ExplosionSize;
                break;
        }

        StopAllCoroutines();
        StartCoroutine(Shoot());
    }
}
