using UnityEngine;
/// <summary>
/// Returns scriptable object 
/// </summary>
public class ResourceSystem : Singleton<ResourceSystem> {
    /// <summary>
    /// Represents the stat changes
    /// </summary>
    private ScriptableStatChanges _statChanges;
    /// <summary>
    /// Represents the abilities of the stats
    /// </summary>
    private AbilityDistribution _abilityDistribution;
    /// <summary>
    /// Represents the combat progression 
    /// </summary>
    private ScriptableCombatProgression _scriptableCombatProgression;
    /// <summary>
    /// Represent the enemy combat data
    /// </summary>
    private ScriptableEnemyCombatData _scriptableEnemyCombatData;
    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }
    /// <summary>
    /// Load all resources
    /// </summary>
    private void AssembleResources() {
        _statChanges = Resources.Load<ScriptableStatChanges>("Stat Changes");
        _abilityDistribution = Resources.Load<AbilityDistribution>("AbilityDistribution");
        _scriptableCombatProgression = Resources.Load<ScriptableCombatProgression>("Scriptable Combat Progression");
        _scriptableEnemyCombatData = Resources.Load<ScriptableEnemyCombatData>("EnemyCombatData");
    }
    /// <summary>
    /// Get stat changes
    /// </summary>
    /// <returns>Returns Scriptable Stat Changes</returns>
    public ScriptableStatChanges GetScriptableStatChanges() => _statChanges;
    /// <summary>
    /// Get Ability distribution
    /// </summary>
    /// <returns>Ability Distribution</returns>
    public AbilityDistribution GetScriptableAbilityDistribution() => _abilityDistribution;
    /// <summary>
    /// Get Combat progression
    /// </summary>
    /// <returns>Scriptable Combat Progression</returns>
    public ScriptableCombatProgression GetScriptableCombatProgression() => _scriptableCombatProgression;
    /// <summary>
    /// Get enemy Combat data
    /// </summary>
    /// <returns>Scriptable Enemy Combat Data</returns>
    public ScriptableEnemyCombatData GetScriptableEnemyCombatData() => _scriptableEnemyCombatData;
}
