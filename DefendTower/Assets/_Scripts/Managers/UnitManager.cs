using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles functions that concern all units
/// </summary>
public class UnitManager : MonoBehaviour {
    private EnemyManager _enemyManager;

    private void Start() {
        _enemyManager = GameManager.Instance.EnemyManager;
    }

    /// <summary>
    /// Get a new target
    /// </summary>
    /// <param name="origin">The unit that needs a target</param>
    /// <param name="toTarget">The targets faction</param>
    /// <returns>The chosen target</returns>
    public UnitBase GetTargeting(UnitBase origin, Faction toTarget) {
        IEnumerable<UnitBase> targets = toTarget switch {
            Faction.Tower => _enemyManager.Enemies,
            Faction.Enemy => throw new NotImplementedException(),
            _ => null,
        };
        return origin.AttackTargetingStrategy.GetNextTarget(origin, targets);
    }
}

