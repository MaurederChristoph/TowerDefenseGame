using System;
using UnityEngine;
public class ScriptableStatChanges : ScriptableObject {
    [field: SerializeField] public StatChange Str { get; private set; }
    [field: SerializeField] public StatChange Dex { get; private set; }
    [field: SerializeField] public StatChange Con { get; private set; }
    [field: SerializeField] public StatChange Int { get; private set; }
    [field: SerializeField] public StatChange Cha { get; private set; }
    [field: SerializeField] public StatChange Wis { get; private set; }
}

[Serializable]
public class StatChange {
    [field: SerializeField] public StatType StatType { get; private set; }
    [field: SerializeField] public float Change { get; private set; }
}
