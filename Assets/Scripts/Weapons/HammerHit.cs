using System.Collections;
using UnityEngine;
using static PlayerStatManager;

public class HammerHit : MonoBehaviour
{
    private Stats playerStats;
    [SerializeField] private int baseDamage = 3;
    private int timer = 3;

    void Start()
    {
        playerStats = transform.parent.GetComponent<PlayerStatManager>().PlayerStats;
    }

    void FixedUpdate()
    {
        //timer and Time.DeltaTime seconds countdown goes here

        if(timer >= 0)
        {
            StartCoroutine("HammerDamage");
            timer = 3;
        }
    }

    private IEnumerator HammerDamage()
    {
        Debug.Log("Hammer activated!");
        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().TakeDamage(Mathf.RoundToInt((baseDamage +
                playerStats.DamageModifierAdditive) * playerStats.DamageModifierMultiplicitive));
        }
    }
}
