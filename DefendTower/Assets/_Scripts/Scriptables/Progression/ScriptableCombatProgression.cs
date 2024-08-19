using System;
using System.Collections.Generic;
using UnityEngine;
public class ScriptableCombatProgression : ScriptableObject {
	[field: SerializeField] public List<CombatData> CombatData { get; set; }
}

[Serializable]
public class CombatData {
	[field: SerializeField] public int PowerLevel { get; set; }
	[field: SerializeField] public int CombatStrength { get; set; }
	[field: SerializeField] public int MinAmountOfNewUnit { get; set; }
	[field: SerializeField] public int MaxAmountOfNewUnit { get; set; }
	[field: SerializeField] public int OldUnitRange { get; set; }
}
