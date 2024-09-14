using System;
/// <summary>
/// Stores the stat changes that stat level ups grant
/// </summary>
public static class StatChanges {
    /// <summary>
    /// Get the correct stat change
    /// </summary>
    /// <param name="stats">The stats that will be called to get the correct stat</param>
    /// <param name="statType">The stat type that will be changed</param>
    /// <returns>The correct amount of a changed stat</returns>
    public static float GetStatChange(Stats stats, StatType statType) {
        var statChange = ResourceSystem.Instance.GetScriptableStatChanges();
        return statType switch {
            StatType.Str => statChange.Str.Amount * stats.Str.Value,
            StatType.Dex => statChange.Dex.Amount * stats.Dex.Value,
            StatType.Con => statChange.Con.Amount * stats.Con.Value,
            StatType.Int => statChange.Int.Amount * stats.Int.Value,
            StatType.Cha => statChange.Cha.Amount * stats.Cha.Value,
            StatType.Wis => statChange.Wis.Amount * stats.Wis.Value,
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
}
