using System.Collections;
using CharacterScripts;
using UnityEngine;
using UnityEngine.AI;

namespace NPCs.Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public Transform player;
        public float spawnInterval = 5f;
        public int maxEnemies = 5;
        private int _currentEnemyCount = 0;

        private void Start()
        {
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                if (_currentEnemyCount < maxEnemies)
                {
                    SpawnEnemy();
                    _currentEnemyCount++;
                }
            }
        }

        private void SpawnEnemy()
        {
            GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            EnemyAI enemyAI = newEnemy.GetComponent<EnemyAI>();
            enemyAI.player = player;
            Health enemyHealth = newEnemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.onDeath += () => { _currentEnemyCount--; };
            }
        }
    }
}