using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem> {
	private ScriptableStatChanges _statChanges;
	private AbilityDistribution _abilityDistribution;
	private ScriptableCombatProgression _scriptableCombatProgression;
	private ScriptableEnemyCombatData _scriptableEnemyCombatData;
	protected override void Awake() {
		base.Awake();
		AssembleResources();
	}

	private void AssembleResources() {
		_statChanges = Resources.Load<ScriptableStatChanges>("Stat Changes");
		_abilityDistribution = Resources.Load<AbilityDistribution>("AbilityDistribution");
		_scriptableCombatProgression = Resources.Load<ScriptableCombatProgression>("Scriptable Combat Progression");
		_scriptableEnemyCombatData = Resources.Load<ScriptableEnemyCombatData>("EnemyCombatData");
	}
	public ScriptableStatChanges GetScriptableStatChanges() => _statChanges;
	public AbilityDistribution GetScriptableAbilityDistribution() => _abilityDistribution;
	public ScriptableCombatProgression GetScriptableCombatProgression() => _scriptableCombatProgression;
	public ScriptableEnemyCombatData GetScriptableEnemyCombatData() => _scriptableEnemyCombatData;
}
