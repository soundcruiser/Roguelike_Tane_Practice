using UnityEngine;

public class GridGameController : MonoBehaviour
{
    [Header("Map")]
    [SerializeField] private int mapWidth = 12;
    [SerializeField] private int mapHeight = 10;

    private GridMap map;

    public PlayerController Player { get; private set; }
    public EnemyController Enemy { get; private set; }

    public void Initialize()
    {
        map = new GridMap(mapWidth, mapHeight);
        BuildVisualMap();
        SpawnUnits();
    }

    public bool CanMoveTo(Vector2Int pos)
    {
        if (!map.IsWalkable(pos))
        {
            return false;
        }

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

    private void CreateTile(Vector2Int pos, bool walkable, Transform parent)
    {
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
        GameObject playerObj = new GameObject("Player");
        SpriteRenderer playerRenderer = playerObj.AddComponent<SpriteRenderer>();
        playerRenderer.sprite = SpriteFactory.SharedSquare;
        playerRenderer.color = Color.cyan;
        playerRenderer.sortingOrder = 10;
        Player = playerObj.AddComponent<PlayerController>();
        Player.Setup(this, new Vector2Int(2, 2));

        GameObject enemyObj = new GameObject("Enemy");
        SpriteRenderer enemyRenderer = enemyObj.AddComponent<SpriteRenderer>();
        enemyRenderer.sprite = SpriteFactory.SharedSquare;
        enemyRenderer.color = Color.red;
        enemyRenderer.sortingOrder = 10;
        Enemy = enemyObj.AddComponent<EnemyController>();
        Enemy.Setup(this, new Vector2Int(mapWidth - 3, mapHeight - 3));
    }
}
