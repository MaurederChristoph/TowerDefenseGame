using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides methods to call actions with various types of delays
/// </summary>
public class DelayedActionHandler : MonoBehaviour {
    private readonly static Dictionary<string, Coroutine> _keys = new();

    /// <summary>
    /// Calls the specified action after a delay of the specified number of seconds
    /// </summary>
    /// <param name="action">The method that will be called after the delay</param>
    /// <param name="seconds">The delay after which the method will be called</param>
    /// <returns>Key of the created coroutine</returns>>
    public string CallAfterSeconds(Action<string> action, float seconds) {
        var key = GenerateKey();
        var coroutine = StartCoroutine(CallAfterSecondsCoroutine(action, seconds, key));
        _keys.Add(key, coroutine);
        return key;
    }

    /// <summary>
    /// Coroutine that handles the delay of the specified action by the specified number of seconds
    /// </summary>
    /// <param name="action">The method that will be called after the delay</param>
    /// <param name="seconds">The delay duration in seconds</param>
    /// <param name="key">The identifier of the coroutine class</param>
    /// <returns>IEnumerator for coroutine management</returns>
    private static IEnumerator CallAfterSecondsCoroutine(Action<string> action, float seconds, string key) {
        yield return new WaitForSeconds(seconds);
        action.Invoke(key);
    }

    /// <summary>
    /// Calls the specified action after the specified number of fixed frames
    /// </summary>
    /// <param name="action">The method that will be called after the delay</param>
    /// <param name="frameCount">The number of fixed frames to wait before calling the action</param>
    /// <returns>Key of the created coroutine</returns>>
    public string CallAfterFixedFrames(Action<string> action, int frameCount = 1) {
        var key = GenerateKey();
        var coroutine = StartCoroutine(CallAfterFixedFramesCoroutine(action, frameCount, key));
        _keys.Add(key, coroutine);
        return key;
    }

    /// <summary>
    /// Coroutine that handles the delay of the specified action by the specified number of fixed frames
    /// </summary>
    /// <param name="action">The method that will be called after the delay</param>
    /// <param name="frameCount">The number of fixed frames to wait before calling the action</param>
    /// <param name="key">The identifier of the coroutine class</param>
    /// <returns>IEnumerator for coroutine management</returns>
    private static IEnumerator CallAfterFixedFramesCoroutine(Action<string> action, int frameCount, string key) {
        while(frameCount > 0) {
            yield return new WaitForFixedUpdate();
            frameCount--;
        }
        action.Invoke(key);
    }

    /// <summary>
    /// Calls the specified action at the end of the current frame
    /// </summary>
    /// <param name="action">The method that will be called after the delay</param>
    /// <returns>Key of the created coroutine</returns>>
    public string CallAtEndOfFrame(Action<string> action) {
        var key = GenerateKey();
        Coroutine coroutine = StartCoroutine(CallAtEndOfFrameCoroutine(action, key));
        _keys.Add(key, coroutine);
        return key;
    }

    /// <summary>
    /// Coroutine that handles the delay of the specified action until the end of the current frame
    /// </summary>
    /// <param name="action">The method that will be called after the delay</param>
    /// <param name="key">The identifier of the coroutine class</param>
    /// <returns>IEnumerator for coroutine management</returns>
    private static IEnumerator CallAtEndOfFrameCoroutine(Action<string> action, string key) {
        yield return new WaitForEndOfFrame();
        action.Invoke(key);
    }

    public void StopDelayedAction(string key) {
        if(!_keys.TryGetValue(key, out var coroutine)) {
            return;
        }
        StopCoroutine(coroutine);
        _keys.Remove(key);
    }

    /// <summary>
    /// Generates a unique ID
    /// </summary>
    /// <returns>Created ID</returns>
    private string GenerateKey() {
        return Guid.NewGuid().ToString("N");
    }
}
