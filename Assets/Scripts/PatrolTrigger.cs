using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolTrigger : MonoBehaviour
{
    public Patrol[] PatrolScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < PatrolScript.Length; i++)
                PatrolScript[i].StartPatrolling = true;
        }
    }
}
