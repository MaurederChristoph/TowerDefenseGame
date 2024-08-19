using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Handles Action that concern enemies
/// </summary>
public class EnemyManager : MonoBehaviour {
	[Tooltip("Represents the lane where enemies walk on")]
	[SerializeField] private Lane _highLane, _midLane, _lowLane;

	/// <summary>
	/// The amount fof time it take for a unit package to spawn
	/// </summary>
	private float _packageSpawningInterval;

	/// <summary>
	/// The amount of time it take between each spawn
	/// </summary>
	private float _spawningInterval;

	/// <summary>
	/// List of all currently alive enemies
	/// </summary>
	private readonly List<EnemyBase> _enemies = new();

	/// <summary>
	/// Returns a copy of all current enemies
	/// </summary>
	public List<EnemyBase> Enemies {
		get => _enemies.ToList();
	}

	private readonly Queue<CombatDataInfo> _spawnList = new();
	private GameManager _gameManager;
	private DelayedActionHandler _delayedActionHandler;

	private void Start() {
		_gameManager = GameManager.Instance;
		_delayedActionHandler = _gameManager.DelayedActionHandler;
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
	public void SpawnEnemy(string _ = "") {
		var unitData = _spawnList.Dequeue();
		var outerAmount = unitData.UnitAmount / 2;
		var tickRate = _packageSpawningInterval / (outerAmount + 1);

		_highLane.SetTickRate(tickRate);
		_midLane.SetTickRate(tickRate);
		_lowLane.SetTickRate(tickRate);

		_highLane.AddUnits(unitData.scriptableEnemy, outerAmount);
		_lowLane.AddUnits(unitData.scriptableEnemy, outerAmount);
		if (unitData.UnitAmount % 2 != 0) {
			_midLane.WaitForSpawnCycles(outerAmount);
			_midLane.AddUnits(unitData.scriptableEnemy);
		}
		if (_spawnList.Count > 0) {
			_delayedActionHandler.CallAfterSeconds(SpawnEnemy, _spawningInterval);
		}
	}

	/// <summary>
	/// Adds and enemy to the currently alive enemies
	/// </summary>
	/// <param name="enemy">The enemy that is to add</param>
	private void AddEnemy(EnemyBase enemy) {
		_enemies.Add(enemy);
	}

	public void HandleNewSpawnList(List<CombatDataInfo> enemyList) {
		foreach(var enemy in enemyList) {
			_spawnList.Enqueue(enemy);
		}
		_packageSpawningInterval = 58f / _spawnList.Count * 0.8f;
		_spawningInterval = 58f / _spawnList.Count;
		_delayedActionHandler.CallAfterSeconds(SpawnEnemy, _spawningInterval);
	}
	public void RemoveEnemy(EnemyBase enemy) {
		if (!_enemies.Contains(enemy)) {
			return;
		}
		_enemies.Remove(enemy);
	}
	private void OnDestroy() {
		_lowLane.RemoveEnemySpawnListener(AddEnemy);
		_midLane.RemoveEnemySpawnListener(AddEnemy);
		_highLane.RemoveEnemySpawnListener(AddEnemy);
	}
}
