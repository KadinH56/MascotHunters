using System.Collections;
using UnityEngine;

public class CorkGunProjectile : Projectile
{
    [SerializeField] private float explosionDamageMultiplier;

    [SerializeField] private GameObject[] sprites;
    [SerializeField] private Collider projectileCollider;
    [SerializeField] private Collider explosionCollider;
    [SerializeField] private AudioClip bulletHitSound;

    public override void OnKill(bool hitTarget)
    {
        base.OnKill(hitTarget);
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
    }

    private IEnumerator DestroyMe()
    {
        yield return new WaitForFixedUpdate();
        Destroy(gameObject);
    }
}
