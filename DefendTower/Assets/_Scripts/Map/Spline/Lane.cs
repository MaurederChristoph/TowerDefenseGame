using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Lane : MonoBehaviour {
    private readonly Queue<ScriptableEnemy> _units = new();
    private int _spawnCycleTicks;
    private int _ticksSinceLastSpawn = 0;

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
        var unitInstance = Instantiate(unit.UnitPrefab);
        var unitSplineAnimator = unitInstance.GetComponent<SplineAnimate>();
        unitSplineAnimator.Duration = unit.Time;
        unitSplineAnimator.Container = GetComponent<SplineContainer>();
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
    /// Sets the tick rate for the current spawn cycle 
    /// </summary>
    /// <param name="tickRate">The time interval in seconds between each spawn in a batch</param>
    public void SetTickRate(float tickRate) {
        _spawnCycleTicks = Convert.ToInt32(tickRate / Time.fixedDeltaTime);
    }
}