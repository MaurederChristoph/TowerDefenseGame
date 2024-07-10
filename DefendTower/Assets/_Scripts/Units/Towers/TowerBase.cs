using System.Collections.Generic;

/// <summary>
/// Base class for all towers 
/// </summary>
public class TowerBase : UnitBase {
    /// <summary>
    /// The target faction of spells
    /// </summary>
    public Faction SpellTargets { get; private set; } = Faction.Enemy;
    /// <summary>
    /// The current spell the tower is casting
    /// </summary>
    public ScriptableProjectile SpellProjectile { get; private set; }
    /// <summary>
    /// How the unit will choose it's target for spells
    /// </summary>
    public TargetingStrategy SpellTargetingStrategy { get; private set; }
    /// <summary>
    /// How much mana is needed to create cast a spell
    /// </summary>
    public int MaxMana { get; private set; }
    /// <summary>
    /// How much mana the tower generates per second
    /// </summary>
    public float ManaRegeneration { get; private set; }
    /// <summary>
    /// How Long it takes for the tower to cast its spell
    /// </summary>
    public float CastingSpeed { get; private set; }
    /// <summary>
    /// The stats of the tower
    /// </summary>
    public Stats Stats { get; private set; }
    /// <summary>
    /// The stats that are changed temporary and displayed in the UI
    /// </summary>
    public List<Stats> TempStats { get; private set; }
    /// <summary>
    /// Translates tower properties form scriptable tower object to tower script
    /// </summary>
    /// <param name="unit">Scriptable unit</param>
    public override void InitUnit(ScriptableUnit unit) {
        base.InitUnit(unit);
        var tower = (ScriptableTower)unit;
        SpellProjectile = tower.SpellProjectile;
        SpellTargetingStrategy = TargetingStrategy.FromType(tower.SpellTargetingStrategy);
        MaxMana = tower.MaxMana;
        ManaRegeneration = tower.ManaRegeneration;
        CastingSpeed = tower.CastingSpeed;
        Stats = tower.Stats;
    }
}
