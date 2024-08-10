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
    public static Stat Str { get; private set; } = new(StatType.Str);
    public static Stat Dex { get; private set; } = new(StatType.Dex);
    public static Stat Con { get; private set; } = new(StatType.Con);
    public static Stat Int { get; private set; } = new(StatType.Int);
    public static Stat Cha { get; private set; } = new(StatType.Cha);
    public static Stat Wis { get; private set; } = new(StatType.Wis);

    private List<Stat> _statList = new() { Str, Dex, Con, Int, Cha, Wis };

    public Stat GetStatFromType(StatType statType) {
        return _statList.First(s => s.StatType == statType);
    }
    public Stat GetHighestStat() {
        return _statList.OrderByDescending(s => s.BaseStat).First();
    }
}
