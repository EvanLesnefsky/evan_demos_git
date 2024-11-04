using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRLaunchMechanism : MonoBehaviour
{
    public Transform leftHandTransform; // Assign your left hand XR transform here
    public Transform rightHandTransform; // Assign your right hand XR transform here
    public Transform arrowTransform; // Assign the arrow 3D model here (e.g., a cylinder or arrow object)
    public float distanceMultiplier = 10f; // Multiplier for launch force based on hand distance
    public float maxLaunchForce = 100f; // Maximum allowed launch force
    public float arrowMaxLength = 5f; // Maximum length of the arrow to visually represent max launch force

    public InputActionReference leftHandPressAction;  // Input Action Reference for left hand press
    public InputActionReference rightHandActivateAction; // Input Action Reference for right hand activation

    private bool isLeftHandPressed = false;
    private bool isRightHandPressed = false;
    private bool isRightHandHeld = false; // Track if right hand is being held down

    private void OnEnable()
    {
        leftHandPressAction.action.performed += OnLeftHandPressed;
        rightHandActivateAction.action.performed += OnRightHandPressed;
        rightHandActivateAction.action.canceled += OnRightHandReleased; // Detect when the right hand is released

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
            arrowTransform.gameObject.SetActive(false); // Hide the arrow when not pulling
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
        // Calculate the distance between the left and right hands
        float distance = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);

        // Calculate the launch force based on the distance
        float launchForce = Mathf.Clamp(distance * distanceMultiplier, 0, maxLaunchForce);

        // Apply force in the direction the left hand is facing
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 launchDirection = leftHandTransform.forward; // Launch in the direction the left hand is facing
            rb.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
    }

    private void ResetState()
    {
        isLeftHandPressed = false;
        isRightHandPressed = false;
        isRightHandHeld = false;
        arrowTransform.gameObject.SetActive(false); // Hide the arrow when resetting state
    }

    private void UpdateArrow()
    {
        // Show the arrow
        arrowTransform.gameObject.SetActive(true);

        // Calculate the distance between hands
        float distance = Vector3.Distance(leftHandTransform.position, rightHandTransform.position);

        // Calculate the arrow's length (capped at arrowMaxLength)
        float arrowLength = Mathf.Clamp(distance, 0, arrowMaxLength);

        // Scale the arrow along its local Z-axis based on the hand distance
        // The assumption is that the cylinder (arrow) stretches along the Z-axis.
        arrowTransform.localScale = new Vector3(arrowTransform.localScale.x, arrowTransform.localScale.y, arrowLength);

        // Position the arrow so that it grows from the bottom (base) end
        // The base will stay at the left hand position, and the arrow will extend out in the forward direction.
        arrowTransform.position = leftHandTransform.position + (arrowTransform.forward * (arrowLength / 2));

        // Rotate the arrow to point in the direction of the left hand's forward vector
        arrowTransform.rotation = Quaternion.LookRotation(leftHandTransform.forward);
    }
}
