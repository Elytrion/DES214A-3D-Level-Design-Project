using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCutscene : MonoBehaviour
{
    public Transform[] Positions;
    public float Speed = 1.0f;
    public float TurningSpeed = 2.0f;
    public float TurningDistance = 5.0f;
    public bool StartCutscene = false;

    private int _currentPos = 0;
    
    

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if (StartCutscene)
        {
            MoveAndRotateToPosition();
        }
    }

    void MoveAndRotateToPosition()
    {
        if (Vector3.Distance(transform.position, Positions[_currentPos].position) > 1.0f)
        {
            transform.position = Vector3.MoveTowards(transform.position, Positions[_currentPos].position, Speed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.position, Positions[_currentPos].position) < TurningDistance)
        {
            //rotate to next position
            if ((_currentPos + 1) < Positions.Length)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Positions[_currentPos + 1].rotation, TurningSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.eulerAngles, Positions[_currentPos + 1].eulerAngles) < 5.0f)
                {
                    transform.eulerAngles = Positions[_currentPos + 1].eulerAngles;
                    transform.position = Positions[_currentPos].position;
                    _currentPos++;
                }
            }
        }
    }
}
