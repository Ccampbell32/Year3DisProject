using UnityEngine;

public class SimpleFPSController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;

    [Header("Mouse")]
    public float mouseSensitivity = 2f;

    [Header("Sprint")]
    public float maxSprintTime = 4f;
    public float sprintCooldown = 3f;

    [Header("Head Bob")]
    public float bobSpeed = 6f;
    public float bobAmount = 0.05f;

    [Header("Lean")]
    public float leanAngle = 15f;
    public float leanSpeed = 6f;

    public Transform cameraHolder;

    float xRotation = 0f;
    float sprintTimer;
    bool sprintCooling;

    float defaultY;
    float bobTimer;

    float currentLean;

    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        sprintTimer = maxSprintTime;
        defaultY = cameraHolder.localPosition.y;
    }

    void Update()
    {
        Look();
        Move();
        HeadBob();
        Lean();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
    }

    // ---------------- LOOK ----------------

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, currentLean);
        transform.Rotate(Vector3.up * mouseX);
    }

    // ---------------- MOVE + SPRINT ----------------

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool moving = x != 0 || z != 0;

        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && sprintTimer > 0 && !sprintCooling && moving)
        {
            speed = sprintSpeed;
            sprintTimer -= Time.deltaTime;

            if (sprintTimer <= 0)
                sprintCooling = true;
        }

        if (sprintCooling)
        {
            sprintTimer += Time.deltaTime;

            if (sprintTimer >= maxSprintTime)
            {
                sprintTimer = maxSprintTime;
                sprintCooling = false;
            }
        }

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
    }

    // ---------------- HEAD BOB ----------------

    void HeadBob()
    {
        if (controller.velocity.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float bob = Mathf.Sin(bobTimer) * bobAmount;

            cameraHolder.localPosition = new Vector3(0, defaultY + bob, 0);
        }
        else
        {
            bobTimer = 0;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition,
                new Vector3(0, defaultY, 0), Time.deltaTime * 5f);
        }
    }

    // ---------------- LEAN ----------------

    void Lean()
    {
        float targetLean = 0;

        if (Input.GetKey(KeyCode.Q))
            targetLean = leanAngle;

        if (Input.GetKey(KeyCode.E))
            targetLean = -leanAngle;

        currentLean = Mathf.Lerp(currentLean, targetLean, Time.deltaTime * leanSpeed);
    }
}
