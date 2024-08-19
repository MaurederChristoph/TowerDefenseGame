using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a changeable value
/// </summary>
[Serializable]
public class Stat {
	/// <summary>
	/// Called when the stat is changed
	/// </summary>
	private Action<int> _onStatChange;
	private Action<int, float> _onTempStatChange;
	private Action<int> _onBaseStatChange;

	/// <summary>
	/// Total stat value
	/// </summary>
	public int Value => BaseStat + TempStats.Sum();
	/// <summary>
	/// Permanent stat value
	/// </summary>
	[field: SerializeField] public int BaseStat { get; private set; }
	/// <summary>
	/// List of the current stat modifications
	/// </summary>
	private readonly Dictionary<string, int> _tempStatModifiers = new();
	public Stat(StatType statType) {
		StatType = statType;
	}

	/// <summary>
	/// Provides a copy of the current stat modifications
	/// </summary>
	public List<int> TempStats => _tempStatModifiers.Values.ToList();

	public StatType StatType { get; private set; }

	public void Init() {
		BaseStat = 0;
	}
	/// <summary>
	/// Changes the values of the stat
	/// </summary>
	/// <param name="value">The amount by which the stat will be changed</param>
	/// <param name="seconds">The duration of the change</param>
	public void Change(int value, float seconds = -1) {
		if (seconds > 0) {
			var key = GameManager.Instance.DelayedActionHandler.CallAfterSeconds(RemoveTempStatChange, seconds);
			_tempStatModifiers.Add(key, value);
			_onTempStatChange?.Invoke(value, seconds);
		} else {
			BaseStat += value;
			_onBaseStatChange?.Invoke(value);
		}
		_onStatChange?.Invoke(value);
	}
	/// <summary>
	/// Removes a temporary stat change
	/// </summary>
	/// <param name="key">The key of the stat change</param>
	private void RemoveTempStatChange(string key) {
		var oldValues = Value;
		_tempStatModifiers.Remove(key);
		_onStatChange?.Invoke(Value - oldValues);
	}

	/// <summary>
	/// Adds a method to an event that is called when the stat changes
	/// </summary>
	/// <param name="listener">The method that will be called</param>
	/// <param name="int">The amount by which the stat has been changed</param>>
	public void AddStatChangeListener(Action<int> listener) {
		_onStatChange += listener;
	}
	/// <summary>
	/// Removes a method of an event that is called when the stat changes
	/// </summary>
	/// <param name="listener">The method that will be removed</param>
	/// <param name="int">The amount by which the stat has been changed</param>>
	public void RemoveStatChangeListener(Action<int> listener) {
		_onStatChange -= listener;
	}

	/// <summary>
	/// Adds a method to an event that is called when a Temp stat is changes
	/// </summary>
	/// <param name="listener">The method that will be called</param>
	/// <param name="int">The amount by which the stat has been temporarily changed</param>>
	/// <param name="float">The time the stat will be changed</param>>
	public void AddTempStatChangeListener(Action<int, float> listener) {
		_onTempStatChange += listener;
	}
	/// <summary>
	/// Removes a method of an event that is called when a Temp stat is changes
	/// </summary>
	/// <param name="listener">The method that will be removed</param>
	/// <param name="int">The amount by which the stat has been temporarily changed</param>>
	/// <param name="float">The time the stat will be changed</param>>
	public void RemoveTempStatChangeListener(Action<int, float> listener) {
		_onTempStatChange -= listener;
	}

	/// <summary>
	/// Adds a method to an event that is called when a base stat is changes
	/// </summary>
	/// <param name="listener">The method that will be called</param>
	/// <param name="int">The amount by which the stat has been changed</param>>
	public void AddBaseStatChangeListener(Action<int> listener) {
		_onBaseStatChange += listener;
	}

	/// <summary>
	/// Removes a method of an event that is called when a Base stat is changes
	/// </summary>
	/// <param name="listener">The method that will be removed</param>
	/// <param name="int">The amount by which the stat has been changed</param>>
	public void RemoveBaseStatChangeListener(Action<int> listener) {
		_onBaseStatChange -= listener;
	}
}
