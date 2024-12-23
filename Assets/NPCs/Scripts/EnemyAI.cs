using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using Map.Scripts;
using UnityEngine.Rendering.Universal;

namespace NPCs.Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        public Transform player;
        public float speed = 3f;
        public Pathfinding _pathfinding;

        private List<Node> path;
        private int targetIndex;
        private float avoidanceRadius = 0.3f;
        private GridManager grid;
        private Rigidbody2D _rb;
        
        private void Start()
        {
            GameObject pathfindingObject = new GameObject("PathfindingObject");
            _pathfinding = pathfindingObject.AddComponent<Pathfinding>();
            grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridManager>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (player == null || _pathfinding == null) return;
            
            List<Node> newPath = _pathfinding.FindPath(transform.position, player.position);
            if (newPath != null) 
            {if(path == null || !newPath.SequenceEqual(path))
            {
                targetIndex = 0;
                path = newPath;
            }}

            MoveAlongPath();
        }

        private double EuclideanDist(Vector2 point1, Vector2 point2)
        {
            return Math.Sqrt((point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y));
        }
        
        private void MoveAlongPath()
        {
            if (path == null || targetIndex >= path.Count)
            {
                return;
            }

            Node targetNode = path[targetIndex];
            Vector2 targetPosition = targetNode.worldPosition;
            
            if (grid.GetNodeFromWorldPosition(targetPosition) != grid.GetNodeFromWorldPosition(transform.position))
            {
                bool collidingWithAnotherEnemy = false;
                Vector2 futurePosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                /*
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(targetPosition, avoidanceRadius);
                foreach(var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy")  
                        && hitCollider != GetComponent<Collider2D>())
                    {
                        collidingWithAnotherEnemy = true;
                    }
                }
                */
                if (true)
                {
                    Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
                    Vector2 lookDirection = ((Vector2)player.position - (Vector2)transform.position).normalized;

                    if (direction != Vector2.zero)
                    {
                        float rotationAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                        transform.rotation = Quaternion.Euler(0, 0, rotationAngle-90);
                    }

                    _rb.velocity = direction * speed;
                    //transform.position = futurePosition;
                }
                else
                {
                    _rb.velocity = Vector2.zero;
                }
            }
            else
            {
                targetIndex++;
            }

            transform.position.Set(transform.position.x, transform.position.y, -2);
        }
    }
}