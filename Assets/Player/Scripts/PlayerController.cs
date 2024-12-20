using System;
using Bullets.Scripts;
using CharacterScripts;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons.Scripts;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f;
        public GameObject activeWeapon;
        public GameObject secondaryWeapon;
        
        private Rigidbody2D _rb;
        private Camera _mainCamera;
        private Vector2 _velocity;
        private bool _isTryingToShoot;
        private float _nextActionTime = 0f;
        private WeaponScript activeWeaponScript;
        private InputAction shootingAction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _mainCamera = Camera.main;
            _isTryingToShoot = false;
            activeWeaponScript = activeWeapon.GetComponent<WeaponScript>();
        }

        private void FixedUpdate()
        {
            _rb.velocity = _velocity;
            WeaponBehaviour();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            _velocity = moveInput * moveSpeed;
        }

        public void OnSwitchWeapon()
        {
            (activeWeapon, secondaryWeapon) = (secondaryWeapon, activeWeapon);
            activeWeaponScript = activeWeapon.GetComponent<WeaponScript>();
        }
        
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (!context.action.triggered) return;

            _isTryingToShoot = true;
            shootingAction = context.action;
            /*shootingAction.started += context =>
            {
                _isTryingToShoot = true;
            };*/
            shootingAction.canceled += context =>
            {
                Debug.Log("OnShoot pa si la revedere:" + _isTryingToShoot);
                _isTryingToShoot = false;
            };
            //if (context.action.WasReleasedThisFrame()) _isTryingToShoot = false;
            
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

        public void WeaponBehaviour()
        {
            if (_isTryingToShoot && Time.time >= _nextActionTime)
            {
                _nextActionTime = Time.time + activeWeaponScript.fireRateInSeconds;
                GameObject bullet = BulletPool.Instance.GetPooledObject();
                bullet.GetComponent<DamageDealer>().damageAmount = activeWeaponScript.damage;
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.transform.Translate(Vector3.right);
                bullet.SetActive(true);
            }
        }
    }
}
