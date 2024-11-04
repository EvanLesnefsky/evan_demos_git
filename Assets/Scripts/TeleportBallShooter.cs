using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletTemplate;
    public float shootPower = 100f;
    public InputActionReference trigger;
    public GameObject playerOrigin;

    void Start()
    {
        trigger.action.performed += Shoot;
    }

    void Shoot(InputAction.CallbackContext __)
    {
        GameObject newBullet = Instantiate(bulletTemplate, transform.position, transform.rotation);
        Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.AddForce(transform.forward * shootPower);
        }
        newBullet.AddComponent<BulletCollision>().Setup(playerOrigin, newBullet);
    }
}

public class BulletCollision : MonoBehaviour
{
    private GameObject playerOrigin;
    private GameObject bullet;
    private Vector3 ballX;
    private float teleportOffset = 2f;
    public void Setup(GameObject playerOrigin, GameObject bullet)
    {
        this.playerOrigin = playerOrigin;
        this.bullet = bullet;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (playerOrigin != null)
        {
            Vector3 teleportPosition = bullet.transform.position;
            teleportPosition.y += teleportOffset;
            playerOrigin.transform.position = teleportPosition;
        }
        Destroy(bullet);
    }
}
