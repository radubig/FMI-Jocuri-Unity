using Bullets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public float moveSpeed = 5f;

	private Vector2 moveInput;
	private Rigidbody2D rb;
	private Camera mainCamera;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		mainCamera = Camera.main; // Get reference to the main camera
	}

	// Called when the Move input action is performed
	public void OnMove(InputAction.CallbackContext context)
	{
		moveInput = context.ReadValue<Vector2>();
	}
	
	// Called when the Shoot input action is performed
	public void OnShoot(InputAction.CallbackContext context)
	{
		if (context.action.triggered)
		{
			// Shoot bullet
			GameObject bullet = BulletPool.Instance.GetPooledObject();
			bullet.transform.position = transform.position;
			bullet.transform.rotation = transform.rotation;
			bullet.transform.Translate(Vector3.right);
			bullet.SetActive(true);
		}
	}
	
	private void FixedUpdate()
	{
		// Handle movement
		Vector2 move = new Vector2(moveInput.x, moveInput.y) * moveSpeed;
		rb.velocity = move;

		// Rotate player to face the mouse
		RotatePlayerToMouse();
	}

	private void LateUpdate()
	{
		// Ensure the camera always stays at the default rotation
		mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
	}


	private void RotatePlayerToMouse()
	{
		// Get the mouse position in world space
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCamera.nearClipPlane));

		// Calculate direction from player to mouse
		Vector2 direction = (worldMousePos - transform.position).normalized;

		// Calculate the angle in degrees
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		// Apply rotation to the player (adjust based on your character's sprite orientation)
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}
}
