using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour {
	[SerializeField] private int _health = 3;
	[SerializeField] private Slider _slider;
	[SerializeField] private GameObject _showLostScreen;

	private void Start() {
		_slider.maxValue = _health;
		_slider.value = _health;
	}
	private void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent<EnemyBase>(out _)) return;
		Destroy(other);
		_health--;
		if (_health == 0) {
			_showLostScreen.SetActive(true);
		} else {
			_slider.value = _health;
		}
	}
}
