using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Represents a stat in all stats
/// </summary>
public class Stat {
    /// <summary>
    /// Total stat value
    /// </summary>
    public int Value => BaseStat + TempStats.Sum();

    /// <summary>
    /// Permanent stat value
    /// </summary>
    public int BaseStat { get; private set; }

    /// <summary>
    /// List of the current stat modifications
    /// </summary>
    private readonly List<int> _tempStatModifiers = new();

    /// <summary>
    /// Provides a copy of the current stat modifications
    /// </summary>
    public List<int> TempStats => GetCorrectTempStats().ToList();

    /// <summary>
    /// Removes all temporary effects w
    /// </summary>
    /// <returns></returns>
    private List<int> GetCorrectTempStats() {
        return null;
    }

    /// <summary>
    /// Changes the values of the stat
    /// </summary>
    /// <param name="value"></param>
    /// <param name="time"></param>
    public void Change(int value, float time) {
        if(time > 0) {
            _tempStatModifiers.Add(value);
            Task.Delay(TimeSpan.FromSeconds(time)).ContinueWith(t => _tempStatModifiers.Remove(value));
        } else {
            BaseStat = +value;
        }
    }
}

