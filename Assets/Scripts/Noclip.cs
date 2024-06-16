using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noclip : MonoBehaviour
{
    private Transform _playerTransform;
    private CharacterController _playerCC;
    public float BaseNoClipSpeed = 20f;
    public float MaxNoClipSpeed = 40f;
    public bool NoClipEnabled = false;

    private float NoClipSpeed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GetComponent<Transform>();
        _playerCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            NoClipEnabled = !NoClipEnabled;
            _playerCC.detectCollisions = !NoClipEnabled;
            _playerCC.enabled = !NoClipEnabled;
        }

        NoClipSpeed = BaseNoClipSpeed;
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            NoClipSpeed = MaxNoClipSpeed;
        }

        if (NoClipEnabled)
        {
            MovePlayerTransform();
        }
    }

    void MovePlayerTransform()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _playerTransform.position += _playerTransform.forward * NoClipSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            _playerTransform.position -= _playerTransform.forward * NoClipSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            _playerTransform.position -= _playerTransform.right * NoClipSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            _playerTransform.position += _playerTransform.right * NoClipSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            _playerTransform.position += _playerTransform.up * NoClipSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _playerTransform.position -= _playerTransform.up * NoClipSpeed * Time.deltaTime;
        }
    }
}
