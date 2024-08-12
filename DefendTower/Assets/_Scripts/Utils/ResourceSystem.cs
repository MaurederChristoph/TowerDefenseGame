using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem> {
    private ScriptableStatChanges _statChanges;
    private AbilityDistribution _abilityDistribution;
    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources() {
        _statChanges = Resources.Load<ScriptableStatChanges>("Stat Changes");
        _abilityDistribution = Resources.Load<AbilityDistribution>("AbilityDistribution");
    }
    public ScriptableStatChanges GetScriptableStatChanges() => _statChanges;
    public AbilityDistribution GetScriptableAbilityDistribution() => _abilityDistribution;
}
