using UnityEngine;

public class GoalComponent : MonoBehaviour
{
    // Store the spawner and the spaceship to broadcast when the goal is reached
    GoalSpawner _spawner;
    SpaceshipControllerBase _spaceship;

    [SerializeField] bool _IsGloballyFound = false;

    // TODO : actually reflect the remaining time 
    float _FalloffDistance = 0.0f;
    float _FalloffSpeed = 1.0f;

    // Positive or Negative
    float _PN = 1.0f;

    public bool IsAutoFound
    {
        get { return _IsGloballyFound; }
    }

    public void Init(GoalSpawner spawner, SpaceshipControllerBase spaceship)
    {
        _spawner = spawner;
        _spaceship = spaceship;
    }

    private void Update()
    {
        _FalloffDistance += Time.deltaTime * _PN * _FalloffSpeed;
        if (_FalloffDistance > 1.0f)
        {
            _PN = -1.0f;
        }
        if (_FalloffDistance < 0.0f)
        {
            _PN = 1.0f;
        }

        _FalloffDistance = Mathf.Clamp(_FalloffDistance, 0.0f, 1.0f);

        GetComponent<Renderer>().material.SetFloat("_Falloff", _FalloffDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpaceshipControllerBase>() == _spaceship)
        {
            _spawner.OnGoalReached();
            _spaceship.OnGoalReached(this);
        }
    }
}
