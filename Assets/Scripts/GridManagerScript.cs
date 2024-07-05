using UnityEngine;

public class GridManagerScript : MonoBehaviour
{
    public GameObject tilePrefab;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public ObstacleData obstacleData;
    private GameObject[,] grid;

    void Start()
    {
        grid = new GameObject[gridWidth, gridHeight];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x, 0, y);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                tile.name = $"Tile_{x}_{y}";
                tile.AddComponent<TileInfo>().SetPosition(x, y);
                grid[x, y] = tile;
            }
        }
    }

    public bool IsTileBlocked(int x, int y)
    {
        int index = y * gridWidth + x;
        return obstacleData.obstacles[index];
    }
}
