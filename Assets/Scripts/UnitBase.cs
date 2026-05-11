using System;
using UnityEngine;

// プレイヤーと敵に共通する基底クラスです。
public abstract class UnitBase : MonoBehaviour
{
    private UnitStats stats;

    // HUD などが購読する戦闘ログイベント。全ユニット共通で1つ。
    public static event Action<string> OnCombatLog;

    protected GridGameController Game;

    public Vector2Int GridPosition { get; protected set; }
    public int CurrentHp { get; private set; }
    public int MaxHp => stats != null ? stats.maxHp : 0;
    public bool IsAlive => CurrentHp > 0;

    public void Setup(GridGameController game, Vector2Int startPos, UnitStats unitStats)
    {
        // 初期化時に参照と座標、HPを設定します。
        Game = game;
        stats = unitStats;
        GridPosition = startPos;
        CurrentHp = stats.maxHp;
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

        target.ReceiveDamage(stats.attack);
        string msg = $"{name} が {target.name} に {stats.attack} ダメージを与えた。";
        Debug.Log(msg);
        OnCombatLog?.Invoke(msg);
    }

    public void ReceiveDamage(int amount)
    {
        CurrentHp -= amount;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            string msg = $"{name} は倒れた。";
            Debug.Log(msg);
            OnCombatLog?.Invoke(msg);
            gameObject.SetActive(false);
        }
    }
}
