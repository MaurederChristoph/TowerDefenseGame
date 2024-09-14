using System;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

/// <summary>
/// Data storage for tower stats
/// </summary>
[Serializable]
public class Stats {
	/// <summary>
	/// Represents the Strength stat.
	/// </summary>
	public Stat Str { get; private set; } = new(StatType.Str);

	/// <summary>
	/// Represents the Dexterity stat.
	/// </summary>
	public Stat Dex { get; private set; } = new(StatType.Dex);

	/// <summary>
	/// Represents the Constitution stat.
	/// </summary>
	public Stat Con { get; private set; } = new(StatType.Con);

	/// <summary>
	/// Represents the Intelligence stat.
	/// </summary>
	public Stat Int { get; private set; } = new(StatType.Int);

	/// <summary>
	/// Represents the Charisma stat.
	/// </summary>
	public Stat Cha { get; private set; } = new(StatType.Cha);

	/// <summary>
	/// Represents the Wisdom stat.
	/// </summary>
	public Stat Wis { get; private set; } = new(StatType.Wis);

	/// <summary>
	/// The changes applied to stats, as defined in a scriptable object.
	/// </summary>
	private ScriptableStatChanges _statChanges;

	/// <summary>
	/// Initializes the stats and sets up listeners for stat changes.
	/// </summary>
	public void InitStats() {
		_statList = new List<Stat> { Str, Dex, Con, Int, Cha, Wis };
		foreach(var stat in _statList) {
			stat.Init();
		}
		_statChanges = ResourceSystem.Instance.GetScriptableStatChanges();
		Cha.AddBaseStatChangeListener(HandleBaseChaChange);
		Cha.AddTempStatChangeListener(HandleTempChaChange);
	}

	/// <summary>
	/// Handles temporary Charisma stat changes.
	/// </summary>
	/// <param name="changeValue">The amount by which Charisma changes.</param>
	/// <param name="time">The duration of the temporary change.</param>
	private void HandleTempChaChange(int changeValue, float time) {
		var rnd = new Random();
		for(var i = 0;i < (int)_statChanges.Cha.Amount;i++) {
			_statList[rnd.Next(0, _statList.Count)].Change(changeValue, time);
		}
	}

	/// <summary>
	/// Handles base Charisma stat changes, distributing the changes to other stats based on conditions.
	/// </summary>
	/// <param name="changeValue">The amount by which Charisma changes.</param>
	private void HandleBaseChaChange(int changeValue) {
		var rnd = new Random();
		for(var i = 0;i < rnd.Next(1, (int)_statChanges.Cha.Amount + 1);i++) {
			List<Stat> possibleTarget = _statList.Where(s => s.BaseStat < 9 && s.BaseStat != 4 && s.StatType != StatType.Cha).ToList();
			if (possibleTarget.Count == 0) {
				return;
			}
			var change = rnd.Next(0, possibleTarget.Count);
			possibleTarget[change].Change(changeValue);
		}
	}

	/// <summary>
	/// List of all stats.
	/// </summary>
	public List<Stat> _statList = null;

	/// <summary>
	/// Retrieves a copy of the stat list.
	/// </summary>
	/// <returns>A list of stats.</returns>
	public List<Stat> GetStatList() => _statList.ToList();

	/// <summary>
	/// Retrieves a stat based on its type.
	/// </summary>
	/// <param name="statType">The type of stat to retrieve.</param>
	/// <returns>The stat corresponding to the given type.</returns>
	public Stat GetStatFromType(StatType statType) {
		return _statList.First(s => s.StatType == statType);
	}

	/// <summary>
	/// Retrieves the stat with the highest current value.
	/// </summary>
	/// <returns>The stat with the highest value.</returns>
	public Stat GetHighestStat() {
		return _statList.OrderByDescending(s => s.Value).First();
	}

	/// <summary>
	/// Retrieves the stat with the highest base value.
	/// </summary>
	/// <returns>The stat with the highest base value.</returns>
	public Stat GetHighestBaseStat() {
		return _statList.OrderByDescending(s => s.BaseStat).First();
	}
}
