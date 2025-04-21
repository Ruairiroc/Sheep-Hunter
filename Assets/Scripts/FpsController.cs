using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private FarmerGameController controls;
    private Vector2 moveInput;
    private Vector2 cameraInput;

    private ShotgunController shotgunController;



    public float health = 3f;

    public float pushForce = 5f; // Force applied to the player when hit by an enemy

    void Awake()
    {
        controls = new FarmerGameController();

        controls.Gameplay.MoveFarmer.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.MoveFarmer.canceled += ctx => moveInput = Vector2.zero;

        controls.Gameplay.MoveCamera.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.MoveCamera.canceled += ctx => cameraInput = Vector2.zero;

        controls.Gameplay.Jump.performed += ctx => Jump();

        controls.Gameplay.Shoot.performed += ctx => shotgunController?.Shoot();
    }
    void Start()
    {
        Transform camera = Camera.main.transform;
        shotgunController = GetComponentInChildren<ShotgunController>();
        if (shotgun == null)
        {
            Debug.LogError("FpsController: Shotgun transform not assigned!");
        }
        if (shotgunController == null)
        {
            Debug.LogError("FpsController: ShotgunController not found!");
        }
        if (camera == null)
        {
            Debug.LogError("FpsController: Camera transform not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * cameraInput.x * Time.deltaTime * mouseSensitivityX);
        lookRotation += cameraInput.y * Time.deltaTime * mouseSensitivityY;
        lookRotation = Mathf.Clamp(lookRotation, -90, 90);
        camera.localEulerAngles = Vector3.left * lookRotation;

        // Ensure the shotgun follows the camera's rotation
        if (shotgun != null)
        {
            shotgun.localEulerAngles = camera.localEulerAngles;
            shotgun.Rotate(Vector3.up * 90);
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        Vector3 targetMoveAmount = moveDir * moveSpeed;
        moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
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

    void Jump()
    {
        if (Mathf.Abs(GetComponent<Rigidbody>().velocity.y) < 0.01f) // Ensure the player is grounded
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Player jumped!");
        }
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
