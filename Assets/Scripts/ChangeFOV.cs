 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFOV : MonoBehaviour
{
    public Camera MainCamera;
    public float FOVModifyAmount = 0.0f;
    public float FOVLerpSpeed = 1.0f;

    public Transform Target;
    public bool ShouldFaceTarget = false;
    public float MinimumTurnAngle = 5.0f;

    private float _ogFOV = 70.0f;
    private float _modifiedFOV = 0.0f;
    private bool _inTrigger = false;
    private bool DontTrigger = false;

    public bool TriggerOnce = false;

    private bool _canLerpOriginalFOV = false;

    // Start is called before the first frame update
    void Start()
    {
        _ogFOV = MainCamera.fieldOfView;
        _modifiedFOV = _ogFOV + FOVModifyAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (_inTrigger)
        {
            if (ShouldFaceTarget)
            {
                // check if the camera is facing the target
                Vector3 targetDir = Target.position - MainCamera.transform.position;
                float angle = Vector3.Angle(targetDir, MainCamera.transform.forward);

                if (angle > MinimumTurnAngle)
                {
                    LerpToOriginalFOV();
                }
                else
                {
                    LerpToTargetFOV();
                }
            }
            else
            {
                LerpToTargetFOV();
            }
        }
        else
        {
            if (_canLerpOriginalFOV)
                LerpToOriginalFOV();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (DontTrigger)
            return;
        _canLerpOriginalFOV = true;
        if (other.CompareTag("Player"))
        {
            _inTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (DontTrigger)
            return;
        if (TriggerOnce)
        {
            DontTrigger = true;
        }
        if (other.CompareTag("Player"))
        {
            _inTrigger = false;
        }
    }

    public void LerpToTargetFOV()
    {
        if (MainCamera.fieldOfView == _modifiedFOV)
            return;
        
        if (Mathf.Abs(MainCamera.fieldOfView - _modifiedFOV) < 0.1f)
            MainCamera.fieldOfView = _modifiedFOV;
        MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, _modifiedFOV, Time.deltaTime * FOVLerpSpeed);
    }

    public void LerpToOriginalFOV()
    {
        if (MainCamera.fieldOfView == _ogFOV)
            return;

        if (Mathf.Abs(MainCamera.fieldOfView - _ogFOV) < 0.1f)
        {
            if (_canLerpOriginalFOV)
                _canLerpOriginalFOV = false;
            MainCamera.fieldOfView = _ogFOV;
        }
        MainCamera.fieldOfView = Mathf.Lerp(MainCamera.fieldOfView, _ogFOV, Time.deltaTime * FOVLerpSpeed);
    }

    public void ForceReset()
    {
        DontTrigger = true;
        _inTrigger = false;
    }
}
