using UnityEngine;
/// <summary>
/// Applies more damage to the enemies
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Vulnerable", fileName = "Vulnerable")]
public class Vulnerable : ScriptableAbilityEffect {
    [field: SerializeField] public float Duration { get; private set; }
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        target.AddVulnerable();
        GameManager.Instance.DelayedActionHandler.CallAfterSeconds(target.RemoveVulnerable, Duration);
    }
}
