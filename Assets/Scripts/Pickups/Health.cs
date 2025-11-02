using UnityEngine;

public class Health : PickupBase
{
    [SerializeField] private int healthRegained;

    public override void Effect(PlayerStatManager player)
    {
        player.Healing(healthRegained);
    }
}
