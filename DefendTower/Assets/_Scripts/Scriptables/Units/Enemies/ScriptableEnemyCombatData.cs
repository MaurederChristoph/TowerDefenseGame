using System;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableEnemyCombatData : ScriptableObject {
    [Tooltip("List of combat data entries for different enemy types.")]
    [field: SerializeField] public List<CombatData> CombatData { get; private set; } = new();
}

[Serializable]
public struct CombatData {
    [Tooltip("The type of enemy associated with this combat data")]
    public ScriptableEnemy scriptableEnemy;

    [Tooltip("The icon representing the enemy in combat UI")]
    public Sprite Icon;

    [Tooltip("Determines the strength of this unit batch")]
    public int CombatValue;

    [Tooltip("The amount of units in a combat batch of given enemy type")]
    public int UnitAmount;
}
