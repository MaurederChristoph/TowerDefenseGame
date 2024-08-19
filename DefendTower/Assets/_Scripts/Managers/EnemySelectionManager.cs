using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class EnemySelectionManager : MonoBehaviour {
	private GameManager _gameManager;
	private EnemyManager _enemyManager;
	private ScriptableCombatProgression _scriptableCombatProgression;
	private ScriptableEnemyCombatData _enemyCombatData;
	private void Start() {
		_gameManager = GameManager.Instance;
		_enemyManager = _gameManager.EnemyManager;
		_gameManager.AddPowerLevelChangeListener(GetSpawnUnits);
		_scriptableCombatProgression = ResourceSystem.Instance.GetScriptableCombatProgression();
		_enemyCombatData = ResourceSystem.Instance.GetScriptableEnemyCombatData();
	}


	private void GetSpawnUnits(int powerLevel) {
		var currentLevel = _scriptableCombatProgression.CombatData.First(c => c.PowerLevel == powerLevel);
		List<CombatDataInfo> combatList = _enemyCombatData.CombatData;
		List<CombatDataInfo> unitsToSpawn = new();
		var rnd = new Random();
		var currentCombatPower = 0;

		var possibleUnits =
			combatList.Where(c => c.AppearPowerLevel >= powerLevel - currentLevel.OldUnitRange && c.AppearPowerLevel <= powerLevel)
				.ToList();
		var strongestUnit = possibleUnits.OrderByDescending(c => c.AppearPowerLevel).First();
		List<CombatDataInfo> spawnPool = possibleUnits.Where(c => c != strongestUnit).ToList();

		var newUnitAmount = rnd.Next(currentLevel.MinAmountOfNewUnit, currentLevel.MaxAmountOfNewUnit + 1);
		for(var i = 0;i < newUnitAmount;i++) {
			unitsToSpawn.Add(strongestUnit);
			currentCombatPower += strongestUnit.CombatValue;
		}

		while(spawnPool.Count != 0) {
			var newUnit = rnd.Next(0, spawnPool.Count);
			if (currentCombatPower + spawnPool[newUnit].CombatValue <= currentLevel.CombatStrength) {
				unitsToSpawn.Add(spawnPool[newUnit]);
				currentCombatPower += spawnPool[newUnit].CombatValue;
			} else {
				spawnPool.Remove(spawnPool[newUnit]);
			}
		}

		Shuffle(unitsToSpawn, rnd);
		_enemyManager.HandleNewSpawnList(unitsToSpawn.ToList());
	}

	public static void Shuffle<T>(IList<T> list, Random rnd) {
		var n = list.Count;
		while(n > 1) {
			n--;
			var k = rnd.Next(n + 1);
			(list[k], list[n]) = (list[n], list[k]);
		}
	}
}
