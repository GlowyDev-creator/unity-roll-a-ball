using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 11.0f;
    public float jumpForce = 5.0f; // Control how high the ball jumps

    private Rigidbody rb;
    private Transform cameraTransform;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // Check for Jump input in Update so it doesn't miss keypresses
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Apply instant upward velocity change for a crisp jump
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false; // Prevent double jumping in mid-air
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 movement = (forward * moveVertical) + (right * moveHorizontal);
            rb.AddForce(movement * speed);
        }
    }

    // Detect when the ball touches the ground
    private void OnCollisionStay(Collision collision)
    {
        // Loop through everything the ball is touching
        foreach (ContactPoint contact in collision.contacts)
        {
            // If the surface point is facing mostly upwards, consider it ground
            if (contact.normal.y > 0.6f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    // Set grounded to false immediately when leaving a surface
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
