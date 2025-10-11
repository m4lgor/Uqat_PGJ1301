using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// -----------------------
// This file is not for you to Edit. You should inherit from it, and add your component to the Spaceship in the scene.
// The restriction is that you cannot influence the position or apply any forces to this object in your inherited class.
// It must be done through the RocketEngine.Thrust(float Power)
// -----------------------
// _TargetPosition is the position that you should aim to reach with your Spaceship.
// You can Access the _TargetPosition through the public property TargetPosition.
// -----------------------

public class SpaceshipControllerBase : MonoBehaviour
{
    protected List<RocketEngine> _RocketEngines = new List<RocketEngine>();
    protected List<ISensor> _Sensors = new List<ISensor>();
    protected Vector3 _TargetPosition = Vector3.zero; // Aim to reach this position
    protected Rigidbody _Rigidbody; // Cache Rigidbody component

    private void Awake()
    {
        // Cache the Rigidbody component
        _Rigidbody = GetComponent<Rigidbody>();

        // Cache all RocketEngine components in children
        _RocketEngines.AddRange(GetComponentsInChildren<RocketEngine>());

        // Cache all ISensor components in children
        _Sensors.AddRange(GetComponentsInChildren<ISensor>());

        // Initialize target position to the spaceship's current position
        _TargetPosition = transform.position; 
    }

    protected void Update()
    {
        UpdateTarget();
    }

    /// <summary>
    /// Updates the target position by finding the first GoalComponent in the scene.
    /// </summary>
    void UpdateTarget()
    {
        var goalComponent = FindFirstObjectByType<GoalComponent>();
        if (!goalComponent)
        {
            return;
        }

        if(goalComponent.IsAutoFound)
        {
            _TargetPosition = goalComponent.transform.position;
        }
    }

    /// <summary>
    /// Returns the current target position that the spaceship should aim to reach.
    /// Setting this property will update the target position.
    /// </summary>  
    public Vector3 TargetPosition
    {
        get { return _TargetPosition; }
        set { _TargetPosition = value; }
    }

    /// <summary>
    /// Utility function to disable all engines.
    /// </summary>
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

    /// <summary>
    /// Event called when a goal is reached by this spaceship.
    /// </summary>
    /// <param name="goalComponent"></param>
    public virtual void OnGoalReached(GoalComponent goalComponent)
    {
        // You can override this method in your inherited class to handle goal reached events.
        // This base implementation does nothing.
    }

    protected void OnDrawGizmos()
    {
        if (!GlobalSettings.Instance)
            return;

        Gizmos.color = Color.white;

        Gizmos.DrawRay(_TargetPosition - GlobalSettings.Instance.WorldRight, GlobalSettings.Instance.WorldRight * 2.0f);
        Gizmos.DrawRay(_TargetPosition - GlobalSettings.Instance.WorldUp, GlobalSettings.Instance.WorldUp * 2.0f);
    }
}
