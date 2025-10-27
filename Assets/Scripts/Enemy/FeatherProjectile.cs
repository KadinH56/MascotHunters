using UnityEngine;

public class FeatherProjectile : Projectile
{
    [SerializeField] private SpriteRenderer[] renderers;
    [SerializeField] private Sprite[] sprites;
    public new void Start()
    {
        base.Start();

        Sprite sprite = sprites[Random.Range(0, sprites.Length)];

        foreach (var renderer in renderers)
        {
            renderer.sprite = sprite;
        }
    }
}
