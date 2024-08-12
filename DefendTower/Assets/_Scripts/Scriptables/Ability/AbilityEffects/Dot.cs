using UnityEngine;
/// <summary>
/// Deals damage over time
/// </summary>
[CreateAssetMenu(menuName = "Ability Effects/Dot", fileName = "DOT")]
public class Dot : ScriptableAbilityEffect {
    [Tooltip("The amount of damage the target will reserve each tick")]
    [field: SerializeField] public EffectAmount Damage { get; private set; }
    [Tooltip("The amount of seconds that are in between each tick of damage")]
    [field: SerializeField] public float Delay { get; private set; } = 0.4f;
    [Tooltip("How many ticks of damage there are")]
    [field: SerializeField] public int TotalTicks { get; private set; } = 3;
    public override void ApplyEffect(ProjectileInfo projectile, UnitBase origin, UnitBase target) {
        var ticks = 0;
        var delayedActionHandler = GameManager.Instance.DelayedActionHandler;
        DotDamage();
        return;

        void DotDamage(string _ = "") {
            if(ticks > TotalTicks) return;
            origin.ChangeHealth(Damage.GetIntValue(origin),origin);
            ticks++;
            delayedActionHandler.CallAfterSeconds(DotDamage, Delay);
        }
    }
}
