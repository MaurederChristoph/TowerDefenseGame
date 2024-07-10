using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a changeable value
/// </summary>
[Serializable]
public class Stat {
    public Action<int> OnStatChange;
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

    /// <summary>
    /// Provides a copy of the current stat modifications
    /// </summary>
    /// 
    public List<int> TempStats => _tempStatModifiers.Values.ToList();

    public Stat(int baseValue) {
        BaseStat = baseValue;
    }

    public Stat() {
        BaseStat = 1;
    }

    /// <summary>
    /// Changes the values of the stat
    /// </summary>
    /// <param name="value">The amount by which the stat will be changed</param>
    /// <param name="seconds">The duration of the change</param>
    public void Change(int value, float seconds = -1) {
        if(seconds > 0) {
            var key = GameManager.Instance.DelayedActionHandler.CallAfterSeconds(RemoveTempStatChange, seconds);
            _tempStatModifiers.Add(key, value);
        } else {
            BaseStat = +value;
        }
        OnStatChange.Invoke(Value);
    }
    /// <summary>
    /// Removes a temporary stat change
    /// </summary>
    /// <param name="key">The key of the stat change</param>
    private void RemoveTempStatChange(string key) {
        _tempStatModifiers.Remove(key);
        OnStatChange.Invoke(Value);
    }

    /// <summary>
    /// Adds a method to an event that is called when the stat changes
    /// </summary>
    /// <param name="listener">The method that will be called</param>
    public void AddStatChangeListener(Action<int> listener) {
        OnStatChange += listener;
    }

    /// <summary>
    /// Removes a method of an event that is called when the stat changes
    /// </summary>
    /// <param name="listener">The method that will be removed</param>
    public void RemoveStatChangeListener(Action<int> listener) {
        OnStatChange += listener;
    }
}
