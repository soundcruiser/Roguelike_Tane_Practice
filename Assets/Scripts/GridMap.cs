using UnityEngine;
using System.Collections.Generic;

// マップの通行可能/不可と、部屋データを管理するクラスです。
public class GridMap
{
    // true: 通れる床、false: 通れない壁。
    private readonly bool[,] walkable;
    private readonly int width;
    private readonly int height;
    private readonly List<RectInt> rooms = new List<RectInt>();

    public int Width => width;
    public int Height => height;
    public IReadOnlyList<RectInt> Rooms => rooms;

    public GridMap(int mapWidth, int mapHeight)
    {
        width = mapWidth;
        height = mapHeight;
        walkable = new bool[width, height];
        GenerateDungeon();
    }

    // 部屋＋通路の基本的なローグライク用ダンジョンを作ります。
    private void GenerateDungeon()
    {
        // まず全部を壁（通行不可）で初期化します。
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                walkable[x, y] = false;
            }
        }

        rooms.Clear();

        // 小さいマップでは生成が厳しいので簡易矩形部屋にフォールバックします。
        if (width < 12 || height < 10)
        {
            CreateFallbackRoom();
            return;
        }

        int roomCountTarget = Random.Range(4, 8);
        int tryCount = roomCountTarget * 12;

        for (int i = 0; i < tryCount && rooms.Count < roomCountTarget; i++)
        {
            RectInt candidate = CreateRandomRoom();
            if (IsOverlappingAnyRoom(candidate))
            {
                continue;
            }

            rooms.Add(candidate);
            CarveRoom(candidate);
        }

        // 部屋が1つも作れなかったケースを回避します。
        if (rooms.Count == 0)
        {
            CreateFallbackRoom();
            return;
        }

        // 部屋同士を通路でつなぎ、必ず行き来できるようにします。
        for (int i = 1; i < rooms.Count; i++)
        {
            Vector2Int prevCenter = GetRoomCenter(rooms[i - 1]);
            Vector2Int currentCenter = GetRoomCenter(rooms[i]);
            CarveCorridor(prevCenter, currentCenter);
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

    // 指定した部屋矩形の中心座標を返します。
    public Vector2Int GetRoomCenter(RectInt room)
    {
        return new Vector2Int(room.xMin + room.width / 2, room.yMin + room.height / 2);
    }

    private void CreateFallbackRoom()
    {
        int margin = 1;
        RectInt fallback = new RectInt(
            margin,
            margin,
            Mathf.Max(3, width - margin * 2),
            Mathf.Max(3, height - margin * 2));
        rooms.Add(fallback);
        CarveRoom(fallback);
    }

    private RectInt CreateRandomRoom()
    {
        int roomWidth = Random.Range(4, 8);
        int roomHeight = Random.Range(4, 7);

        int x = Random.Range(1, width - roomWidth - 1);
        int y = Random.Range(1, height - roomHeight - 1);
        return new RectInt(x, y, roomWidth, roomHeight);
    }

    private bool IsOverlappingAnyRoom(RectInt candidate)
    {
        // 1マス余白を持たせて、部屋同士がくっつき過ぎないようにします。
        RectInt expanded = new RectInt(
            candidate.xMin - 1,
            candidate.yMin - 1,
            candidate.width + 2,
            candidate.height + 2);

        for (int i = 0; i < rooms.Count; i++)
        {
            if (expanded.Overlaps(rooms[i]))
            {
                return true;
            }
        }

        return false;
    }

    private void CarveRoom(RectInt room)
    {
        for (int y = room.yMin; y < room.yMax; y++)
        {
            for (int x = room.xMin; x < room.xMax; x++)
            {
                walkable[x, y] = true;
            }
        }
    }

    private void CarveCorridor(Vector2Int from, Vector2Int to)
    {
        // L字通路をランダム方向で掘ります（横→縦 or 縦→横）。
        if (Random.value < 0.5f)
        {
            DigHorizontal(from.x, to.x, from.y);
            DigVertical(from.y, to.y, to.x);
        }
        else
        {
            DigVertical(from.y, to.y, from.x);
            DigHorizontal(from.x, to.x, to.y);
        }
    }

    private void DigHorizontal(int x1, int x2, int y)
    {
        int min = Mathf.Min(x1, x2);
        int max = Mathf.Max(x1, x2);
        for (int x = min; x <= max; x++)
        {
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
            {
                walkable[x, y] = true;
            }
        }
    }

    private void DigVertical(int y1, int y2, int x)
    {
        int min = Mathf.Min(y1, y2);
        int max = Mathf.Max(y1, y2);
        for (int y = min; y <= max; y++)
        {
            if (x > 0 && x < width - 1 && y > 0 && y < height - 1)
            {
                walkable[x, y] = true;
            }
        }
    }
}
