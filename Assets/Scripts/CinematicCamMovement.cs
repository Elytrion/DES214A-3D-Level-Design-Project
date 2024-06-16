using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FadeInFadeOut))]
public class CinematicCamMovement : MonoBehaviour
{
    public Transform playerTransform;
    public PlayerController playerController;
    public CharacterController playerCC;

    [System.Serializable]
    public class CamPositionData
    {
        public Transform camPosition;
        public bool Noninstant = false;
        public float LerpSpeed = 1f;
        public float Delay = 0f;
        public bool RequireFadeIn = false;
        public bool MoveLinearly = false;
        public bool StayOnTarget = false;
    }
    
    public int _currentCamPosition = 0;
    public CamPositionData[] camPositions;
    public bool fadeInFadeOut = false;
    private FadeInFadeOut _fadeScript;

    private float _camPositionDelayTimer = 0f;
    private bool _inCutscene = false;

    private Vector3 originalPos = new Vector3(0, 0, 0);
    private Vector3 orginalRotation = new Vector3(0, 0, 0);

    public bool HasRun = false;

    private bool _waitForFade = false;
    private bool _endCutscene = false;

    public bool StartImmediate = true;

    public bool CutsceneIsTrigger = false;

    public ToggleBlackBars _blackBarScript;
    public bool UseBlackBars = false;
    public Vector2 BB_Size_Time = new Vector2(1.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        _fadeScript = GetComponent<FadeInFadeOut>();
        if (StartImmediate)
            StartCutscene();
    }

    // Update is called once per frame
    void Update()
    {
        if (HasRun)
            return;

        if (_inCutscene)
        {
            if (fadeInFadeOut && _waitForFade)
            {
                // wait for screen to fade in
                if (!_fadeScript.FadingIn)
                    return;
                else
                    _waitForFade = false;
            }

            if (_camPositionDelayTimer > 0f)
            {
                _camPositionDelayTimer -= Time.deltaTime;

                if ((_currentCamPosition != camPositions.Length && _currentCamPosition != 0) && camPositions[_currentCamPosition - 1].StayOnTarget)
                {
                    playerTransform.position = camPositions[_currentCamPosition - 1].camPosition.position;
                    playerTransform.rotation = camPositions[_currentCamPosition - 1].camPosition.rotation;
                }

                if (_camPositionDelayTimer <= 0.0f)
                {
                    if (_currentCamPosition != camPositions.Length && camPositions[_currentCamPosition].RequireFadeIn)
                    {
                        _fadeScript.StartFade = true;
                        _waitForFade = true;
                    }
                }
            }
            else
            {
                RunCutscene();
            }
        }
    }

    public void StartCutscene()
    {
        if (camPositions.Length == 0)
        {
            Debug.LogError("No cam positions set for cutscene!");
            return;
        }

        originalPos = playerTransform.position;
        orginalRotation = playerTransform.rotation.eulerAngles;

        playerController.canMove = false;
        playerCC.enabled = false;
        _currentCamPosition = 0;
        
        if (fadeInFadeOut)
        {
            _fadeScript.StartFade = true;
            _waitForFade = true;
        }

        if (UseBlackBars)
            _blackBarScript.show(BB_Size_Time.x, BB_Size_Time.y);
        
        _inCutscene = true;
    }

    void RunCutscene()
    {   
        if (_currentCamPosition < camPositions.Length)
        {
            Transform camPos = camPositions[_currentCamPosition].camPosition;
            bool dontMoveInstantly = camPositions[_currentCamPosition].Noninstant;
            float lerpSpeed = camPositions[_currentCamPosition].LerpSpeed;
            float delay = camPositions[_currentCamPosition].Delay;
            bool requiresFadeInTransition = camPositions[_currentCamPosition].RequireFadeIn;
            bool LinearMovement = camPositions[_currentCamPosition].MoveLinearly;

            if (requiresFadeInTransition && fadeInFadeOut)
            {
                // Teleport to the next position after the fade-in transition
                if (_fadeScript.FadingIn)
                {
                    _fadeScript.StartFade = false;
                    playerTransform.position = camPos.position;
                    playerTransform.rotation = camPos.rotation;
                    _waitForFade = true;
                    _currentCamPosition++;
                    _camPositionDelayTimer = delay;
                }
            }
            else if (dontMoveInstantly)
            {
                // lerp to position
                if (LinearMovement)
                    playerTransform.position = Vector3.MoveTowards(playerTransform.position, camPos.position, lerpSpeed * Time.deltaTime);
                else
                    playerTransform.position = Vector3.Lerp(playerTransform.position, camPos.position, lerpSpeed * Time.deltaTime);

                //track the distance and use that as t
                float currDistance = Vector3.Distance(playerTransform.position, camPos.position);

                float maxDistance = 0.0f;
                if (_currentCamPosition == 0)
                    maxDistance = Vector3.Distance(originalPos, camPos.position);
                else
                    maxDistance = Vector3.Distance(camPositions[_currentCamPosition - 1].camPosition.position, camPos.position);

                float t = (maxDistance - currDistance) / maxDistance;
                if (_currentCamPosition == 0)
                    playerTransform.rotation = Quaternion.Lerp(Quaternion.Euler(orginalRotation), camPos.rotation, t);
                else
                    playerTransform.rotation = Quaternion.Lerp(camPositions[_currentCamPosition - 1].camPosition.rotation, camPos.rotation, t);
                
                if (Vector3.Distance(playerTransform.position, camPos.position) < 0.1f)
                {
                    playerTransform.rotation = camPos.rotation;
                    playerTransform.position = camPos.position;
                    _currentCamPosition++;
                    _camPositionDelayTimer = delay;
                }
            }
            else
            {
                // teleport directly to position
                playerTransform.rotation = camPos.rotation;
                playerTransform.position = camPos.position;
                _currentCamPosition++;
                _camPositionDelayTimer = delay;
            }
        }
        else
        {
            EndCutscene();
        }
    }

    void EndCutscene()
    {
        if (fadeInFadeOut && !_endCutscene)
        {
            _fadeScript.StartFade = true;
            _waitForFade = true;
            _endCutscene = true;
        }  

        if (_waitForFade)
            return;

        _inCutscene = false;
        _currentCamPosition = 0;
        playerTransform.position = originalPos;
        playerTransform.rotation = Quaternion.Euler(orginalRotation);
        playerController.canMove = true;
        playerCC.enabled = true;
        HasRun = true;
        
        if (UseBlackBars)
            _blackBarScript.hide(BB_Size_Time.y);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && CutsceneIsTrigger)
        {
            if (HasRun)
                return;

            StartCutscene();
        }
    }
}
