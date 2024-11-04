using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class XRClimbing : MonoBehaviour
{
    public InputActionReference leftHandGrabAction;
    public InputActionReference rightHandGrabAction;

    public Transform leftHandTransform;
    public Transform rightHandTransform;

    public float grabDistance = 0.2f; // Max distance to allow climbing
    public LayerMask climbableLayer;

    private Rigidbody playerRigidbody;
    private bool isLeftGrabbing = false;
    private bool isRightGrabbing = false;
    private Vector3 lastLeftHandPosition;
    private Vector3 lastRightHandPosition;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        if (playerRigidbody == null)
        {
            Debug.LogError("Please attach a Rigidbody to the XR Origin object.");
        }

        leftHandGrabAction.action.Enable();
        rightHandGrabAction.action.Enable();
    }

    private void FixedUpdate() // Use FixedUpdate for physics calculations
    {
        HandleHandClimbing(leftHandGrabAction, leftHandTransform, ref isLeftGrabbing, ref lastLeftHandPosition);
        HandleHandClimbing(rightHandGrabAction, rightHandTransform, ref isRightGrabbing, ref lastRightHandPosition);
    }

    private void HandleHandClimbing(InputActionReference handGrabAction, Transform handTransform, ref bool isGrabbing, ref Vector3 lastHandPosition)
    {
        // Check if the hand grab action is performed and within distance of a climbable object
        if (handGrabAction.action.ReadValue<float>() > 0.1f && IsHandNearClimbable(handTransform.position))
        {
            if (!isGrabbing)
            {
                isGrabbing = true;
                lastHandPosition = handTransform.position;
            }
            else
            {
                // Calculate the vertical movement of the hand
                Vector3 handMovement = handTransform.position - lastHandPosition;

                // Apply only vertical movement to the player
                playerRigidbody.MovePosition(playerRigidbody.position + new Vector3(0, handMovement.y, 0));

                lastHandPosition = handTransform.position; // Update last hand position
            }
        }
        else
        {
            isGrabbing = false;
        }
    }

    private bool IsHandNearClimbable(Vector3 handPosition)
    {
        // Check if there's a climbable object within the grab distance of the hand
        return Physics.CheckSphere(handPosition, grabDistance, climbableLayer);
    }
}
