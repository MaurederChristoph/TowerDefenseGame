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
public struct CombatDataInfo {
    [Tooltip("The type of enemy associated with this combat data")]
    public ScriptableEnemy scriptableEnemy;

    [Tooltip("The icon representing the enemy in combat UI")]
    public Sprite Icon;

    [Tooltip("Determines the strength of this unit batch")]
    public int CombatValue;

    [Tooltip("The amount of units in a combat batch of given enemy type")]
    public int UnitAmount;
}
