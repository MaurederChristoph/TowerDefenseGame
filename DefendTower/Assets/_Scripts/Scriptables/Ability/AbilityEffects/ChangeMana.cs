using UnityEngine;
/// <summary>
/// Changes the amount of mana a unit has  
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Change Mana", fileName = "ChangeMana")]
public class ChangeMana : ScriptableAbilityEffect {
    [field: SerializeField] public EffectAmount ManaGain { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        if(target is TowerBase tower) {
            tower.IncreaseMana(ManaGain.GetIntValue(origin));
        }
    }
}
