using System.Collections.Generic;
using UnityEngine;

namespace Bullets.Scripts
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance;

        private List<GameObject> _bulletPool = new();
        private int _poolSize = 10;
        
        [SerializeField] private GameObject bulletPrefab;
        
        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        private void Start()
        {
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet);
            }
        }

        public GameObject GetPooledObject()
        {
            foreach (var bullet in _bulletPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    return bullet;
                }
            }
            return null;
        }
    }
}