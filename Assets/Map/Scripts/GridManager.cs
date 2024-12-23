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

        private Sprite CreateGridSprite(float size)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);
            texture.Apply();

            Sprite sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, size, size), new Vector2(0.5f, 0.5f), 1.0f);
            sprite.name = "SquareSprite";

            GameObject squareObject = new GameObject();

            return sprite;
        }
        
        
        
        private void InitializeGrid()
        {
            grid = new Node[gridSize.x, gridSize.y];
            Vector3 worldBottomLeft = transform.position - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * cellSize) + Vector3.up * (y * cellSize);
                    
                    DisplayGrid(worldPoint,x , y);

                    bool walkable = !Physics2D.OverlapCircle(worldPoint, cellSize / 2, obstacleLayers);
                    grid[x, y] = new Node(walkable, worldPoint, x, y);
                }
            }
        }

        private void DisplayGrid(Vector3 worldPoint, int x, int y)
        {
            GameObject gridSquare = new GameObject("gridSquare"+x+y);
            SpriteRenderer renderer = gridSquare.AddComponent<SpriteRenderer>();

            renderer.sprite = CreateGridSprite(0.09f); 
            renderer.color = Color.blue;
                    
            gridSquare.transform.position = worldPoint;
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