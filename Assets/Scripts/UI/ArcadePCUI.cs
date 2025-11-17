using UnityEngine;
using System.Collections.Generic;

public class ArcadePCUI : MonoBehaviour
{
    [SerializeField] private List<UIBuildTextureManager> UIBuildTextureManagers = new();

    private void Start()
    {
        UIBuildTextureManager.SetAllTextures();
    }
}
