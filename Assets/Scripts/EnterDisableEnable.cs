using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDisableEnable : MonoBehaviour
{
    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectsToEnable;

    public bool disableOnStart = true;
    public bool requireButtonPress = false;

    private bool waitingForPress = false;
    
    
    void EnableDisableObjects()
    {
        foreach (GameObject obj in ObjectsToDisable)
        {
            if (obj.GetComponent<DisplayTextOnTrigger>() != null)
                obj.GetComponent<DisplayTextOnTrigger>().ForceExit();

            obj.SetActive(false);
        }
        foreach (GameObject obj in ObjectsToEnable)
        {
            obj.SetActive(true);
        }
    }

    private void Start()
    {
        if (disableOnStart)
        {
            foreach (GameObject obj in ObjectsToEnable)
            {
                obj.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (requireButtonPress && waitingForPress)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                EnableDisableObjects();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!requireButtonPress)
                EnableDisableObjects();
            else
                waitingForPress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (requireButtonPress)
                waitingForPress = false;
        }
    }
    
}
