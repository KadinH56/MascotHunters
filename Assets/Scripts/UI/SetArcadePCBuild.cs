using UnityEngine;

public class SetArcadePCBuild : MonoBehaviour
{
    [SerializeField] private bool isArcadeBuild = false;
    void Awake()
    {
        GameInformation.IsArcadeBuild = isArcadeBuild;
    }
}
