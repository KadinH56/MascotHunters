using System.Collections;
using UnityEngine;

public class CorkGunWeapon : WeaponBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float initShootTime = 1.5f;
    [SerializeField] private float initShootSpeed = 16f;

    private float speedModifier = 1f;
    private float timeModifier = 1f;

    [SerializeField] private AudioClip shootSound;

    private void Start()
    {
        //StartCoroutine(Shoot());

        LevelupDescriptions.Add(1, "Get a Cork Gun");
        LevelupDescriptions.Add(2, "Reduce Reload Time (100% -> 90%)");
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

            proj.GetComponent<CorkGunProjectile>().Damage = CalculateDamage();
            proj.GetComponent<Rigidbody>().linearVelocity = initShootSpeed * speedModifier * pMovement.Facing.normalized;
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
