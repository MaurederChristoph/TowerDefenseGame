using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableStats : ScriptableObject {
    [Header("MOVEMENT")]
    public float MaxSpeed = 14;
    public float Acceleration = 120;
    public float GroundDeceleration = 60;
    public float AirDeceleration = 30;
    [Range(0f, 0.5f)]
    public float GrounderDistance = 0.05f;
    public float GroundStickingMultiplier;

    [Header("EXTRA")]
    public int ExternalVelocityDecay = 100;

    [Header("JUMP")]
    public float JumpPower = 30 ;
    public float MaxFallSpeed = 20;
    public float FallAcceleration = 100;
    public int CoyoteFrames = 7;
    public int JumpBufferFrames = 7;
    public float JumpEndEarlyGravityModifier = 1;
}
