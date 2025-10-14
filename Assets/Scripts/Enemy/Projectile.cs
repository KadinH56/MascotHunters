using System.Collections;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage = 0;

    [SerializeField] private bool playerProj = false;

    public int Damage { get => damage; set => damage = value; }

    [SerializeField] private float despawnTime = 1.5f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(Despawner());
        transform.rotation = Quaternion.Euler(0, -Mathf.Atan2(rb.linearVelocity.z, rb.linearVelocity.x) * Mathf.Rad2Deg, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerProj)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.GetComponent<EnemyScript>().TakeDamage(damage);
                OnKill(true);
                return;
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerStatManager>().TakeDamage(damage);
            OnKill(true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnKill(true);
    }

    public virtual IEnumerator Despawner()
    {
        yield return new WaitForSeconds(despawnTime);
        OnKill(false);
    }

    public virtual void OnKill(bool hitTarget)
    {
        Destroy(gameObject);
    }
}
