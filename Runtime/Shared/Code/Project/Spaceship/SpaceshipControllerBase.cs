using System.Collections.Generic;
using UnityEngine;

// -----------------------
// This file is not for you to Edit. You should inherit from it, and add your component to the Spaceship in the scene.
// The restriction is that you cannot influence the position or apply any forces to this object in your inherited class.
// It must be done through the RocketEngine.Thrust(float forceMagnitude)
// -----------------------

public class SpaceshipControllerBase : MonoBehaviour
{
    protected List<RocketEngine> _RocketEngines = new List<RocketEngine>();
    protected Vector3 _TargetPosition = Vector3.zero; // Placeholder for target position, replace with actual target logic
    protected Rigidbody _Rigidbody; // Cache Rigidbody component

    private void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody>();
        _RocketEngines.AddRange(GetComponentsInChildren<RocketEngine>());
        _TargetPosition = transform.position; // Initialize target position to the spaceship's current position
    }

    public Vector3 TargetPosition
    {
        get { return _TargetPosition; }
        set
        {
            _TargetPosition = value;
        }
    }

    protected void DisableAllEngines()
    {
        foreach (var rocketEngine in _RocketEngines)
        {
            if (rocketEngine)
            {
                rocketEngine.EnablePropulsion(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (!GlobalSettings.Instance)
            return;

        Gizmos.color = Color.white;

        Gizmos.DrawRay(_TargetPosition - GlobalSettings.WorldRight, GlobalSettings.WorldRight * 2.0f);
        Gizmos.DrawRay(_TargetPosition - GlobalSettings.WorldUp, GlobalSettings.WorldUp * 2.0f);
    }
}
