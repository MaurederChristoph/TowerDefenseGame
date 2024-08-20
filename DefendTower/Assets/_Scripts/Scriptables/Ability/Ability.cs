using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Stores the overarching ability
/// </summary>
[CreateAssetMenu(menuName = "Ability", fileName = "_Ability")]
public class Ability : ScriptableObject {
    [Tooltip("The name of the ability")]
    [field: SerializeField] public string Name { get; private set; }
    [Tooltip("The description of the ability")]
    [field: SerializeField] public string Description { get; private set; }
    [Tooltip("the effects that will be added to a unit if chosen")]
    [field: SerializeField] public List<ScriptableAbilityEffect> Effects { get; private set; }
}
