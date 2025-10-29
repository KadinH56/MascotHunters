using System.Collections;
using UnityEngine;

public class PermenentBlood : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(120f);
        Destroy(gameObject);
    }
}
