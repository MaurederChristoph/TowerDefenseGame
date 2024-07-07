using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour {
    /// <summary>
    /// The amount off projectiles that are shot before reloading
    /// </summary>
    private float _burstSize = 1f;
    /// <summary>
    /// Multiplies the Attack speed between each shot within a burst volley
    /// </summary>
    private float _inBetweenBurstShotsMultiplier = 1f;
    /// <summary>
    /// Multiplies the Attack speed after the burst is over
    /// </summary>    
    private float _burstReloadMultiplier = 1f;
}
