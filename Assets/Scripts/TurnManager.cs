using UnityEngine;

// ターンの進行状態を管理する列挙型です。
public enum TurnState
{
    // プレイヤーが入力できるターン。
    PlayerTurn,
    // 敵が行動するターン。
    EnemyTurn,
    // 今回は未使用ですが、将来の演出待ちなどに使える状態。
    Busy
}

// ゲーム全体のターン進行を管理するクラスです。
public class TurnManager : MonoBehaviour
{
    // どこからでも参照しやすいようにシングルトン化しています。
    public static TurnManager Instance { get; private set; }

    // 現在のターン状態。
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
        // 生成済みのユニットを登録し、ゲーム開始時はプレイヤーターンにします。
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
        // 敵が生きている場合だけ敵行動を実行します。
        if (enemy != null && enemy.IsAlive)
        {
            enemy.TakeTurn(player);
        }

        // 敵行動が終わったらプレイヤーターンへ戻します。
        State = TurnState.PlayerTurn;
    }
}
