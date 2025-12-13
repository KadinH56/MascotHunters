using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CorkGunProjectile : Projectile
{
    [SerializeField] private float explosionDamageMultiplier;

    [SerializeField] private GameObject[] sprites;
    [SerializeField] private Collider projectileCollider;
    [SerializeField] private Collider explosionCollider;
    [SerializeField] private AudioClip bulletHitSound;

    [SerializeField] private float explosionSize = 0f;

    [SerializeField] private LayerMask enemyLayers;

    [SerializeField] private bool explode = false;

    public float ExplosionSize { get => explosionSize; set => explosionSize = value; }

    public override void OnKill(bool hitTarget)
    {
        explode = true;

        AudioSource.PlayClipAtPoint(bulletHitSound, transform.position);

        //Lookat later when upgrade stuff
        //if (hitTarget)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //Damage = Mathf.RoundToInt(Damage * explosionDamageMultiplier);
        //foreach (GameObject sprite in sprites)
        //{
        //    sprite.SetActive(false);
        //}
        //projectileCollider.enabled = false;
        //explosionCollider.enabled = true;

        //StartCoroutine(DestroyMe());
        //No past Lucas, your code sucks

        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionSize, enemyLayers);
        foreach(GameObject sprite in sprites)
        {
            sprite.SetActive(false);
        }

        foreach (Collider enemy in enemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.gameObject.GetComponent<EnemyScript>().TakeDamage(Damage / 2);
            }
        }
        StartCoroutine(DestroyMe());
    }

    private IEnumerator DestroyMe()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}
