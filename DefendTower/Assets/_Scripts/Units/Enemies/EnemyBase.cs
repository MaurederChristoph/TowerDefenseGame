using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all enemies
/// </summary>
public class EnemyBase : UnitBase {
    /// <summary>
    /// The time it takes the enemy to complete the track
    /// </summary>
    public float Speed { get; private set; } = 22;
    /// <summary>
    /// The enemy type this unit is
    /// </summary>
    public EnemyType EnemyType { get; private set; }
    /// <summary>
    /// Translates enemy properties form scriptable enemy object to enemy script
    /// </summary>
    /// <typeparam name="T">Type of scriptable unit</typeparam>
    /// <param name="unit">Scriptable unit</param>
    protected override void InitUnit(ScriptableUnit unit) {
        base.InitUnit(unit);
        var enemy = (ScriptableEnemy)unit;
        Speed = enemy.Time;
        EnemyType = enemy.EnemyType;
    }
}
