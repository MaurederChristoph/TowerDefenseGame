using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Provides methods to call actions with various types of delays
/// </summary>
public class DelayedActionHandler : MonoBehaviour {
    /// <summary>
    /// Calls the specified action after a delay of the specified number of seconds
    /// </summary>
    /// <param name="action">The action to be called after the delay</param>
    /// <param name="seconds">The delay duration in seconds</param>
    public void CallAfterSeconds(Action action, float seconds) {
        StartCoroutine(CallAfterSecondsCoroutine(action, seconds));
    }

    /// <summary>
    /// Coroutine that handles the delay of the specified action by the specified number of seconds
    /// </summary>
    /// <param name="action">The action to be called after the delay</param>
    /// <param name="seconds">The delay duration in seconds</param>
    /// <returns>IEnumerator for coroutine management</returns>
    private IEnumerator CallAfterSecondsCoroutine(Action action, float seconds) {
        yield return new WaitForSeconds(seconds);
        action?.Invoke();
    }

    /// <summary>
    /// Calls the specified action after the specified number of fixed frames
    /// </summary>
    /// <param name="action">The action to be called after the delay</param>
    /// <param name="frameCount">The number of fixed frames to wait before calling the action</param>
    public void CallAfterFixedFrames(Action action, int frameCount = 1) {
        StartCoroutine(CallAfterFixedFramesCoroutine(action, frameCount));
    }

    /// <summary>
    /// Coroutine that handles the delay of the specified action by the specified number of fixed frames
    /// </summary>
    /// <param name="action">The action to be called after the delay</param>
    /// <param name="frameCount">The number of fixed frames to wait before calling the action</param>
    /// <returns>IEnumerator for coroutine management</returns>
    private IEnumerator CallAfterFixedFramesCoroutine(Action action, int frameCount) {
        while(frameCount > 0) {
            yield return new WaitForFixedUpdate();
            frameCount--;
        }
        action?.Invoke();
    }

    /// <summary>
    /// Calls the specified action at the end of the current frame
    /// </summary>
    /// <param name="action">The action to be called at the end of the frame</param>
    public void CallAtEndOfFrame(Action action) {
        StartCoroutine(CallAtEndOfFrameCoroutine(action));
    }

    /// <summary>
    /// Coroutine that handles the delay of the specified action until the end of the current frame
    /// </summary>
    /// <param name="action">The action to be called at the end of the frame.</param>
    /// <returns>IEnumerator for coroutine management</returns>
    private IEnumerator CallAtEndOfFrameCoroutine(Action action) {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }
}
