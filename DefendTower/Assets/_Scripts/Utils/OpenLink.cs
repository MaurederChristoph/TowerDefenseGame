using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// opens a browse link
/// </summary>
public class OpenLink : MonoBehaviour {
    /// <summary>
    /// Opens a web link
    /// </summary>
    public void OpenWebLink() {
        System.Diagnostics.Process.Start("https://www.linkedin.com/in/christoph-maureder/");
    }
}
