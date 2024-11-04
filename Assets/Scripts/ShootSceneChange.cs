using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShootSceneChange : MonoBehaviour
{
    public InputActionReference triggerLeft;
    public InputActionReference triggerRight;
    void Update()
    {
        float leftTriggerValue = triggerLeft.action.ReadValue<float>();
        float rightTriggerValue = triggerRight.action.ReadValue<float>();

        if (leftTriggerValue > 0.5f && rightTriggerValue > 0.5f) {
            SceneManager.LoadScene("Scenes/Iron Man");
        }
    }
}
