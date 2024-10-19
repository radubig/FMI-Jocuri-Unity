using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr : MonoBehaviour
{
    // Public variables to set in the Unity Inspector
    public GameObject tilePrefab;    // Assign your tile prefab in the inspector
    public int width = 10;           // Width of the tilemap (number of tiles)
    public int height = 10;          // Height of the tilemap (number of tiles)
    public float tileSize = 64f;     // Tile size (64x64 pixels; divide by 100 for Unity units)

    // Start is called before the first frame update
    void Start()
    {
        GenerateTilemap();
    }

    // Method to generate the tilemap
    void GenerateTilemap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate the position for each tile
                Vector3 position = new Vector3(x * (tileSize / 100), y * (tileSize / 100), 0);

                // Instantiate the tile at the calculated position
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);

                // Optionally, name the tile for easy identification in the Hierarchy
                tile.name = "Tile_" + x + "_" + y;

                // Set the tile's parent to keep things organized (optional)
                tile.transform.parent = this.transform;
            }
        }
    }
}