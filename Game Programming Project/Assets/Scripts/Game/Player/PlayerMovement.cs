using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [HideInInspector] public static PlayerMovement instance;

    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float crouchTime = 0.2f;
    [SerializeField] private float sprintDoubleClickTime = 0.5f;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private float maxDistance;
    [SerializeField] private LayerMask layerMask;

    [HideInInspector] public float mouseSensitivity;

    private Rigidbody rb;
    private float verticalRotation = 0f;
    private float standingHeight;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private bool doubleClickStarted = false;
    private float lastForwardPressTime = 0f;

    private void Start()
    {
        instance = this;

        rb = GetComponent<Rigidbody>();
        standingHeight = transform.localScale.y;

        ResetSensitivity();
    }

    private void Update()
    {
        // Rotate player based on mouse movement
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up, mouseX);
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Move player based on keyboard input
        float horizontalMovement = Input.GetAxisRaw("Horizontal") * (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime;
        float verticalMovement = Input.GetAxisRaw("Vertical") * (isSprinting ? sprintSpeed : walkSpeed) * Time.deltaTime;
        transform.Translate(Vector3.forward * verticalMovement);
        transform.Translate(Vector3.right * horizontalMovement);

        // Crouch
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isCrouching = false;
        }

        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, crouchHeight, crouchTime), transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Lerp(transform.localScale.y, standingHeight, crouchTime), transform.localScale.z);
        }

        // Sprint
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Time.time - lastForwardPressTime < sprintDoubleClickTime)
            {
                doubleClickStarted = true;
            }
            lastForwardPressTime = Time.time;
        }

        if (doubleClickStarted && Time.time - lastForwardPressTime > sprintDoubleClickTime)
        {
            doubleClickStarted = false;
        }

        isSprinting = doubleClickStarted;

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && !isCrouching && GroundCheck())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            TemporaryData.instance.totalJumps++;
            SFXManager.instance.PlaySFX(1);
            if (SceneManager.GetActiveScene().name == "Game")
            {
                GameData.instance.gameJumps++;
            }
            else if (SceneManager.GetActiveScene().name == "Infinite")
            {
                InfiniteData.instance.gameJumps++;
            }
        }
    }

    public void ResetSensitivity()
    {
        mouseSensitivity = (TemporaryData.instance.sensitivity * 500) + 200;
    }

    private bool GroundCheck()
    {
        return Physics.BoxCast(transform.position, boxSize, -transform.up, transform.rotation, maxDistance, layerMask);
    }
}