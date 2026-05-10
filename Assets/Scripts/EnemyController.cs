using UnityEngine;

// 敵の簡易AIです。プレイヤーへ近づき、隣接時は攻撃します。
public class EnemyController : UnitBase
{
    public void TakeTurn(PlayerController player)
    {
        if (!IsAlive || player == null || !player.IsAlive)
        {
            return;
        }

        Vector2Int delta = player.GridPosition - GridPosition;
        int manhattan = Mathf.Abs(delta.x) + Mathf.Abs(delta.y);
        // マンハッタン距離が1なら隣接しているので攻撃します。
        if (manhattan == 1)
        {
            Attack(player);
            return;
        }

        Vector2Int step = ChooseStep(delta);
        if (step != Vector2Int.zero)
        {
            TryMove(step);
        }
    }

    private Vector2Int ChooseStep(Vector2Int delta)
    {
        Vector2Int xStep = new Vector2Int(delta.x == 0 ? 0 : (delta.x > 0 ? 1 : -1), 0);
        Vector2Int yStep = new Vector2Int(0, delta.y == 0 ? 0 : (delta.y > 0 ? 1 : -1));

        // 差が大きい軸を優先して進むと、追跡が自然に見えます。
        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
        {
            if (xStep != Vector2Int.zero && Game.CanMoveTo(GridPosition + xStep))
            {
                return xStep;
            }
            if (yStep != Vector2Int.zero && Game.CanMoveTo(GridPosition + yStep))
            {
                return yStep;
            }
        }
        else
        {
            if (yStep != Vector2Int.zero && Game.CanMoveTo(GridPosition + yStep))
            {
                return yStep;
            }
            if (xStep != Vector2Int.zero && Game.CanMoveTo(GridPosition + xStep))
            {
                return xStep;
            }
        }

        return Vector2Int.zero;
    }
}
