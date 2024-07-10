using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Programmable Enum for determining the type and functionality of targeting
/// </summary>
public class TargetingStrategy {
    /// <summary>
    /// Name of the strategy
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Corresponding enum value of the TargetingStrategy
    /// </summary>
    public TargetingStrategyType Type { get; }

    ///<summary>
    /// <para>Gets the next target valid target</para>
    /// <para>Params:</para>
    /// <para>- <see cref="UnitBase"/>: Origin</para>
    /// <para>- <see cref="IEnumerable{T}"/>: List of potential Targets</para>
    /// <para>Returns: <see cref="UnitBase"/> of target</para>
    /// </summary>
    private Func<UnitBase, IEnumerable<UnitBase>, UnitBase> OnGetNextTarget { get; }


    /// <summary>
    /// Gets the next target valid target
    /// </summary>
    /// <param name="origin">Unit that wants to attack</param>
    /// <param name="targetList">List of valid targets</param>
    /// <returns>The chosen target</returns>
    public UnitBase GetNextTarget(UnitBase origin, IEnumerable<UnitBase> targetList) {
        return OnGetNextTarget?.Invoke(origin, targetList);
    }

    private TargetingStrategy(string name, TargetingStrategyType type, Func<UnitBase, IEnumerable<UnitBase>, UnitBase> getNextTarget) {
        Name = name;
        Type = type;
        OnGetNextTarget = getNextTarget;
    }

    /// <summary>
    /// Targets self
    /// </summary>
    public readonly static TargetingStrategy Self = new("Self", TargetingStrategyType.Self, SelfTargeting);

    /// <summary>
    /// Targets the closest target within the towers range
    /// </summary>
    public readonly static TargetingStrategy Closest = new("Closest", TargetingStrategyType.Closest, ClosestTargeting);

    /// <summary>
    /// Targets the furthest target within the towers range
    /// </summary>
    public readonly static TargetingStrategy Furthest = new("Furthest", TargetingStrategyType.Furthest, FurthestTargeting);

    /// <summary>
    /// Targets the current target if alive otherwise targets the furthest target on the track within range
    /// </summary>
    public readonly static TargetingStrategy UntilTargetDeath =
        new("UntilTargetDeath", TargetingStrategyType.UntilTargetDeath, UntilTargetDeathTargeting);

    /// <summary>
    /// Maps serialized TargetingStrategyType to corresponding TargetingStrategy
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static TargetingStrategy FromType(TargetingStrategyType type) {
        return type switch {
            TargetingStrategyType.Self => Self,
            TargetingStrategyType.Closest => Closest,
            TargetingStrategyType.Furthest => Furthest,
            TargetingStrategyType.UntilTargetDeath => UntilTargetDeath,
            _ => throw new NotSupportedException(),
        };
    }

    /// <summary>
    /// Return self
    /// </summary>
    private static UnitBase SelfTargeting(UnitBase origin, IEnumerable<UnitBase> _) {
        return origin;
    }
    /// <summary>
    /// Return the closest target within the towers range
    /// </summary>
    private static UnitBase ClosestTargeting(UnitBase origin, IEnumerable<UnitBase> targets) {
        return DistanceBasedTargeting(origin, targets, true);
    }
    /// <summary>
    /// Return the furthest target within the towers range
    /// </summary>
    private static UnitBase FurthestTargeting(UnitBase origin, IEnumerable<UnitBase> targets) {
        return DistanceBasedTargeting(origin, targets, false);
    }
    /// <summary>
    /// Return the current target if alive otherwise and within range return the furthest target on the track within range
    /// </summary>
    private static UnitBase UntilTargetDeathTargeting(UnitBase origin, IEnumerable<UnitBase> targets) {
        if(origin.AttackTarget != null &&
           Vector3.Distance(origin.transform.position, origin.AttackTarget.transform.position) < origin.Range) {
            return origin.AttackTarget;
        } else {
            return FirstTargeting(origin, targets);
        }
    }
    /// <summary>
    /// Return the furthest target on the track within range
    /// </summary>
    private static UnitBase FirstTargeting(UnitBase origin, IEnumerable<UnitBase> targets) {
        return targets.Where(t => Vector3.Distance(t.transform.position, origin.transform.position) < origin.Range)
            .OrderByDescending(t => t.GetComponent<SplineAnimate>().ElapsedTime)
            .FirstOrDefault();
    }

    /// <summary>
    /// Calculates the distance between origin and all targets
    /// </summary>
    /// <param name="origin">Origin of calculations</param>
    /// <param name="targets">List of all targets</param>
    /// <param name="min">searches closest(true) or furthest(false) target</param>
    /// <returns>Closest or furthest target form the origin within range of said origin</returns>
    private static UnitBase DistanceBasedTargeting(UnitBase origin, IEnumerable<UnitBase> targets, bool min) {
        var currentDistance = min ? float.MaxValue : float.MinValue;
        UnitBase returnTarget = null;
        foreach(var target in targets) {
            var distance = Vector3.Distance(target.transform.position, origin.transform.position);
            if(distance > origin.Range || min ? (distance > currentDistance) : (distance < currentDistance)) {
                continue;
            }
            currentDistance = distance;
            returnTarget = target;
        }
        return returnTarget;
    }
}
