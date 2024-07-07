using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class EnemyManager : MonoBehaviour {
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
    }

    /// <summary>
    /// Calculates the enemy batch distribution and forwards it to each <see cref="Lane"/>
    /// </summary>
    /// <param name="enemyType">The type of enemy for which a batch will be spawned</param>
    public void SpawnUnit(EnemyType enemyType) {
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
    /// <returns><see cref="CombatData"/> for the given <see cref="EnemyType"/></returns>
    private CombatData GetCombatData(EnemyType enemyType) {
        return _enemyCombatInfo.CombatData.First(e => e.scriptableEnemy.EnemyType == enemyType);
    }
}
