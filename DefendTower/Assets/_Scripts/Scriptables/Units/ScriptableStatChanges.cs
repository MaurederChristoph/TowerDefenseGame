using System;
using UnityEngine;
/// <summary>
/// Represents all number changes of stats
/// </summary>
public class ScriptableStatChanges : ScriptableObject {

    [Tooltip("How much basic attack power change will be effected.")]
    [field: SerializeField] public StatChange Str { get; private set; }
    [Tooltip("How much basic attack attackspeed change will be effected.")]
    [field: SerializeField] public StatChange Dex { get; private set; }
    [Tooltip("How much heatlh change will be effected.")]
    [field: SerializeField] public StatChange Con { get; private set; }
    [Tooltip("How much spell damage will be effected.")]
    [field: SerializeField] public StatChange Int { get; private set; }
    [Tooltip("How many stats will be increased.")]
    [field: SerializeField] public StatChange Cha { get; private set; }
    [Tooltip("How much the mana regeneration will be effected.")]
    [field: SerializeField] public StatChange Wis { get; private set; }
}

[Serializable]
public class StatChange {
    [Tooltip("What type of stat this is")]
    [field: SerializeField] public StatType StatType { get; private set; }
    [Tooltip("How much the stat will be changed")]
    [field: SerializeField] public float Amount { get; private set; }
}
