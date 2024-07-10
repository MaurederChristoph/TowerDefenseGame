using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles functions that concern all units
/// </summary>
public class UnitManager : MonoBehaviour {
    /// <summary>
    /// Reference to the current instance of the game manager 
    /// </summary>
    private GameManager _gameManager;
    /// <summary>
    /// Reference to the current instance of the enemy manager
    /// </summary>
    private EnemyManager _enemyManager;
    /// <summary>
    /// Reference to the current instance of the tower manager
    /// </summary>
    private TowerManager _towerManager;

    private void Start() {
        _gameManager = GameManager.Instance;
        _enemyManager = _gameManager.EnemyManager;
        _towerManager = _gameManager.TowerManager;
    }

    /// <summary>
    /// Get a new target
    /// </summary>
    /// <param name="origin">The unit that needs a target</param>
    /// <param name="toTarget">The targets faction</param>
    /// <returns>The chosen target</returns>
    public UnitBase GetTarget(UnitBase origin, Faction toTarget) {
        IEnumerable<UnitBase> targets = toTarget switch {
            Faction.Tower => _enemyManager.Enemies,
            Faction.Enemy => _towerManager.Towers,
            _ => null,
        };
        return origin.AttackTargetingStrategy.GetNextTarget(origin, targets);
    }
}
