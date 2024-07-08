using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base data storage for all units
/// </summary>
public class ScriptableUnit : ScriptableObject {
    [Tooltip("The unit that will be instantiated")]
    [field: SerializeField] public UnitBase UnitPrefab { get; private set; }
    [Tooltip("The Starting health of an unit")]
    [field: SerializeField] public int MaxHealth { get; private set; } = 100;
    [Tooltip("Represents the damage a unit will do")]
    [field: SerializeField] public int Power { get; private set; } = 5;
    [Tooltip("The amount of attacks a unit will do per second")]
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;
    [Tooltip("The distance the unit can shoot")]
    [field: SerializeField] public float Range { get; private set; } = 5f;
    [Tooltip("The target faction of basic attacks")]
    [field: SerializeField] public Faction AttackTargets { get; private set; }
    [Tooltip("How the unit will choose it's target for basic attacks")]
    [field: SerializeField] public TargetingStrategyType AttackTargetingStrategy { get; private set; }
}
