using UnityEngine;
/// <summary>
/// Revives the target after some time
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Revive", fileName = "Revive")]
public class Revive : ScriptableAbilityEffect {

    [Tooltip("The cooldown of the revive")]
    [field: SerializeField] public EffectAmount Cooldown { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(origin.CreationTime + Cooldown.GetIntValue(origin) > Time.time) {
            origin.ResetUnit();
        }
    }
}
