using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "AbilityDistribution", fileName = "AbilityDistribution")]
public class AbilityDistribution : ScriptableObject {
	public List<Level> Str = new();
	public List<Level> Dex = new();
	public List<Level> Con = new();
	public List<Level> Cha = new();
	public List<Level> Wis = new();
	public List<Level> Int = new();
	public List<Ability> GetAbilityList(StatType statType, int statAmount) {
		return statType switch {
			StatType.Str => statAmount >= 10
				? Str.First(l => l.AbilityLevel >= 10).Abilities
				: Str.First(l => l.AbilityLevel >= 5).Abilities,
			StatType.Dex => statAmount >= 10
				? Dex.First(l => l.AbilityLevel >= 10).Abilities
				: Dex.First(l => l.AbilityLevel >= 5).Abilities,
			StatType.Con => statAmount >= 10
				? Con.First(l => l.AbilityLevel >= 10).Abilities
				: Con.First(l => l.AbilityLevel >= 5).Abilities,
			StatType.Int => statAmount >= 10
				? Int.First(l => l.AbilityLevel >= 10).Abilities
				: Int.First(l => l.AbilityLevel >= 5).Abilities,
			StatType.Cha => statAmount >= 10
				? Cha.First(l => l.AbilityLevel >= 10).Abilities
				: Cha.First(l => l.AbilityLevel >= 5).Abilities,
			StatType.Wis => statAmount >= 10
				? Wis.First(l => l.AbilityLevel >= 10).Abilities
				: Wis.First(l => l.AbilityLevel >= 5).Abilities,
			_ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
		};
	}
}

[Serializable]
public class Level {
	public int AbilityLevel;
	public List<Ability> Abilities;
}
