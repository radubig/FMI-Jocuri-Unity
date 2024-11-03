using Bullets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;

        private Rigidbody2D _rb;
        private Camera _mainCamera;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _mainCamera = Camera.main;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            _rb.velocity = moveInput * moveSpeed;
        }

        public void OnShoot(InputAction.CallbackContext context)
        {
            if (!context.action.triggered) return;

            GameObject bullet = BulletPool.Instance.GetPooledObject();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = transform.rotation;
            bullet.transform.Translate(Vector3.right);
            bullet.SetActive(true);
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (context.control.device is Mouse)
            {
                Vector2 mousePos = context.ReadValue<Vector2>();
                Vector3 worldMousePos = _mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, _mainCamera.nearClipPlane));
                Vector2 direction = (worldMousePos - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else if (context.control.device is Gamepad)
            {
                Vector2 lookInput = context.ReadValue<Vector2>();
                if (lookInput == Vector2.zero) return;

                Vector2 direction = lookInput.normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
    }
}
