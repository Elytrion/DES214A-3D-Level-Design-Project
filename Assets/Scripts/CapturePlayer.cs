using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
    
public class CapturePlayer : MonoBehaviour
{
    public Transform Checkpoint;
    public TMP_Text UITextElement;

    private float _timer = 3.0f;
    private bool _caughtPlayer = false;

    private GameObject _playerGO;

    private void Update()
    {
        if (_caughtPlayer)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0.0f)
            {
                _playerGO.transform.position = Checkpoint.position;
                _playerGO.GetComponent<CharacterController>().enabled = true;
                _caughtPlayer = false;
                UITextElement.text = "";
                _timer = 3.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Checkpoint != null && UITextElement != null)
            {
                _playerGO = other.gameObject;
                _caughtPlayer = true;
                UITextElement.text = "You have been captured!";
                _playerGO.GetComponent<CharacterController>().enabled = false;
            }
        }
    }
}
