using UnityEngine;
/// <summary>
/// If a target enters the range of this effect they can only attack the 
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Taunt", fileName = "Taunt")]
public class Taunt : ScriptableAbilityEffect {
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        target.ChangeTargetingStrategyTemporary(TargetingStrategy.Taunt);
        target.ChangeCurrentAttackTarget(origin);
        origin.AddOnEnemyLeaveRangeListener(RemoveTaunt);
        return;

        void RemoveTaunt(UnitBase leftEnemy) {
            if(leftEnemy != target) {
                return;
            }
            target.ResetTargetingStrategy();
            origin.RemoveOnEnemyLeaveRangeListener(RemoveTaunt);
        }
    }
}
