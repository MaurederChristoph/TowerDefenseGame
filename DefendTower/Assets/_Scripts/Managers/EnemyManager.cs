using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

/// <summary>
/// Handles Action that concern enemies
/// </summary>
public class EnemyManager : MonoBehaviour {
    /// <summary>
    /// List of all currently alive enemies
    /// </summary>
    private readonly List<EnemyBase> _enemies = new();
    /// <summary>
    /// Returns a copy of all current enemies
    /// </summary>
    public List<EnemyBase> Enemies => _enemies.ToList();

    [Tooltip("Represents the lane where enemies walk on")]
    [SerializeField] private Lane _highLane, _midLane, _lowLane;
    [Tooltip("Represents the total time in which a enemy batch is spawned")]
    [SerializeField] private float _spawningInterval;

    /// <summary>
    /// Data storage for all relevant enemy data
    /// </summary>
    private ScriptableEnemyCombatData _enemyCombatInfo;


    private void Start() {
        _enemyCombatInfo = Resources.Load<ScriptableEnemyCombatData>("EnemyCombatData");
        AddEnemySpawnListeners();
    }

    /// <summary>
    /// Adds a listener to all enemy spawn events
    /// </summary>
    private void AddEnemySpawnListeners() {
        _highLane.AddEnemySpawnListener(AddEnemy);
        _midLane.AddEnemySpawnListener(AddEnemy);
        _lowLane.AddEnemySpawnListener(AddEnemy);
    }

    /// <summary>
    /// Calculates the enemy batch distribution and forwards it to each <see cref="Lane"/>
    /// </summary>
    /// <param name="enemyType">The type of enemy for which a batch will be spawned</param>
    public void SpawnEnemy(EnemyType enemyType) {
        var unitData = GetCombatData(enemyType);
        var outerAmount = unitData.UnitAmount / 2;
        var tickRate = _spawningInterval / (outerAmount + 1);

        _highLane.SetTickRate(tickRate);
        _midLane.SetTickRate(tickRate);
        _lowLane.SetTickRate(tickRate);

        _highLane.AddUnits(unitData.scriptableEnemy, outerAmount);
        _lowLane.AddUnits(unitData.scriptableEnemy, outerAmount);
        if(unitData.UnitAmount % 2 != 0) {
            _midLane.WaitForSpawnCycles(outerAmount);
            _midLane.AddUnits(unitData.scriptableEnemy);
        }
    }

    /// <summary>
    /// Searched the <see cref="ScriptableEnemyCombatData"/> for a given enemy type
    /// </summary>
    /// <param name="enemyType">The enemy type for which it will search</param>
    /// <returns><see cref="CombatDataInfo"/> for the given <see cref="EnemyType"/></returns>
    private CombatDataInfo GetCombatData(EnemyType enemyType) {
        return _enemyCombatInfo.CombatData.First(e => e.scriptableEnemy.EnemyType == enemyType);
    }

    /// <summary>
    /// Adds and enemy to the currently alive enemies
    /// </summary>
    /// <param name="enemy">The enemy that is to add</param>
    private void AddEnemy(EnemyBase enemy) {
        _enemies.Add(enemy);
    }

    private void OnDestroy() {
        _lowLane.RemoveEnemySpawnListener(AddEnemy);
        _midLane.RemoveEnemySpawnListener(AddEnemy);
        _highLane.RemoveEnemySpawnListener(AddEnemy);
    }
    public void RemoveEnemy(EnemyBase enemy) {
        if(!_enemies.Contains(enemy)) {
            return;
        }
        _enemies.Remove(enemy);
    }
}
