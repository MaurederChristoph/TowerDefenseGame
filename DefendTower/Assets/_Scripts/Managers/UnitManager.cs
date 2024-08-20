using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
            Faction.Tower => _towerManager.Towers,
            Faction.Enemy => _enemyManager.Enemies,
            _ => null,
        };
        return origin.AttackTargetingStrategy.GetNextTarget(origin, targets);
    }

    /// <summary>
    /// Finds all units of a type within a given range 
    /// </summary>
    /// <param name="origin">The unit of origin</param>
    /// <param name="toTarget">The target faction that will be returned</param>
    /// <param name="range">The range which all units will re returned</param>
    /// <returns>List of units surrounding the origin</returns>
    public List<UnitBase> GetTargetsWithinRange(UnitBase origin, Faction toTarget, float range) {
        IEnumerable<UnitBase> targets = toTarget switch {
            Faction.Tower => _towerManager.Towers,
            Faction.Enemy => _enemyManager.Enemies,
            _ => null,
        };
        return targets.Where(t => t.Faction == toTarget)
            .Where(t => Vector3.Distance(origin.transform.position, t.transform.position) < range)
            .ToList();
    }
}
