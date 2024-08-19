using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data storage with list of all <see cref="CombatDataInfo"/>
/// </summary>
public class ScriptableEnemyCombatData : ScriptableObject {
	[Tooltip("List of combat data entries for different enemy types.")]
	[field: SerializeField] public List<CombatDataInfo> CombatData { get; private set; } = new();
}

/// <summary>
/// Data container to determine spawn(<see cref="ScriptableEnemy"/>, batch size and combat strength) and UI representaion 
/// </summary>
[Serializable]
public class CombatDataInfo {
	[Tooltip("The type of enemy associated with this combat data")]
	[field: SerializeField] public ScriptableEnemy scriptableEnemy { get; set; }

	[Tooltip("The icon representing the enemy in combat UI")]
	[field: SerializeField] public Sprite Icon { get; set; }

	[Tooltip("Determines the strength of this unit batch")]
	[field: SerializeField] public int CombatValue { get; set; }

	[Tooltip("The amount of units in a combat batch of given enemy type")]
	[field: SerializeField] public int UnitAmount { get; set; }

	[Tooltip("The Power level when the unit first appears")]
	[field: SerializeField] public int AppearPowerLevel { get; set; }
}
