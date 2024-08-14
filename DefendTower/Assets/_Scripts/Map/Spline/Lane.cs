using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
/// <summary>
/// Spawns enemies from a queue to the track 
/// </summary>
public class Lane : MonoBehaviour {
    /// <summary>
    /// The queue of enemies that will be spawned
    /// </summary>
    private readonly Queue<ScriptableEnemy> _units = new();

    /// <summary>
    /// The amount of ticks it takes for a new enemy to spawn
    /// </summary>
    private int _spawnCycleTicks;

    /// <summary>
    /// The ticks (fixedDeltaTime) that happened since the last enemy was spawned
    /// </summary>
    private int _ticksSinceLastSpawn;

    /// <summary>
    /// Event that will be invoked when an enemy is spawned
    /// </summary>
    private Action<EnemyBase> _onEnemySpawn;

    private void FixedUpdate() {
        _ticksSinceLastSpawn++;
        if(_ticksSinceLastSpawn >= _spawnCycleTicks && _units.Count > 0) {
            SpawnUnit();
            _ticksSinceLastSpawn = 0;
        }
    }
    
    /// <summary>
    /// Instantiates the first unit in the queue
    /// </summary>
    private void SpawnUnit() {
        var unit = _units.Dequeue();
        if(unit == null) { return; }
        var unitInstance = Instantiate(unit.UnitPrefab,new Vector3(-999,-999),quaternion.identity);
        unitInstance.InitUnit(unit);
        var unitSplineAnimator = unitInstance.GetComponent<SplineAnimate>();
        unitSplineAnimator.Duration = unit.Speed;
        unitSplineAnimator.Container = GetComponent<SplineContainer>();
        _onEnemySpawn?.Invoke((EnemyBase)unitInstance);
    }

    /// <summary>
    /// Add units to the Queue that instantiates the units on the lane
    /// </summary>
    /// <param name="unitType">The type of unit that will be instantiated</param>
    /// <param name="amount">How many of the units will be spawned</param>
    public void AddUnits(ScriptableEnemy unitType, int amount = 1) {
        for(var i = 0;i < amount;i++) {
            _units.Enqueue(unitType);
        }
    }

    /// <summary>
    /// Adds empty slots into the queue to delay a later spawn
    /// </summary>
    /// <param name="amount">The amount of spawn cycles that are waited</param>
    public void WaitForSpawnCycles(int amount) {
        AddUnits(null, amount);
    }

    /// <summary>
    /// Sets the tick rate for the current spawn batch 
    /// </summary>
    /// <param name="tickRate">The time interval in seconds between each spawn in a batch</param>
    public void SetTickRate(float tickRate) {
        _spawnCycleTicks = Convert.ToInt32(tickRate / Time.fixedDeltaTime);
    }

    /// <summary>
    /// Adds a method to an event that is called when an enemy is spawned
    /// </summary>
    /// <param name="listener">The method that will be called</param>
    public void AddEnemySpawnListener(Action<EnemyBase> listener) {
        _onEnemySpawn += listener;
    }

    /// <summary>
    /// Removes a method of an event that is called when an enemy is spawned
    /// </summary>
    /// <param name="listener">The method that will be removed</param>
    public void RemoveEnemySpawnListener(Action<EnemyBase> listener) {
        _onEnemySpawn -= listener;
    }
}
