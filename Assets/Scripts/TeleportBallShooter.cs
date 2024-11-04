using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletTemplate;       // Bullet prefab
    public float shootPower = 100f;         // Speed of the bullet
    public InputActionReference trigger;    // Trigger input action
    public GameObject playerOrigin;         // Reference to the XR Origin for teleporting

    void Start()
    {
        trigger.action.performed += Shoot;
    }

    void Shoot(InputAction.CallbackContext __)
    {
        // Create the bullet and add force to shoot it forward
        GameObject newBullet = Instantiate(bulletTemplate, transform.position, transform.rotation);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.AddForce(transform.forward * shootPower);
        }

        // Add collision detection directly to the bullet
        newBullet.AddComponent<BulletCollision>().Setup(playerOrigin, newBullet);
    }
}

public class BulletCollision : MonoBehaviour
{
    private GameObject playerOrigin;
    private GameObject bullet;
    private Vector3 ballX;
    private float teleportOffset = 2f; // Offset above the collision point

    // Sets up references for the player and bullet
    public void Setup(GameObject playerOrigin, GameObject bullet)
    {
        this.playerOrigin = playerOrigin;
        this.bullet = bullet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Teleport the player to a position slightly above the bullet’s position on collision
        if (playerOrigin != null)
        {
            Vector3 teleportPosition = bullet.transform.position;
            teleportPosition.y += teleportOffset; // Add offset to the Y position
            playerOrigin.transform.position = teleportPosition;
        }
        Destroy(bullet);
    }
}
