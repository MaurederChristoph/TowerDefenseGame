using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// Slows down the unit when entering the range of the unit
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Slow", fileName = "Slow")]
public class Slow : ScriptableAbilityEffect {
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        target.ApplySpeedPenalty();
        origin.AddOnEnemyEnterRangeListener(RemoveSlow);
        return;

        void RemoveSlow(UnitBase leftEnemy) {
            if(leftEnemy != target) {
                return;
            }
            target.ResetSpeed();
            origin.RemoveOnEnemyLeaveRangeListener(RemoveSlow);
        }
    }
}
