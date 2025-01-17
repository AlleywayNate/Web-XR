using UnityEngine;

public class BowlingBallController : MonoBehaviour
{
    [Header("Drag & Throw Settings")]
    public float throwForceMultiplier = 5f;

    [Header("Ball Control Settings")]
    public float ballMoveSpeed = 5f;

    private Rigidbody ballRigidbody;
    private bool isDragging = false;
    private Vector3 dragStartPosition;
    private Vector3 dragEndPosition;
    private bool controlBall = false;

    private void Start()
    {
        // Get the Rigidbody component
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Handle ball control after throw
        if (controlBall)
        {
            float moveInput = Input.GetAxis("Horizontal"); // A and D or Arrow keys
            Vector3 movement = new Vector3(moveInput * ballMoveSpeed * Time.deltaTime, 0, 0);
            ballRigidbody.MovePosition(transform.position + movement);
        }
    }

    private void OnMouseDown()
    {
        if (!controlBall) // Only allow dragging if ball control hasn't been switched
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
        }
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            // Optional: You can visualize the drag direction here (e.g., drawing a line in the scene).
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            dragEndPosition = Input.mousePosition;

            // Calculate drag direction and magnitude
            Vector3 dragVector = dragEndPosition - dragStartPosition;
            Vector3 throwDirection = new Vector3(dragVector.x, 0, dragVector.y).normalized;

            // Apply force to the ball
            ballRigidbody.AddForce(throwDirection * dragVector.magnitude * throwForceMultiplier);

            // Switch control to the ball
            controlBall = true;

            // Disable player control here (e.g., reference your player control script and disable it)
            DisablePlayerControl();
        }
    }

    private void DisablePlayerControl()
    {
        // Example: Assuming your player control script is called "PlayerController"
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }
        }
    }
}

