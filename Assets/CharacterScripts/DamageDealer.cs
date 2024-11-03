using UnityEngine;
using System;

namespace CharacterScripts
{

    public class DamageDealer : MonoBehaviour
    {
        public float damageAmount = 20f;

        public event Action OnDamageDealt;

        public void DealDamage(Health health)
        {
            if (health == null) return;
            
            health.TakeDamage(damageAmount);

            OnDamageDealt?.Invoke();
        }
    }
}