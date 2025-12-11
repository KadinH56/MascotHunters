using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private GameObject menuCamera;
    //[SerializeField] private GameObject mainCamera;
    [SerializeField] private int camManager;
    [SerializeField] private float rotationsPerMinute;


    private void Cam1()
    {
       // menuCamera.SetActive(true);
        //mainCamera.SetActive(false);
    }

    private void Cam2()
    {
        //menuCamera.transform.position = Vector3.MoveTowards(transform.position, mainCamera.transform.position,
            //3* Time.deltaTime);
        //if(menuCamera.transform.position == mainCamera.transform.position)
        {
            //menuCamera.SetActive(false);
            //mainCamera.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (camManager == 0)
        {
            menuCamera.transform.Rotate(0, 6f * rotationsPerMinute * Time.deltaTime, 0);
        }
    }
}
