﻿using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Data storage for tower stats
/// </summary>
[Serializable]
public class Stats {
	public Stat Str { get; private set; } = new();
	public Stat Dex { get; private set; } = new();
	public Stat Con { get; private set; } = new();
	public Stat Int { get; private set; } = new();
	public Stat Cha { get; private set; } = new();
	public Stat Wis { get; private set; } = new();

	public Stats() {
		_statList = new List<Stat> { Str, Dex, Con, Int, Cha, Wis };
		foreach(var stat in _statList) {
			stat.Init();
		}
	}

	private List<Stat> _statList = null;

	public Stat GetStatFromType(StatType statType) {
		return _statList.First(s => s.StatType == statType);
	}
	public Stat GetHighestStat() {
		return _statList.OrderByDescending(s => s.BaseStat).First();
	}
}
