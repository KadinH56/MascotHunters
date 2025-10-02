using System.Collections;
using UnityEngine;
using static PlayerStatManager;

public class HammerHit : MonoBehaviour
{
    private Stats playerStats;
    [SerializeField] private int baseDamage = 3;
    [SerializeField] private int hitCountdown = 15;

    void Start()
    {
        playerStats = transform.parent.GetComponent<PlayerStatManager>().PlayerStats;
    }

    void FixedUpdate()
    {
        //timer and Time.DeltaTime seconds countdown goes here

        if(hitCountdown == 0)
        {
            StartCoroutine("HammerDamage");
            hitCountdown = 15;
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
