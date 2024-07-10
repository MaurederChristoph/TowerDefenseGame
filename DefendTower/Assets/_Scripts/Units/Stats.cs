using System;
using UnityEngine;
/// <summary>
/// Data storage for tower stats
/// </summary>
[Serializable]
public class Stats {
    [field: SerializeField] public Stat Str { get; private set; }
    [field: SerializeField] public Stat Dex { get; private set; }
    [field: SerializeField] public Stat Con { get; private set; }
    [field: SerializeField] public Stat Int { get; private set; }
    [field: SerializeField] public Stat Cha { get; private set; }
    [field: SerializeField] public Stat Wis { get; private set; }
}
