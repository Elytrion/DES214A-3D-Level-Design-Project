using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public bool QuickTeleport = false;
    public bool RequireButtonPress = false;

    public Transform StartPosition;
    public Transform EndPosition;
    private bool beginFade = false;
    private GameObject _playerGO;
    private bool teleportPlayer = false;
    private bool canTeleport = false;
    
    public float speedScale = 1f;
    public Color fadeColor = Color.black;
    
    public AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1),
        new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    public bool startFadedOut = false;

    private float alpha = 0f;
    private Texture2D texture;
    private int direction = 0;
    private float time = 0f;

    private bool hasTextOnTrigger = false;
    private DisplayTextOnTrigger _textTrigger;

    private void Start()
    {
        if (startFadedOut) alpha = 1f; else alpha = 0f;
        texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
        texture.Apply();
        if (gameObject.GetComponent<DisplayTextOnTrigger>() != null)
        {
            hasTextOnTrigger = true;
            _textTrigger = gameObject.GetComponent<DisplayTextOnTrigger>();
        }
    }

    private void Update()
    {
        if (direction == 0 && beginFade)
        {
            if (alpha >= 1f) // Fully faded out
            {
                alpha = 1f;
                time = 0f;
                direction = 1;
                beginFade = false;
            }
            else // Fully faded in
            {
                alpha = 0f;
                time = 1f;
                direction = -1;
                beginFade = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && canTeleport)
        {
            beginFade = true;
            canTeleport = false;
            _playerGO.GetComponent<CharacterController>().enabled = false;
        }

        if (teleportPlayer)
        {
            if (RequireButtonPress && !Input.GetKeyDown(KeyCode.E) && QuickTeleport) return;
            teleportPlayer = false;
            _playerGO.transform.position = EndPosition.position;
            _playerGO.transform.rotation = EndPosition.rotation;
            _playerGO.GetComponent<CharacterController>().enabled = true;
            if (hasTextOnTrigger)
                _textTrigger.ForceExit();
            if (!QuickTeleport)
                beginFade = true;
        }


    }
    public void OnGUI()
    {
        if (alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
        if (direction != 0)
        {
            time += direction * Time.deltaTime * speedScale;
            alpha = Curve.Evaluate(time);
            texture.SetPixel(0, 0, new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha));
            texture.Apply();
            if (alpha <= 0f || alpha >= 1f)
            {
                if (direction == -1)
                    teleportPlayer = true;
                
                direction = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerGO = other.gameObject;
            if (!QuickTeleport)
            {
                if (RequireButtonPress)
                    canTeleport = true;
                else
                {
                    _playerGO.GetComponent<CharacterController>().enabled = false;
                    beginFade = true;
                }
            }
            else
            {
                _playerGO.GetComponent<CharacterController>().enabled = false;
                teleportPlayer = true;
            }
        }
    }

}