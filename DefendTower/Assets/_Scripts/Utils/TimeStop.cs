using UnityEngine;
/// <summary>
/// Stops the time scale 
/// </summary>
public class TimeStop : MonoBehaviour {
	/// <summary>
	/// changes the time scale
	/// </summary>
	/// <param name="timeScale">The new time scale</param>
	public void SetTimeScale(int timeScale) {
		Time.timeScale = timeScale;
	}
}
