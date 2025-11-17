using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

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

    /// <summary>
    /// If we have an Image, use this
    /// </summary>
    [SerializeField] private Image image;

    /// <summary>
    /// If for whatever reason we have a rawImage...you get the idea
    /// </summary>
    [SerializeField] private RawImage rawImage;

    public UIBuildTextureManager()
    {
        TextureManagers.Add(this);
    }

    ~UIBuildTextureManager()
    {
        TextureManagers.Remove(this);
    }

    public void SetTexture()
    {
        Sprite sprite = GameInformation.IsArcadeBuild ? arcadeBuild : PCBuild;

        if (image != null)
        {
            image.sprite = sprite;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = sprite;
        }

        if (rawImage != null)
        {
            rawImage.texture = sprite.texture;
        }
    }

    public static void SetAllTextures()
    {
        foreach (UIBuildTextureManager textureManager in TextureManagers)
        {
            textureManager.SetTexture();
        }
    }
}
