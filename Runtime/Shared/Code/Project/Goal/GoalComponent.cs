using UnityEngine;

public class GoalComponent : GoalComponentBase
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpaceshipControllerBase>() == _spaceship)
        {
            _spawner.OnGoalReached();
            _spaceship.OnGoalReached(this);
        }
    }
}
