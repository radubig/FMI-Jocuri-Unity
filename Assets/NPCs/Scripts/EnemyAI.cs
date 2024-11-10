using System;
using System.Collections.Generic;
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
        private float avoidanceRadius = 0.4f;
        
        private void Start()
        {
            GameObject pathfindingObject = new GameObject("PathfindingObject");
            _pathfinding = pathfindingObject.AddComponent<Pathfinding>();
        }

        private void Update()
        {
            if (player == null || _pathfinding == null) return;
            
            path = _pathfinding.FindPath(transform.position, player.position);
            targetIndex = 0;

            MoveAlongPath();
        }

        private void MoveAlongPath()
        {
            if (path == null || targetIndex >= path.Count)
            {
                Debug.Log("null lol");
                return;
            }

            Node targetNode = path[targetIndex];
            Vector2 targetPosition = targetNode.worldPosition;

            if ((Vector2)transform.position != targetPosition)
            {
                Vector2 futurePosition = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                bool collidingWithAnotherEnemy = false;
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(futurePosition, avoidanceRadius);
                foreach(var hitCollider in hitColliders)
                {
                    if (hitCollider.CompareTag("Enemy")  && hitCollider != GetComponent<Collider2D>())
                    {
                        collidingWithAnotherEnemy = true;
                    }
                }

                if (!collidingWithAnotherEnemy)
                {
                    transform.position = futurePosition;
                }
            }
            else
            {
                targetIndex++;
            }
        }
    }
}