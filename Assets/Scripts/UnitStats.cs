using UnityEngine;

/// <summary>
/// ユニット（プレイヤー・敵）のステータスを定義する ScriptableObject です。
/// Inspector で値を調整でき、.asset ファイルとして保存します。
/// </summary>
[CreateAssetMenu(fileName = "NewUnitStats", menuName = "Roguelike/UnitStats")]
public class UnitStats : ScriptableObject
{
    [Header("基本ステータス")]
    public int maxHp = 10;
    public int attack = 3;
}
