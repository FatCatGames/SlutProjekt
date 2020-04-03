using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public Camera portalCam;
    private Transform playerCam;
    // Start is called before the first frame update
    void Start()
    {
        playerCam = Camera.main.transform;
        isBehindPortal = PositionBehindPortal(playerCam.position);
    }

    private bool isBehindPortal;
    // Update is called once per frame
    void Update()
    {
        Vector3 camInLocal = transform.worldToLocalMatrix.MultiplyPoint3x4(playerCam.position);
        if(Mathf.Abs(camInLocal.z) > 4) {
            isBehindPortal = PositionBehindPortal(playerCam.position);
            return;
        }
        //player is on other side of the portal this frame
        if (PositionBehindPortal(playerCam.position) != isBehindPortal && Mathf.Abs(camInLocal.x) < 0.5f)
        {
            isBehindPortal = !isBehindPortal;
            MaterialManager.Instance.InvertStencil();
        }
        
        //Matrix4x4 mat = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * playerCam.localToWorldMatrix;
        //linkedPortal.portalCam.transform.SetPositionAndRotation(mat.GetColumn(3), mat.rotation);
    }

    private bool PositionBehindPortal(Vector3 camPosition)
    {
        Vector3 forward = Vector3.Normalize(transform.TransformDirection(Vector3.forward));
        Vector3 toOther = Vector3.Normalize(camPosition - transform.position);

        return Vector3.Dot(forward, toOther) < 0; // player is behind portal
    }
}
