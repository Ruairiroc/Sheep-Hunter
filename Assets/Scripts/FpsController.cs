using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsController : MonoBehaviour
{

    public float mouseSensitivityX = 100.0f;
    public float mouseSensitivityY = 100.0f;

    public float moveSpeed = 10.0f;
    public float jumpForce = 5.0f;
    public Transform camera;
    public Transform shotgun;
    float lookRotation;
    Vector3 moveAmount;
    Vector3 smoothMoveVelocity;

    public float health = 3f;

    public float pushForce = 5f; // Force applied to the player when hit by an enemy
    void Start()
    {
        Transform camera = Camera.main.transform;
        if (shotgun == null)
        {
            Debug.LogError("FpsController: Shotgun transform not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivityX);
        lookRotation += Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivityY;
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camera.localEulerAngles = Vector3.left * lookRotation;

        // Ensure the shotgun follows the camera's rotation
        if (shotgun != null)
        {
            shotgun.localEulerAngles = camera.localEulerAngles;
            shotgun.Rotate(Vector3.up * 90);
        }

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        Vector3 targetMoveAmount = moveDir * moveSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);

        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }

    void onCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Calculate the direction from the enemy to the player
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;

            // Apply a force to the player in the opposite direction of the enemy
            GetComponent<Rigidbody>().AddForce(pushDirection * pushForce, ForceMode.Impulse);

            health -= 1f; // Decrease health by 1 when hit by an enemy
            Debug.Log($"Player health: {health}");
            if (health <= 0)
            {
                Debug.Log("Player destroyed");
                Destroy(gameObject); // Destroy the player object when health reaches 0
            }
        }
    }
}
