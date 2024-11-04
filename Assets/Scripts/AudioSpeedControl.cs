using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class AudioSpeedControl : MonoBehaviour
{
    public AudioSource audioSource;
    public float maxSpeed = 10.0f;
    public float minVolume = 0.2f;
    public float maxVolume = 1.0f;
 
    private Rigidbody playerRigidbody;
 
    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }
 
    private void Update()
    {
        // Calculate player speed
        float playerSpeed = playerRigidbody.velocity.magnitude;
 
        // Map player speed to volume range
        float volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(0, maxSpeed, playerSpeed));
 
        // Set audio source volume based on player speed
        audioSource.volume = volume;
    }
}
