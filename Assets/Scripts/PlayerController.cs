using UnityEngine;

public class PlayerController : UnitBase
{
    private void Update()
    {
        if (TurnManager.Instance == null || TurnManager.Instance.State != TurnState.PlayerTurn || !IsAlive)
        {
            return;
        }

        Vector2Int input = ReadInput();
        if (input == Vector2Int.zero)
        {
            return;
        }

        EnemyController enemy = Game.Enemy;
        if (enemy != null && enemy.IsAlive && enemy.GridPosition == GridPosition + input)
        {
            Attack(enemy);
            TurnManager.Instance.EndPlayerTurn();
            return;
        }

        if (TryMove(input))
        {
            TurnManager.Instance.EndPlayerTurn();
        }
    }

    private Vector2Int ReadInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            return Vector2Int.up;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            return Vector2Int.down;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            return Vector2Int.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            return Vector2Int.right;
        }

        return Vector2Int.zero;
    }
}
