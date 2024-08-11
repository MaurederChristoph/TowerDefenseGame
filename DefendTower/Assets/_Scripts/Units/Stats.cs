using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Data storage for tower stats
/// </summary>
[Serializable]
public class Stats {
    public Stat Str { get; private set; } = new(StatType.Str);
    public Stat Dex { get; private set; } = new(StatType.Dex);
    public Stat Con { get; private set; } = new(StatType.Con);
    public Stat Int { get; private set; } = new(StatType.Int);
    public Stat Cha { get; private set; } = new(StatType.Cha);
    public Stat Wis { get; private set; } = new(StatType.Wis);

    public Stats() {
        _statList = new List<Stat> { Str, Dex, Con, Int, Cha, Wis };
    }

    private List<Stat> _statList = null;

    public Stat GetStatFromType(StatType statType) {
        return _statList.First(s => s.StatType == statType);
    }
    public Stat GetHighestStat() {
        return _statList.OrderByDescending(s => s.BaseStat).First();
    }
}
