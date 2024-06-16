using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    private Light _light;
    private bool _isFlickering = false;

    public Vector2 LightOnTimeRange;
    public Vector2 LightOnIntensityRange;
    public Vector2 LightOffTimeRange;

    public GameObject EmissiveObject;
    public Material EmissiveMaterial;
    public Material NonemissiveMaterial;

    public bool LightShouldTurnOffFully = true;
    
    private float _timer = 1.0f;
    private bool lightOn = true;

    [Range(1, 50)]
    public int smoothing = 5;

    Queue<float> smoothQueue;
    float lastSum = 0;

    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        _light = GetComponent<Light>();
        StartFlickering();
    }

    void Update()
    {
        if (_isFlickering)
        {
            if (lightOn)
            {
                _timer -= Time.deltaTime;
                RapidFlicker();
                if (_timer <= 0)
                {
                    if (LightShouldTurnOffFully)
                    {
                        _timer = Random.Range(LightOffTimeRange.x, LightOffTimeRange.y);
                        lightOn = false;
                    }
                    else
                    {
                        _timer = Random.Range(LightOnTimeRange.x, LightOnTimeRange.y);
                    }
                }
            }
            else
            {
                _timer -= Time.deltaTime;
                _light.intensity = 0;
                if (_timer <= 0)
                {
                    _timer = Random.Range(LightOnTimeRange.x, LightOnTimeRange.y);
                    lightOn = true;
                }
            }
        }

        if (EmissiveObject == null)
            return;
        
        if (lightOn)
        {
            EmissiveObject.GetComponent<Renderer>().material = EmissiveMaterial;
        }
        else
        {
            EmissiveObject.GetComponent<Renderer>().material = NonemissiveMaterial;
        }
    }

    public void StartFlickering()
    {
        _isFlickering = true;
        _timer = Random.Range(LightOnTimeRange.x, LightOnTimeRange.y);
    }

    public void StopFlickering()
    {
        _isFlickering = false;
        _light.intensity = 0;
    }

    void RapidFlicker()
    {
        // pop off an item if too big
        while (smoothQueue.Count >= smoothing)
        {
            lastSum -= smoothQueue.Dequeue();
        }

        // Generate random new item, calculate new average
        float newVal = Random.Range(LightOnIntensityRange.x, LightOnIntensityRange.y);
        smoothQueue.Enqueue(newVal);
        lastSum += newVal;

        // Calculate new smoothed average
        _light.intensity = lastSum / (float)smoothQueue.Count;
    }

}
