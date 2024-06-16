using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public Transform[] PatrolPoints;
    public float MovingSpeed = 1.0f;
    public float MovingDistance = 2.0f;
    public float TurningSpeed = 2.0f;
    public float TurningDistance = 5.0f;

    public int[] DelayPoints;
    public float DelayTime;

    private float _delayTimer = 0.0f;

    private int _currentPos = 0;

    public bool StartPatrolling = false;

    public bool DoOnce = false;

    // Update is called once per frame
    void Update()
    {
        if (DoOnce && _currentPos == PatrolPoints.Length - 1)
        {
            return;
        }

        if (StartPatrolling)
            MoveAndRotateTolocalPosition();
    }

    void MoveAndRotateTolocalPosition()
    {
        if (Vector3.Distance(transform.localPosition , PatrolPoints[_currentPos].localPosition ) > MovingDistance)
        {
            transform.localPosition  = Vector3.MoveTowards(transform.localPosition , PatrolPoints[_currentPos].localPosition , MovingSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(transform.localPosition , PatrolPoints[_currentPos].localPosition ) < TurningDistance)
        {
            //rotate to next localPosition 
            int nextPosIndex = (_currentPos + 1) % PatrolPoints.Length;

            foreach (int index in DelayPoints)
            {
                if (index == _currentPos)
                {
                    _delayTimer += Time.deltaTime;
                    if (_delayTimer >= DelayTime)
                        break;
                    else
                        return;
                }
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, PatrolPoints[nextPosIndex].localRotation, TurningSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.eulerAngles, PatrolPoints[nextPosIndex].eulerAngles) < 1.0f)
            {  
                transform.eulerAngles = PatrolPoints[nextPosIndex].eulerAngles;
                transform.localPosition  = PatrolPoints[_currentPos].localPosition ;
                _currentPos = nextPosIndex;
                _delayTimer = 0.0f;
            }
        }

    }
}
