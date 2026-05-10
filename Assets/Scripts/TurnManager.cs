using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn,
    Busy
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public TurnState State { get; private set; } = TurnState.PlayerTurn;

    private PlayerController player;
    private EnemyController enemy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(PlayerController playerController, EnemyController enemyController)
    {
        player = playerController;
        enemy = enemyController;
        State = TurnState.PlayerTurn;
    }

    public void EndPlayerTurn()
    {
        if (State != TurnState.PlayerTurn)
        {
            return;
        }

        State = TurnState.EnemyTurn;
        ResolveEnemyTurn();
    }

    private void ResolveEnemyTurn()
    {
        if (enemy != null && enemy.IsAlive)
        {
            enemy.TakeTurn(player);
        }

        State = TurnState.PlayerTurn;
    }
}
