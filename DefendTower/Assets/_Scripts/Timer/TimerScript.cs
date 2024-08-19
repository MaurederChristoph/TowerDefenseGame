using System;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class TimerScript : MonoBehaviour {
	public TMP_Text _timerText;
	public int Level { get; private set; } = 1;

	private float _timeRemaining = 900f;
	private float _nextLevelTime = 840f;

	private Action<int> _onPowerLevelChange;

	private void Start() {
		_onPowerLevelChange?.Invoke(1);
	}
	public void Update() {
		if (_timeRemaining > 0) {
			_timeRemaining -= Time.deltaTime;
			if (_timeRemaining <= _nextLevelTime) {
				Level++;
				_onPowerLevelChange?.Invoke(Level);
				_nextLevelTime -= 60f;
			}
			DisplayTime(_timeRemaining);
		} else {
			_timeRemaining = 0;
			DisplayTime(_timeRemaining);
			_onPowerLevelChange?.Invoke(16);
		}
	}

	public void DisplayTime(float timeToDisplay) {
		timeToDisplay += 1;
		float minutes = Mathf.FloorToInt(timeToDisplay / 60);
		float seconds = Mathf.FloorToInt(timeToDisplay % 60);
		_timerText.text = $"{minutes:00}:{seconds:00}";
	}
	public void AddOnPowerLevelChangeListener(Action<int> listener) {
		_onPowerLevelChange += listener;
	}
	public void RemoveOnPowerLevelChangeListener(Action<int> listener) {
		_onPowerLevelChange -= listener;
	}
}
