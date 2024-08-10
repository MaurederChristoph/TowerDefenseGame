using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityDistribution", fileName = "AbilityDistribution")]
public class AbilityDistribution : ScriptableObject {
    public List<Level> Str = new();
    public List<Level> Dex = new();
    public List<Level> Con = new();
    public List<Level> Cha = new();
    public List<Level> Wis = new();
    public List<Level> Int = new();
}

[Serializable]
public class Level {
    public int AbilityLevel;
    public List<Ability> Abilities;
}
