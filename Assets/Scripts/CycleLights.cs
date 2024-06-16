using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleLights : MonoBehaviour
{
    public Light pointLight;
    public float cycleSpeed = 1f;
    public float hueSpeed = 0.2f;

    private Color currentColor;
    private float currentHue = 0f;

    private void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
    }

    private void Update()
    {
        currentHue += hueSpeed * Time.deltaTime;
        currentHue %= 1f;

        currentColor = Color.HSVToRGB(currentHue, 1f, 1f);
        pointLight.color = currentColor;
    }
}

