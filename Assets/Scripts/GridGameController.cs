using UnityEngine;

// マップ生成とユニット生成を担当するゲーム本体クラスです。
public class GridGameController : MonoBehaviour
{
    [Header("マップサイズ")]
    [SerializeField] private int mapWidth = 30;
    [SerializeField] private int mapHeight = 20;

    private GridMap map;

    public PlayerController Player { get; private set; }
    public EnemyController Enemy { get; private set; }

    public void Initialize()
    {
        // データ上のマップを作ってから、見た目を生成し、最後にユニットを配置します。
        map = new GridMap(mapWidth, mapHeight);
        FitCameraToMap();
        BuildVisualMap();
        SpawnUnits();
    }

    public bool CanMoveTo(Vector2Int pos)
    {
        // 壁なら移動不可。
        if (!map.IsWalkable(pos))
        {
            return false;
        }

        // 生存中のユニットがいるマスも移動不可。
        bool blockedByPlayer = Player != null && Player.IsAlive && Player.GridPosition == pos;
        bool blockedByEnemy = Enemy != null && Enemy.IsAlive && Enemy.GridPosition == pos;
        return !blockedByPlayer && !blockedByEnemy;
    }

    private void BuildVisualMap()
    {
        Transform root = new GameObject("MapRoot").transform;
        root.SetParent(transform, false);

        for (int y = 0; y < map.Height; y++)
        {
            for (int x = 0; x < map.Width; x++)
            {
                bool walkable = map.IsWalkable(new Vector2Int(x, y));
                CreateTile(new Vector2Int(x, y), walkable, root);
            }
        }
    }

    // マップ全体が画面に収まるようにカメラ位置とサイズを調整します。
    private void FitCameraToMap()
    {
        Camera cam = Camera.main;
        if (cam == null || !cam.orthographic)
        {
            return;
        }

        float centerX = (mapWidth - 1) * 0.5f;
        float centerY = (mapHeight - 1) * 0.5f;
        cam.transform.position = new Vector3(centerX, centerY, -10f);

        float verticalSize = mapHeight * 0.5f + 1f;
        float horizontalSize = (mapWidth * 0.5f + 1f) / Mathf.Max(cam.aspect, 0.1f);
        cam.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
    }

    private void CreateTile(Vector2Int pos, bool walkable, Transform parent)
    {
        // 1x1 のスプライトを並べて簡易的に床/壁を描画します。
        GameObject tile = new GameObject(walkable ? "Floor" : "Wall");
        tile.transform.SetParent(parent, false);
        tile.transform.position = new Vector3(pos.x, pos.y, 1f);

        SpriteRenderer renderer = tile.AddComponent<SpriteRenderer>();
        renderer.sprite = SpriteFactory.SharedSquare;
        renderer.color = walkable ? new Color(0.25f, 0.25f, 0.25f, 1f) : new Color(0.07f, 0.07f, 0.07f, 1f);
        renderer.sortingOrder = -1;
    }

    private void SpawnUnits()
    {
        Vector2Int playerSpawn = GetPlayerSpawnPosition();
        Vector2Int enemySpawn = GetEnemySpawnPosition(playerSpawn);

        // プレイヤー生成
        GameObject playerObj = new GameObject("Player");
        SpriteRenderer playerRenderer = playerObj.AddComponent<SpriteRenderer>();
        playerRenderer.sprite = SpriteFactory.SharedSquare;
        playerRenderer.color = Color.cyan;
        playerRenderer.sortingOrder = 10;
        Player = playerObj.AddComponent<PlayerController>();
        Player.Setup(this, playerSpawn);

        // 敵生成
        GameObject enemyObj = new GameObject("Enemy");
        SpriteRenderer enemyRenderer = enemyObj.AddComponent<SpriteRenderer>();
        enemyRenderer.sprite = SpriteFactory.SharedSquare;
        enemyRenderer.color = Color.red;
        enemyRenderer.sortingOrder = 10;
        Enemy = enemyObj.AddComponent<EnemyController>();
        Enemy.Setup(this, enemySpawn);
    }

    // なるべく最初の部屋中心をプレイヤー開始位置にします。
    private Vector2Int GetPlayerSpawnPosition()
    {
        if (map.Rooms.Count > 0)
        {
            return map.GetRoomCenter(map.Rooms[0]);
        }

        return new Vector2Int(2, 2);
    }

    // 最後の部屋中心を敵開始位置にして、初期距離を確保します。
    private Vector2Int GetEnemySpawnPosition(Vector2Int playerSpawn)
    {
        if (map.Rooms.Count > 1)
        {
            return map.GetRoomCenter(map.Rooms[map.Rooms.Count - 1]);
        }

        for (int y = map.Height - 2; y >= 1; y--)
        {
            for (int x = map.Width - 2; x >= 1; x--)
            {
                Vector2Int candidate = new Vector2Int(x, y);
                if (!map.IsWalkable(candidate))
                {
                    continue;
                }

                if (candidate != playerSpawn)
                {
                    return candidate;
                }
            }
        }

        return playerSpawn + Vector2Int.right;
    }
}
