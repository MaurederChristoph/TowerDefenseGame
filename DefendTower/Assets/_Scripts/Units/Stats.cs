using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

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

	private ScriptableStatChanges _statChanges;
	public void InitStats() {
		_statList = new List<Stat> { Str, Dex, Con, Int, Cha, Wis };
		foreach(var stat in _statList) {
			stat.Init();
		}
		_statChanges = ResourceSystem.Instance.GetScriptableStatChanges();
		Cha.AddBaseStatChangeListener(HandleBaseChaChange);
		Cha.AddTempStatChangeListener(HandleTempChaChange);
	}
	private void HandleTempChaChange(int changeValue, float time) {
		var rnd = new Random();
		for(var i = 0;i < (int)_statChanges.Cha.Amount;i++) {
			_statList[rnd.Next(0, _statList.Count)].Change(changeValue, time);
		}
	}

	private void HandleBaseChaChange(int changeValue) {
		var rnd = new Random();
		for(var i = 0;i < rnd.Next(1, (int)_statChanges.Cha.Amount + 1);i++) {
			List<Stat> possibleTarget = _statList.Where(s => s.BaseStat < 9
			                                                 && s.BaseStat != 4
			                                                 && s.StatType != StatType.Cha).ToList();
			if (possibleTarget.Count == 0) {
				return;
			}
			var change = rnd.Next(0, possibleTarget.Count);
			possibleTarget[change].Change(changeValue);
		}
	}

	public List<Stat> _statList = null;

	public List<Stat> GetStatList() => _statList.ToList();  
	public Stat GetStatFromType(StatType statType) {
		return _statList.First(s => s.StatType == statType);
	}
	public Stat GetHighestStat() {
		return _statList.OrderByDescending(s => s.Value).First();
	}
	public Stat GetHighestBaseStat() {
		return _statList.OrderByDescending(s => s.BaseStat).First();
	}
}
