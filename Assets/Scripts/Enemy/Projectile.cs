using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage = 0;

    public int Damage { get => damage; set => damage = value; }

    [SerializeField] private float despawnTime = 1.5f;

    private void Start()
    {
        StartCoroutine(Despawner());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent.GetComponent<PlayerStatManager>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private IEnumerator Despawner()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
