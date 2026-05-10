using UnityEngine;

public class GridMap
{
    private readonly bool[,] walkable;
    private readonly int width;
    private readonly int height;

    public int Width => width;
    public int Height => height;

    public GridMap(int mapWidth, int mapHeight)
    {
        width = mapWidth;
        height = mapHeight;
        walkable = new bool[width, height];
        GenerateSimpleRoom();
    }

    private void GenerateSimpleRoom()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool border = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                walkable[x, y] = !border;
            }
        }

        // Add a small pillar for obstacle movement checks.
        if (width > 7 && height > 7)
        {
            walkable[4, 4] = false;
            walkable[5, 4] = false;
            walkable[4, 5] = false;
        }
    }

    public bool IsInside(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < width && pos.y >= 0 && pos.y < height;
    }

    public bool IsWalkable(Vector2Int pos)
    {
        return IsInside(pos) && walkable[pos.x, pos.y];
    }
}
