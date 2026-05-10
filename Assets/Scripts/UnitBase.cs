using UnityEngine;

// プレイヤーと敵に共通する基底クラスです。
public abstract class UnitBase : MonoBehaviour
{
    // Inspector から調整できるように SerializeField を付けています。
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int attack = 3;

    protected GridGameController Game;

    public Vector2Int GridPosition { get; protected set; }
    public int CurrentHp { get; private set; }
    public bool IsAlive => CurrentHp > 0;

    public void Setup(GridGameController game, Vector2Int startPos)
    {
        // 初期化時に参照と座標、HPを設定します。
        Game = game;
        GridPosition = startPos;
        CurrentHp = maxHp;
        transform.position = new Vector3(startPos.x, startPos.y, 0f);
    }

    protected bool TryMove(Vector2Int dir)
    {
        Vector2Int next = GridPosition + dir;
        // 壁や他ユニットに塞がれていたら移動しません。
        if (!Game.CanMoveTo(next))
        {
            return false;
        }

        GridPosition = next;
        transform.position = new Vector3(next.x, next.y, 0f);
        return true;
    }

    public void Attack(UnitBase target)
    {
        if (!IsAlive || target == null || !target.IsAlive)
        {
            return;
        }

        target.ReceiveDamage(attack);
        Debug.Log($"{name} が {target.name} に {attack} ダメージを与えた。");
    }

    public void ReceiveDamage(int amount)
    {
        CurrentHp -= amount;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            Debug.Log($"{name} は倒れた。");
            gameObject.SetActive(false);
        }
    }
}
