using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    private Rigidbody playerRigidbody;

    private void Start()
    {
        // Get the Rigidbody component
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get input from keyboard
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down arrow keys

        // Calculate movement direction
        Vector3 movement = new Vector3(moveX, 0, moveZ) * moveSpeed * Time.deltaTime;

        // Apply movement to the Rigidbody
        playerRigidbody.MovePosition(transform.position + movement);
    }
}
