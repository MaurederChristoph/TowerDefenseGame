using System;
using UnityEngine;
/// <summary>
/// Increase Stats of target
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Increase Stat", fileName = "ChangeStat")]
public class ChangeStat : ScriptableAbilityEffect {
    [field: SerializeField] public EffectAmount StatChange { get; private set; }
    [field: SerializeField] public StatChangeStrategy Strategy { get; private set; }
    [field: SerializeField] public float Time { get; private set; } = -1;
    public override void ApplyEffect(ProjectileInfo _, UnitBase origin, UnitBase target) {
        if(target is not TowerBase tower) {
            return;
        }
        var value = StatChange.GetIntValue(origin);

        switch(Strategy) {
            case StatChangeStrategy.IncreaseHighest:
                tower.Stats.GetHighestStat().Change(value, Time);
                break;
            case StatChangeStrategy.DecreaseHighest:
                tower.Stats.GetHighestStat().Change(-value, Time);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }
}
