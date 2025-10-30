using System.Collections;
using UnityEngine;

public class BatHit : WeaponBase
{
    //private CapsuleCollider hitCollider;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private int numHits;
    [SerializeField] private float delayBetweenHits = 0.5f;

    Animator animator;

    bool animating = false;


    void Start()
    {
        //hitCircle.GetComponent<SpriteRenderer>().enabled = false;
        //hitCircle.GetComponent<CapsuleCollider>().enabled = false;

        //hitCollider = GetComponent<CapsuleCollider>();
        //hitRenderer = GetComponent<SpriteRenderer>();

        //hitCollider.enabled = false;
        //hitRenderer.enabled = false;

        animator = GetComponent<Animator>();

        LevelupDescriptions.Add(1, "Get a Baseball Bat");
        levelupDescriptions.Add(2, "Increase Hammer Size");
        weaponLevel = 1;
    }

    private void OnEnable()
    {

    }

    private IEnumerator BattingTime()
    {
        yield return null;
    }

    public override void LevelUpWeapon()
    {
        weaponLevel++;
        switch (weaponLevel)
        {
            case 2:
                numHits++;
                break;
        }
    }
}
