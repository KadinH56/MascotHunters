using System.Collections;
using UnityEngine;

public class PermenentBlood : MonoBehaviour
{
    [SerializeField] private float timeInSeconds = 120f;
    [SerializeField] private bool dealsDamage;

    private int damage = 0;
    private int numDamagesLeft = 3;

    public int Damage { get => damage; set => damage = value; }

    private void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(timeInSeconds);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(dealsDamage);
        if (!dealsDamage)
        {
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyScript>().TakeDamage(Damage);
        }
    }
}
