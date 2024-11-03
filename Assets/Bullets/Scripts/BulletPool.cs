using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bullets.Scripts
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance;

        private readonly List<GameObject> _bulletPool = new();

        [SerializeField, FormerlySerializedAs("_poolSize")]
        private int poolSize = 10;
        
        [SerializeField] private GameObject bulletPrefab;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet);
            }
        }

        public GameObject GetPooledObject()
        {
            foreach (GameObject bullet in _bulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    return bullet;
                }
            }

            GameObject newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false);
            _bulletPool.Add(newBullet);
            return newBullet;
        }
    }
}