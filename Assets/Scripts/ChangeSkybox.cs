using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSkybox : MonoBehaviour
{
    public Camera mainCamera;
    public Color newSkyColor;
    public Light dirLight;
    public Color newLightColor;
    public Color newFogColor;

    public float lightIntensity;
    public bool fogEnabled;
    public Vector3 lightRotation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCamera.clearFlags = CameraClearFlags.SolidColor;
            mainCamera.backgroundColor = newSkyColor;
            
            dirLight.color = newLightColor;
            dirLight.intensity = lightIntensity;
            dirLight.transform.rotation = Quaternion.Euler(lightRotation.x, lightRotation.y, lightRotation.z);

            RenderSettings.fog = fogEnabled;
            if (fogEnabled)
                RenderSettings.fogColor = newFogColor;
        }
    }
}
