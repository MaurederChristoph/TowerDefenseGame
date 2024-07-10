using UnityEngine;

/// <summary>
/// Data storage for all towers
/// </summary>
[CreateAssetMenu(menuName = "Tower", fileName = "Tower")]
public class ScriptableTower : ScriptableUnit {
    [Tooltip("The target faction of basic attacks")]
    [field: SerializeField] public Faction SpellTargets { get; private set; }
    [Tooltip("How the unit will choose it's target for spells")]
    [field: SerializeField] public TargetingStrategyType SpellTargetingStrategy { get; private set; }
    [Tooltip("The default projectile the units will shoot")]
    [field: SerializeField] public ScriptableProjectile SpellProjectile { get; private set; }
    [Tooltip("How much mana is needed to create cast a spell")]
    [field: SerializeField] public int MaxMana { get; private set; }
    [Tooltip("How much mana the tower generates per second")]
    [field: SerializeField] public float ManaRegeneration { get; private set; }
    [Tooltip("How Long it takes for the tower to cast its spell")]
    [field: SerializeField] public float CastingSpeed { get; private set; }
    [Tooltip("The starting stats of the Tower")]
    [field: SerializeField] public Stats Stats { get; private set; }
}
