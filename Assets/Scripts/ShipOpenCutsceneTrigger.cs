using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipOpenCutsceneTrigger : MonoBehaviour
{
    private bool _playerInTrigger = false;
    private bool _cutsceneTriggered = false;
    private bool _cutsceneFinished = false;

    public Transform RedButton;
    public GameObject[] ObjectsToDisable;
    public GameObject[] ObjectsToEnable;

    public DisplayTextOnTrigger _textTrigger;

    public VibrateObject vibrationScript;

    public RotateOpenDoor[] doorsToOpen;
    public float[] DelayTimings;
    private float _delayTimer;
    private int doorIndex = 0;

    public CinematicCamMovement CutsceneScript;

    // Update is called once per frame
    void Update()
    {
        if (_cutsceneFinished)
            return;
        
        if (_playerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            if (!_cutsceneTriggered)
            {
                _cutsceneTriggered = true;               
                
                RedButton.position = new Vector3(RedButton.position.x, RedButton.position.y - 0.5f, RedButton.position.z);
                _textTrigger.ForceExit();
                foreach (GameObject obj in ObjectsToDisable)
                {
                    obj.SetActive(false);
                }

                CutsceneScript.StartCutscene();
            }      
        }

        if (_cutsceneTriggered)
        {
            if (doorIndex < doorsToOpen.Length)
            {
                _delayTimer += Time.deltaTime;
                if (_delayTimer >= DelayTimings[doorIndex])
                {
                    doorsToOpen[doorIndex].SwingOpen = true;
                    doorsToOpen[doorIndex].ShouldMove = true;
                    doorIndex++;
                    _delayTimer = 0.0f;
                }
            }

            if (CutsceneScript.HasRun)
            {
                foreach (GameObject obj in ObjectsToEnable)
                {
                    obj.SetActive(true);
                }
                vibrationScript.IsVibrating = true;
                doorIndex = 0;
                _cutsceneFinished = true;
                _cutsceneTriggered = false;
                _playerInTrigger = false;
                _delayTimer = 0.0f;
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerInTrigger = false;
        }
    }
        
}
