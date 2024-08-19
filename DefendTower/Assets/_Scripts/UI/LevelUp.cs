using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelUp : MonoBehaviour {
	[SerializeField] private UnitBase _unit;
	[SerializeField] private GameObject _levelUpBanner;
	[SerializeField] private TMP_Text[] _stats;
	[SerializeField] private Sprite[] _towerSprites;
	[SerializeField] private Sprite[] _statBoarderSprites;
	[SerializeField] private Image[] _statBoarderSpriteRenderer;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private GameObject _showLevelUp;

	private DelayedActionHandler _delayedActionHandler;
	private GameManager _gameManager;

	private bool _firstMaxed;
	private int _pointsAvailable = 0;
	private int _totalLevels;
	private StatType _highestStat = StatType.None;

	private bool CanLevelUp {
		get => _pointsAvailable > 0;
	}

	private const int _startingPoints = 1;
	private bool _canCloseLevelUp = true;
	private const float _earlyLevelUpTimer = 12;
	private const float _levelUpTimer = 27;
	private void Start() {
		foreach(var stat in _unit.Stats.GetStatList()) {
			stat.AddBaseStatChangeListener(UpdateTower);
		}
		_gameManager = GameManager.Instance;
		_delayedActionHandler = _gameManager.DelayedActionHandler;
		if (_gameManager.CurrentPowerLevel == 1) {
			_pointsAvailable = _startingPoints;
			_totalLevels = _startingPoints;
			_delayedActionHandler.CallAfterSeconds(DisplayLevelUp, _earlyLevelUpTimer);
		} else {
			_pointsAvailable = 1;
			_totalLevels = 1;
			_delayedActionHandler.CallAfterSeconds(DisplayLevelUp, _levelUpTimer);
		}
	}


	private void LateUpdate() {
		if (Input.GetKeyUp(KeyCode.Mouse0) && _canCloseLevelUp) {
			_levelUpBanner.SetActive(false);
		} else if (Input.GetKeyUp(KeyCode.Mouse0)) {
			_canCloseLevelUp = true;
		}
	}

	private void OnMouseDown() {
		if (!CanLevelUp) return;
		ShowLevelSelection();
		_canCloseLevelUp = false;
	}
	public void IncreaseStat(string stat) {
		_pointsAvailable--;
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
			if (statValue is >= 10 and <= 15 && !_firstMaxed) {
				_firstMaxed = true;
				_pointsAvailable++;
				IncreaseStat(stat);
			}
			GameManager.Instance.AbilitySelectionManager.HandleAbilitySelection(statType, statValue, (TowerBase)_unit);
		}
		UpdateTower();
		if (_pointsAvailable > 0) {
			_canCloseLevelUp = false;
		} else {
			_showLevelUp.SetActive(false);
		}
	}

	private void UpdateTower(int _ = 0) {
		var stat = _unit.Stats.GetHighestBaseStat();
		if (_highestStat != stat.StatType) {
			_highestStat = stat.StatType;
			_spriteRenderer.sprite = _towerSprites[(int)_highestStat];
		}
		foreach(var s in _unit.Stats.GetStatList()) {
			_statBoarderSpriteRenderer[(int)s.StatType].sprite = s.BaseStat switch {
				var n when n == stat.BaseStat => _statBoarderSprites[3],
				0 => _statBoarderSprites[0],
				>= 1 and < 4 => _statBoarderSprites[1],
				>= 5 => _statBoarderSprites[2],
				_ => _statBoarderSpriteRenderer[(int)s.StatType].sprite
			};
			_stats[(int)s.StatType].text = s.BaseStat + "";
		}
	}

	private void DisplayLevelUp(string _ = "") {
		_pointsAvailable++;
		_showLevelUp.SetActive(true);
		if (_totalLevels < 19) {
			_totalLevels++;
			_delayedActionHandler.CallAfterSeconds(DisplayLevelUp,
				_gameManager.CurrentPowerLevel != 1 ? _levelUpTimer : _earlyLevelUpTimer);
		}
	}

	private void ShowLevelSelection() {
		foreach(var s in _unit.Stats.GetStatList().Where(s => s.BaseStat >= 10)) {
			_statBoarderSpriteRenderer[(int)s.StatType].GetComponent<Button>().interactable = false;
		}
		_levelUpBanner.SetActive(true);
	}
}
