using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class UIBuildTextureManager
{
    private static List<UIBuildTextureManager> TextureManagers = new();

    [SerializeField] private Sprite arcadeBuild;
    [SerializeField] private Sprite PCBuild;

    /// <summary>
    /// If we have a spriteRenderer, use this
    /// </summary>
    [SerializeField] private SpriteRenderer spriteRenderer;
}
