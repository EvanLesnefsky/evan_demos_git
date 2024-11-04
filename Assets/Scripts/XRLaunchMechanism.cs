using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRLaunchMechanism : MonoBehaviour
{
    public Transform leftHandTransform;
    public Transform rightHandTransform;
    public Transform arrowTransform;
    public float distanceMultiplier = 10f;
    public float maxLaunchForce = 100f;
    public float arrowMaxLength = 5f;

    public InputActionReference leftHandPressAction;
    public InputActionReference rightHandActivateAction;

    private bool isLeftHandPressed = false;
    private bool isRightHandPressed = false;
    private bool isRightHandHeld = false;

    private void OnEnable()
    {
        leftHandPressAction.action.performed += OnLeftHandPressed;
        rightHandActivateAction.action.performed += OnRightHandPressed;
        rightHandActivateAction.action.canceled += OnRightHandReleased;

        leftHandPressAction.action.Enable();
        rightHandActivateAction.action.Enable();
    }

    private void OnDisable()
    {
        leftHandPressAction.action.performed -= OnLeftHandPressed;
        rightHandActivateAction.action.performed -= OnRightHandPressed;
        rightHandActivateAction.action.canceled -= OnRightHandReleased;
        leftHandPressAction.action.Disable();
        rightHandActivateAction.action.Disable();
    }

    private void Update()
    {
        if (isRightHandPressed)
        {
            UpdateArrow();
        }
        else
        {
            arrowTransform.gameObject.SetActive(false);
        }
    }

    private void OnLeftHandPressed(InputAction.CallbackContext context)
    {
        isLeftHandPressed = true;
    }

    private void OnRightHandPressed(InputAction.CallbackContext context)
    {
        if (isLeftHandPressed)
        {
            isRightHandPressed = true;
            isRightHandHeld = true;
        }
    }

    private void OnRightHandReleased(InputAction.CallbackContext context)
    {
        if (isRightHandHeld)
        {
            LaunchPlayer();
            ResetState();
        }
    }

    private void LaunchPlayer()
    {
        float distance = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);
        float launchForce = Mathf.Clamp(distance * distanceMultiplier, 0, maxLaunchForce);
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 launchDirection = leftHandTransform.forward;
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
    }

    private void ResetState()
    {
        isLeftHandPressed = false;
        isRightHandPressed = false;
        isRightHandHeld = false;
        arrowTransform.gameObject.SetActive(false);
    }

    private void UpdateArrow()
    {
        arrowTransform.gameObject.SetActive(true);
        float distance = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);
        float arrowLength = Mathf.Clamp(distance, 0, arrowMaxLength);
        arrowTransform.localScale = new Vector3(arrowTransform.localScale.x, arrowTransform.localScale.y, arrowLength);
        arrowTransform.position = leftHandTransform.position + (arrowTransform.forward * (arrowLength / 2));
        arrowTransform.rotation = Quaternion.LookRotation(leftHandTransform.forward);
    }
}
