using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrateObject : MonoBehaviour
{
    public GameObject[] objectsToVibrate;
    public float vibrationDuration = 0.1f;
    public float delayBetweenVibrations = 0.5f;
    public float vibrationStrength = 0.1f;
    public float vibrationFrequency = 5.0f;

    public bool IsVibrating = false;

    private float timer = 0.0f;
    private int currentObjectIndex = 0;

    private Vector3[] originalPositions;
    
    void Start()
    {
        originalPositions = new Vector3[objectsToVibrate.Length];
        for (int i = 0; i < objectsToVibrate.Length; i++)
        {
            originalPositions[i] = objectsToVibrate[i].transform.position;
        }
    }

    void Update()
    {
        if (IsVibrating)
        {
            timer += Time.deltaTime;
            if (timer >= delayBetweenVibrations)
            {
                timer = 0.0f;
                if (currentObjectIndex < objectsToVibrate.Length)
                {
                    currentObjectIndex++;
                }
            }

            for (int i = 0; i < currentObjectIndex; i++)
            {
                if (objectsToVibrate[i] != null)
                {
                    Vibrate(objectsToVibrate[i], originalPositions[i], vibrationStrength);
                }
            }
        }
    }

    void Vibrate(GameObject obj, Vector3 originalPos, float strength)
    {
        obj.transform.position = originalPos + strength * new Vector3(
                Mathf.PerlinNoise(vibrationFrequency * Time.time, 1),
                Mathf.PerlinNoise(vibrationFrequency * Time.time, 2),
                Mathf.PerlinNoise(vibrationFrequency * Time.time, 3));
    }
}
