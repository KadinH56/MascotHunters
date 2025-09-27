using UnityEngine;

public class MovementBuff : PickupBase
{
    [SerializeField] private float buffDuration = 5f;
    [SerializeField] private float buffPower = 1.5f;

    public override void Effect(PlayerStatManager player)
    {
        player.TempPowerupIEnumerator(PlayerStatManager.PLAYER_STATS.MOVEMENT, buffDuration, buffPower);
    }
}
