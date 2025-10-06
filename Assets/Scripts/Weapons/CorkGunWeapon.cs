using System.Collections;
using UnityEngine;

public class CorkGunWeapon : WeaponBase
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shootTime;

    [SerializeField] private float shootSpeed;
    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootTime);

            GameObject proj = Instantiate(projectile, transform.position, transform.rotation);

            proj.GetComponent<CorkGunProjectile>().Damage = CalculateDamage();
            proj.GetComponent<Rigidbody>().linearVelocity = shootSpeed * pMovement.Facing;
        }
    }
}
