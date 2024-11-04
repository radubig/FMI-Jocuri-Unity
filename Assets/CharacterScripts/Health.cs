using UnityEngine;
using System;

namespace CharacterScripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float _currentHealth;

        public event Action<float> OnHealthChanged;

        private void Awake()
        {
            _currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Max(_currentHealth, 0);

            Debug.Log($"hp remaining for {gameObject.name}: {_currentHealth}");
            // Trigger health change event
            OnHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                HandleDepletedHealth();
            }
        }

        private void HandleDepletedHealth()
        {
            Debug.Log($"{gameObject.name} has died.");
            // To be implemented once we decide what to do when hp reaches 0
        }
    }
}