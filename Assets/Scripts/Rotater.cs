using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public float rotationSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around its local Y-axis
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // if the objects rotation around it's axis exceeds 360, reset it
        if (transform.rotation.eulerAngles.y > 360)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
        }
    }
}
