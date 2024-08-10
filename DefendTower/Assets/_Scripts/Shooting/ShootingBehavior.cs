using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns and moves projectile form origin to destination based on projectile <see cref="ShootingType"/>
/// </summary>
public class ShootingBehavior : MonoBehaviour {
    [Tooltip("Determents the amplitude of the curve")]
    [SerializeField] private Vector3 _centerOffset;

    [Tooltip("Determents the smoothness of the trajectory")]
    [SerializeField] private int _pathCount;

    /// <summary>
    /// Initiates shooting a projectile from origin to target
    /// </summary>
    /// <param name="origin">Starting position of the projectile</param>
    /// <param name="target">Target position where the projectile should hit</param>
    /// <param name="baseTowerProjectile">Details about the projectile</param>
    public void Shoot(Vector3 origin, UnitBase target, ProjectileInfo baseTowerProjectile) {
        var direction = GetAngle(origin, target.transform.position);
        var shotProjectile = Instantiate(baseTowerProjectile.ProjectilePrefab, origin, direction);
        baseTowerProjectile.Target = target;
        List<Vector3> path = GetPath(origin, target.transform.position, baseTowerProjectile);
        if(path == null) {
            Debug.LogError("The path is empty");
            return;
        }
        StartCoroutine(MoveProjectile(shotProjectile, path, baseTowerProjectile.Speed, baseTowerProjectile));
    }

    /// <summary>
    /// Calculates the rotation an object needs to have to face a point
    /// </summary>
    /// <param name="origin">Starting position</param>
    /// <param name="target">Target position</param>
    /// <returns>Quaternion of the correct rotation between two points</returns>
    private Quaternion GetAngle(Vector3 origin, Vector3 target) {
        var rotation = Quaternion.LookRotation(target - origin, Vector3.up);
        return new Quaternion(0, 0, rotation.z, rotation.w);
    }

    /// <summary>
    /// Determines the trajectory path based on the projectile type
    /// </summary>
    /// <param name="origin">Starting position</param>
    /// <param name="target">Target position</param>
    /// <param name="projectile">Details about the projectile</param>
    /// <returns>List of Vector3 defining the trajectory path</returns>
    private List<Vector3> GetPath(Vector3 origin, Vector3 target, ProjectileInfo projectile) {
        switch(projectile.ShootingType) {
            case ShootingType.Path:
                return new List<Vector3>() { origin, target };
            case ShootingType.Point:
                return GetSlerpPoints(origin, target, _centerOffset, _pathCount);
            default:
                Debug.LogError($"Flying behavior of type {projectile.ShootingType} is not defined");
                return null;
        }
    }

    /// <summary>
    /// Generates path path using Slerp
    /// </summary>
    /// <param name="start">Starting position</param>
    /// <param name="end">End position</param>
    /// <param name="center">Offset center position</param>
    /// <param name="count">Number of interpolation points</param>
    /// <returns>List of interpolated points</returns>
    private List<Vector3> GetSlerpPoints(Vector3 start, Vector3 end, Vector3 center, int count = 10) {
        List<Vector3> result = new();
        var relativeCenter = (start + end) / 2 - center;
        var startRelativeCenter = start - relativeCenter;
        var endRelativeCenter = end - relativeCenter;
        var currentStep = 1f / count;
        for(var i = 0f;i < 1 + currentStep;i += currentStep) {
            result.Add(Vector3.Slerp(startRelativeCenter, endRelativeCenter, i) + relativeCenter);
        }
        return result;
    }

    /// <summary>
    /// Moves the projectile along the calculated path
    /// </summary>
    /// <param name="shotProjectile">Projectile object to be moved</param>
    /// <param name="path">List of points defining the trajectory path</param>
    /// <param name="speed">Movement speed of the projectile</param>
    /// <param name="projectile">Details about the projectile</param>
    /// <returns>Coroutine for moving the projectile</returns>
    private IEnumerator MoveProjectile(GameObject shotProjectile, List<Vector3> path, float speed, ProjectileInfo projectile) {
        var targetIndex = 0;
        while(true) {
            Vector3 startPoint = path[targetIndex];
            Vector3 endPoint = path[targetIndex + 1];
            shotProjectile.transform.rotation = GetAngle(startPoint, endPoint);
            var journeyLength = Vector3.Distance(startPoint, endPoint);
            var journeyProgress = 0f;
            var curveTime = (float)targetIndex / (float)path.Count;
            while(journeyProgress < journeyLength) {
                var curveValue = speed * projectile.SpeedCurve.Evaluate(curveTime);
                journeyProgress += curveValue * Time.deltaTime;
                var fractionOfJourney = journeyProgress / journeyLength;
                shotProjectile.transform.position = Vector3.Lerp(startPoint, endPoint, fractionOfJourney);
                yield return null;
            }
            targetIndex++;
            if(targetIndex >= path.Count - 1) {
                break;
            }
        }
        ReachedDestination(projectile, shotProjectile);
    }

    /// <summary>
    /// Notifies projectile upon reaching the destination
    /// </summary>
    /// <param name="projectile">Details about the projectile</param>
    /// <param name="shotProjectile">Instance of the projectile</param>
    private void ReachedDestination(ProjectileInfo projectile, GameObject shotProjectile) {
        projectile.ReachedDestination(shotProjectile);
    }
}
