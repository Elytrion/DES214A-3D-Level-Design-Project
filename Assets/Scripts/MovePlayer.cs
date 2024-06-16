using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Transform StartPos;
    public Transform EndPos;
    [Range(0.0f, 1.0f)]
    public float LerpSpeed = 1.0f;

    bool isMoving = false;

    private GameObject _playerGO;
    private float lerpPercentage = 0.0f;

    private bool _isPlayerInTrigger = false;
    private DisplayTextOnTrigger _textTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _textTrigger = GetComponent<DisplayTextOnTrigger>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerInTrigger && Input.GetKeyDown(KeyCode.E))
        {
            isMoving = true;
            _playerGO.GetComponent<CharacterController>().enabled = false;
            _isPlayerInTrigger = false;
            if (_textTrigger != null)
            {
                _textTrigger.ForceExit();
            }
        }

        if (isMoving)
        {
            lerpPercentage += Time.deltaTime;
            _playerGO.transform.position = Vector3.Lerp(StartPos.position, EndPos.position, LerpSpeed * lerpPercentage);
            if (Vector3.Distance(_playerGO.transform.position, EndPos.position) < 0.1f)
            {
                _playerGO.GetComponent<CharacterController>().enabled = true;
                isMoving = false;
                lerpPercentage = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isMoving == false)
        {
            _isPlayerInTrigger = true;
            _playerGO = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerInTrigger = false;
            _playerGO = null;
        }
    }
}
