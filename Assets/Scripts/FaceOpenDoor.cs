using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceOpenDoor : MonoBehaviour
{
    public RotateOpenDoor OpenDoorScript;
    public Transform LookAtTarget;
    public float RotationSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
       if (OpenDoorScript.SwingOpen)
        {
            //lerp to look at target
            transform.rotation = Quaternion.Lerp(transform.rotation, LookAtTarget.rotation, Time.deltaTime * RotationSpeed);
        }
    }
}
