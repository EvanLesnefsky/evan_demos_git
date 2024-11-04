using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private GameObject playerOrigin;

    private void OnCollisionEnter(Collision collision)
    {
        playerOrigin.transform.position = gameObject.transform.position;
        Destroy(gameObject);
    }
}


