using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int attack = 3;

    protected GridGameController Game;

    public Vector2Int GridPosition { get; protected set; }
    public int CurrentHp { get; private set; }
    public bool IsAlive => CurrentHp > 0;

    public void Setup(GridGameController game, Vector2Int startPos)
    {
        Game = game;
        GridPosition = startPos;
        CurrentHp = maxHp;
        transform.position = new Vector3(startPos.x, startPos.y, 0f);
    }

    protected bool TryMove(Vector2Int dir)
    {
        Vector2Int next = GridPosition + dir;
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
        Debug.Log($"{name} attacks {target.name} for {attack} damage.");
    }

    public void ReceiveDamage(int amount)
    {
        CurrentHp -= amount;
        if (CurrentHp <= 0)
        {
            CurrentHp = 0;
            Debug.Log($"{name} was defeated.");
            gameObject.SetActive(false);
        }
    }
}
