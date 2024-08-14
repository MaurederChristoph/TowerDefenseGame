using System;
using UnityEngine;
using UnityEngine.Serialization;
public class LevelUp : MonoBehaviour {
	[SerializeField] private UnitBase _unit;
	[SerializeField] private GameObject _levelUpBanner;

	public void IncreaseStat(string stat) {
		if (!Enum.TryParse(stat, out StatType statType)) return;
		var statValue = 0;
		switch(statType) {
			case StatType.Str:
				_unit.Stats.Str.Change(1);
				statValue = _unit.Stats.Str.BaseStat;
				break;
			case StatType.Dex:
				_unit.Stats.Dex.Change(1);
				statValue = _unit.Stats.Dex.BaseStat;
				break;
			case StatType.Con:
				_unit.Stats.Con.Change(1);
				statValue = _unit.Stats.Con.BaseStat;
				break;
			case StatType.Int:
				_unit.Stats.Int.Change(1);
				statValue = _unit.Stats.Int.BaseStat;
				break;
			case StatType.Cha:
				_unit.Stats.Cha.Change(1);
				statValue = _unit.Stats.Cha.BaseStat;
				break;
			case StatType.Wis:
				_unit.Stats.Wis.Change(1);
				statValue = _unit.Stats.Wis.BaseStat;
				break;
		}
		if (statValue is 5 or 10) {
			GameManager.Instance.AbilitySelectionManager.HandleAbilitySelection(statType, statValue, (TowerBase)_unit);
		}
	}


	private void Update() {
		if (Input.GetKeyDown(KeyCode.L)) {
			_levelUpBanner.SetActive(!_levelUpBanner.activeSelf);
		}
	}
}
