using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOpenDoor : MonoBehaviour
{
    public Transform[] Doors;
    public float[] ClosedAngles;
    public float[] OpenAngles;
    public float[] OpenSpeeds;

    public bool ShouldMove = false;
    public bool SwingOpen = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!ShouldMove)
            return;
        
        if (SwingOpen)
        {
            for (int i = 0; i < Doors.Length; i++)
            {
                Transform Door = Doors[i];
                float OpenAngle = OpenAngles[i];
                float OpenSpeed = OpenSpeeds[i];
                Door.rotation = Quaternion.Slerp(Door.rotation, Quaternion.Euler(0, OpenAngle, 0), Time.deltaTime * OpenSpeed);
                // if the door is close enough to the rotation amount, stop and set
                if (Quaternion.Angle(Door.rotation, Quaternion.Euler(0, OpenAngle, 0)) < 0.1f)
                {
                    Door.rotation = Quaternion.Euler(0, OpenAngle, 0);
                }
            }
        }
        else
        {
            for (int i = 0; i < Doors.Length; i++)
            {
                Transform Door = Doors[i];
                float ClosedAngle = ClosedAngles[i];
                float OpenSpeed = OpenSpeeds[i];
                Door.rotation = Quaternion.Slerp(Door.rotation, Quaternion.Euler(0, ClosedAngle, 0), Time.deltaTime * OpenSpeed);
                // if the door is close enough to the rotation amount, stop and set
                if (Quaternion.Angle(Door.rotation, Quaternion.Euler(0, ClosedAngle, 0)) < 0.1f)
                {
                    Door.rotation = Quaternion.Euler(0, ClosedAngle, 0);
                }
            }
        }
    }

}
