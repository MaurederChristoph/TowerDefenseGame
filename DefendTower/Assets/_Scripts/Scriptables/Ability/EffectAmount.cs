using System;
using UnityEngine;

/// <summary>
/// Represents an effect amount for a projectile, which is calculated based on a specified stat type and/or value.
/// </summary>
[Serializable]
public class EffectAmount {
    [Tooltip("The stat that will be multiplied with the value if not 'None'")]
    [SerializeField] private StatType _stat = StatType.None;
    [Tooltip("The total value or the multiplier if the value is not 'None'")]
    [SerializeField] private float _value = 1;

    /// <summary>
    /// Calculates the correct numerical value of the <see cref="EffectAmount"/> class
    /// </summary>
    /// <param name="unit">The unit form where the Stats will be taken for the calculation of the total value</param>
    /// <returns>Numerical positive value of the <see cref="EffectAmount"/></returns>
    public int GetIntValue(UnitBase unit) {
        if(_stat == StatType.None) {
            return Mathf.RoundToInt(_value);
        }
        return Mathf.RoundToInt(_value * ((TowerBase)unit).Stats.GetStatFromType(_stat).Value);
    }

    public float GetFloatValue(UnitBase unit) {
        if(_stat == StatType.None) {
            return _value;
        }
        return _value * ((TowerBase)unit).Stats.GetStatFromType(_stat).Value;
    }
}
