using System.Collections;
using UnityEngine;
using static PlayerStatManager;

public class HammerHit : WeaponBase
{
    [SerializeField] private int hitCountdown = 15;
    [SerializeField] private bool isHitting = false;
    [SerializeField] private GameObject hitCircle;

    void Start()
    {
        hitCircle.GetComponent<SpriteRenderer>().enabled = false;
        hitCircle.GetComponent<CapsuleCollider>().enabled = false;

        StartCoroutine(HammerDamage());
    }

    private IEnumerator HammerDamage()
    {
        yield return new WaitForSeconds(hitCountdown);

        while (isHitting == true)
        {
            Debug.Log("Hammer activated!");
            hitCircle.GetComponent<SpriteRenderer>().enabled = true;
            hitCircle.GetComponent <CapsuleCollider>().enabled = true;
            //hitCircle.GetComponent<HitCircle>().Damage = CalculateDamage; IDK
            hitCircle.GetComponent<Rigidbody>().position = pMovement.Facing;
            hitCountdown--;
        }
    }
}
