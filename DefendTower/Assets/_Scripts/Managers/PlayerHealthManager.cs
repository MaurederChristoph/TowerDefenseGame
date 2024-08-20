using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Handels the player overall health
/// </summary>
public class PlayerHealthManager : MonoBehaviour {
	/// <summary>
	/// How much lives the player has
	/// </summary>
	[SerializeField] private int _health = 3;
	/// <summary>
	/// The slider of the health
	/// </summary>
	[SerializeField] private Slider _slider;
	/// <summary>
	/// The lost screen gameobject
	/// </summary>
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
