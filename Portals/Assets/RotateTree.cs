using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTree : MonoBehaviour
{
    public Transform mainCam;

    private void Start()
    {
        mainCam = Camera.main.transform;
    }

    void Update()
    {
        Vector3 dirToCam = mainCam.position - transform.position;
        dirToCam.y = 0;
        transform.rotation = Quaternion.LookRotation(dirToCam, Vector3.up);
    }
}
