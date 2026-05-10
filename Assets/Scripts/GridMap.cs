using UnityEngine;

// マップの通行可能/不可だけを管理する軽量クラスです。
public class GridMap
{
    // true: 通れる床、false: 通れない壁。
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
        // まずは固定レイアウトの簡易部屋を生成します。
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

        // 障害物チェック用に小さい柱を置きます。
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
