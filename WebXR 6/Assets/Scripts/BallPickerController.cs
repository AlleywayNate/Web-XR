using UnityEngine;
using System.Collections;

public class BallPickerController : MonoBehaviour
{
    [Header("Throw Settings")]
    public float throwForceMultiplier = 10f; // Force applied when throwing the ball
    public LayerMask ballLayer; // Layer for the balls

    [Header("Cursor Settings")]
    public Texture2D cursorTexture; // Custom cursor texture
    public Vector2 cursorHotspot = new Vector2(16, 16); // Center of the custom cursor

    private Camera mainCamera;
    private Rigidbody heldBall;
    private bool isHoldingBall = false;
    private bool canPickUpBall = true; // Flag to control ball pickup

    private void Start()
    {
        // Set the custom cursor
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);

        // Get the main camera reference
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (isHoldingBall)
        {
            // If holding a ball, move it with the cursor
            HoldBall();
            CheckForThrow();
        }
        else if (canPickUpBall)
        {
            // If not holding a ball and can pick up, check for a ball under the cursor
            CheckForPickup();
        }
    }

    private void CheckForPickup()
    {
        // Perform a raycast from the center of the screen
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ballLayer))
        {
            // If the raycast hits a ball, check for a mouse click
            if (Input.GetMouseButtonDown(0)) // Left mouse button
            {
                Rigidbody ballRigidbody = hit.collider.GetComponent<Rigidbody>();
                if (ballRigidbody != null)
                {
                    // Pick up the ball
                    heldBall = ballRigidbody;
                    heldBall.useGravity = false; // Disable gravity while holding
                    isHoldingBall = true;
                }
            }
        }
    }

    private void HoldBall()
    {
        // Move the ball in front of the camera
        Vector3 holdPosition = mainCamera.transform.position + mainCamera.transform.forward * 2f;
        heldBall.MovePosition(holdPosition);
    }

    private void CheckForThrow()
    {
        // Throw the ball when the player releases the mouse button
        if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            Vector3 throwDirection = mainCamera.transform.forward;
            heldBall.useGravity = true; // Re-enable gravity
            heldBall.AddForce(throwDirection * throwForceMultiplier, ForceMode.Impulse);

            // Release the ball
            heldBall = null;
            isHoldingBall = false;

            // Start the cooldown before allowing another pickup
            StartCoroutine(PickUpCooldown());
        }
    }

    private IEnumerator PickUpCooldown()
    {
        canPickUpBall = false;
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        canPickUpBall = true;
    }
}
