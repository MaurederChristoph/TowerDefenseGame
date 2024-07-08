using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data storage for all towers
/// </summary>
public class ScriptableTower : ScriptableUnit {
    [Tooltip("The target faction of spells")]
    [field: SerializeField] public Faction SpellTargets { get; private set; }
    [Tooltip("How the unit will choose it's target for spells")]
    [field: SerializeField] public TargetingStrategy SpellTargetingStrategy { get; private set; }
}
