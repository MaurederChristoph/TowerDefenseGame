using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stores data for the power progression of the game
/// </summary>
public class ScriptableCombatProgression : ScriptableObject {
    /// <summary>
    /// Represent the data for a given power level
    /// </summary>
    [field: SerializeField] public List<CombatData> CombatData { get; set; }
}

/// <summary>
/// Stores the specific data for the enemies of a level
/// </summary>
[Serializable]
public class CombatData {

    [Tooltip("The power level when the data will be pulled")]
    [field: SerializeField] public int PowerLevel { get; set; }

    [Tooltip("The total strength of the combined unit power")]
    [field: SerializeField] public int CombatStrength { get; set; }

    [Tooltip("The minimum times the new unit will appear in the given level")]
    [field: SerializeField] public int MinAmountOfNewUnit { get; set; }

    [Tooltip("The maximum times the new unit will appear in the given level")]
    [field: SerializeField] public int MaxAmountOfNewUnit { get; set; }

    [Tooltip("Hom many old units will be included in the current levels enemy generation")]
    [field: SerializeField] public int OldUnitRange { get; set; }
}
