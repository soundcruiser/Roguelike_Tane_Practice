using UnityEngine;

// プレイヤー入力を受け取り、ターン制で移動/攻撃を行います。
public class PlayerController : UnitBase
{
    private void Update()
    {
        // プレイヤーターン以外では入力を受け付けません。
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
        // 移動先に敵がいる場合は移動ではなく攻撃を実行します。
        if (enemy != null && enemy.IsAlive && enemy.GridPosition == GridPosition + input)
        {
            Attack(enemy);
            TurnManager.Instance.EndPlayerTurn();
            return;
        }

        // 移動できた場合のみターン終了します。
        if (TryMove(input))
        {
            TurnManager.Instance.EndPlayerTurn();
        }
    }

    private Vector2Int ReadInput()
    {
        // キー入力を「方向ベクトル」に変換します。
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
