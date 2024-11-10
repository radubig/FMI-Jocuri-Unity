using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Map.Scripts
{
    public class GridManager : MonoBehaviour
    {
        public Vector2Int gridSize;
        public float cellSize = 1f;
        public LayerMask obstacleLayers;

        private Node[,] grid;

        private void Start()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            grid = new Node[gridSize.x * 2, gridSize.y * 2];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

            for (int x = 0; x < gridSize.x * 2; x++)
            {
                for (int y = 0; y < gridSize.y * 2; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize) + Vector3.up * (y * cellSize);
                    bool walkable = !Physics2D.OverlapCircle(worldPoint, cellSize / 2, obstacleLayers);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        public Node GetNodeFromWorldPosition(Vector2 worldPosition)
        {
            float percentX = (worldPosition.x + gridSize.x * cellSize / 2) / (gridSize.x * cellSize);
            float percentY = (worldPosition.y + gridSize.y * cellSize / 2) / (gridSize.y * cellSize);
            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);
            return grid[x, y];
        }

        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;
                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                    {
                        neighbours.Add(grid[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }
    }

    public class Node
    {
        public bool walkable;
        public Vector3 worldPosition;
        public int gridX;
        public int gridY;

        public Node parent;
        public int gCost = 0;
        public int hCost = 0;
        public int fCost => gCost + hCost; 
        
        public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.worldPosition = worldPosition;
            this.gridX = gridX;
            this.gridY = gridY;
        }
    }
}