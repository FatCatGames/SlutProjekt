using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] float acceleration, maxSpeed, turnSpeed;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0));
        float velocityZ = Input.GetAxis("Vertical") * acceleration;
        rb.velocity += transform.forward * Mathf.Clamp(velocityZ, -maxSpeed, maxSpeed) * Time.deltaTime;
    }
}
