using UnityEngine;
using System;

namespace CharacterScripts
{
    public class Health : MonoBehaviour
    {
        public float maxHealth = 100f;
        public GameObject healthBarPrefab;
        public bool isDead = false;
        public event Action onDeath;
        public event Action<float> OnHealthChanged;
        private float _currentHealth;
        private GameObject healthBarObject;

        private void Awake()
        {
            _currentHealth = maxHealth;
            healthBarObject = Instantiate(healthBarPrefab, GetHealthBarPosition(), Quaternion.identity);
        }

        private void LateUpdate()
        {
            if (healthBarPrefab != null)
            {
                healthBarObject.transform.position = GetHealthBarPosition();
            }
        }

        private Vector2 GetHealthBarPosition()
        {
            return new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1);
        }
        
        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            Debug.Log($"hp remaining for {gameObject.name}: {_currentHealth}");

            var fullHpBar = healthBarObject.transform.Find("FullHP");
            var remainingHpBar = healthBarObject.transform.Find("RemainingHP");
            var newBarWidth = fullHpBar.transform.localScale.x * (_currentHealth / maxHealth);
            remainingHpBar.transform.localScale = 
                new Vector3(newBarWidth, fullHpBar.transform.localScale.y, fullHpBar.transform.localScale.z);
            remainingHpBar.transform.position = new Vector2(fullHpBar.transform.position.x - (fullHpBar.transform.localScale.x - newBarWidth)/2, fullHpBar.transform.position.y);
            
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                HandleDepletedHealth();
            }
        }

        private void HandleDepletedHealth()
        {
            Debug.Log($"{gameObject.name} has died.");
            isDead = true;
            onDeath?.Invoke();
            Destroy(healthBarObject);
            Destroy(gameObject);
        }
    }
}