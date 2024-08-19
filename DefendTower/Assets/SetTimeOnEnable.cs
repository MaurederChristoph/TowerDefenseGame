using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeOnEnable : MonoBehaviour {
	[SerializeField]
	private TimeStop _timeStop;
	private void OnEnable() {
		_timeStop.SetTimeScale(0);
	}

	private void OnDisable() {
		_timeStop.SetTimeScale(1);
	}
}
