using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSystem : Singleton<ResourceSystem> {
    private ScriptableStatChanges _statChanges;

    protected override void Awake() {
        base.Awake();
        AssembleResources();
    }

    private void AssembleResources() {
        _statChanges = Resources.Load<ScriptableStatChanges>("Stat Changes");
    }
    public ScriptableStatChanges GetScriptableStatChanges() => _statChanges;
}
