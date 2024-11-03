using CharacterScripts;
using UnityEngine;

namespace Bullets.Scripts
{
    public class BulletBehaviour : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private float bulletLifetime = 1f;
        private Rigidbody2D _rb;
        private float _timer;
        private DamageDealer _damageDealer;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _damageDealer = GetComponent<DamageDealer>();
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
                
                Health health = collision.gameObject.GetComponent<Health>();
                if (health != null) // Only proceed if there is a Health component
                {
                    _damageDealer.DealDamage(health); // Apply damage to the health component
                }
                
                DisableBullet();
            }
            
        }

        private void DisableBullet()
        {
            gameObject.SetActive(false);
        }
    }
}