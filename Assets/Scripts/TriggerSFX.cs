using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSFX : MonoBehaviour
{
    public AudioSource sfxSource;
    public bool TriggerOnce = false;
    public bool PressEToTrigger = false;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PressEToTrigger)
                return;
            if (TriggerOnce && triggered)
                return;

            sfxSource.Play();
            if (TriggerOnce)
                triggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PressEToTrigger)
        {
            if (TriggerOnce && triggered)
                return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                sfxSource.Play();
                if (TriggerOnce)
                    triggered = true;
            }
        }
    }
}
