using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckCutsceneTrigger : MonoBehaviour
{

    public TruckCutscene cutsceneToTrigger;
    public GameObject lightToEnable;
    public float Timer = 6.0f;
    private float timer = 0.0f;
    private bool hasBeenTriggered = false;
    
    void Start()
    {
        lightToEnable.SetActive(false);
    }

    void Update()
    {
        if (hasBeenTriggered)
        {
            timer += Time.deltaTime;
            if (timer >= Timer)
            {
                lightToEnable.SetActive(true);
                // disable this gameobject
                gameObject.SetActive(false);
            }
        }
    }

    // Move the player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true;
            cutsceneToTrigger.StartCutscene = true;
        }
    }
}
