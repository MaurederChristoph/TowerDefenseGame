using UnityEngine;
/// <summary>
/// Changes the speed by which a unit attacks
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Cast Speed", fileName = "Change Cast Speed")]
public class ChangeCastSpeed : ScriptableAbilityEffect {

    [Tooltip("Amount that will be added ot the cast speed")]
    [field: SerializeField] public EffectAmount CastSpeedMultiplier { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(target is TowerBase tower) {
            tower.ChangeCastingSpeed(CastSpeedMultiplier.GetFloatValue(origin));
        }
    }
}
