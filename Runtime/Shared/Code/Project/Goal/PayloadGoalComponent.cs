using UnityEngine;

public class PayloadGoalComponent : GoalComponentBase
{
    protected void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PayloadComponent>())
        {
            _spawner.OnGoalReached();
            _spaceship.OnPayloadGoalReached(this);
        }
    }
}
