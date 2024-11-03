using UnityEngine;

namespace Bullets.Scripts
{
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private float bulletLifetime = 1f;
        private Rigidbody2D _rb;
        private float _timer;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            _rb.velocity = transform.rotation * Vector3.right * bulletSpeed;
            _timer = Time.time;
        }

        private void FixedUpdate()
        {
            if (Time.time - _timer > bulletLifetime)
            {
                Debug.Log("Bullet expired");
                DisableBullet();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("bullet"))
            {
                Debug.Log("Bullet collided with " + collision.gameObject.name);
                DisableBullet();
            }
            // TODO: Interact with health component
        }

        private void DisableBullet()
        {
            gameObject.SetActive(false);
        }
    }
}