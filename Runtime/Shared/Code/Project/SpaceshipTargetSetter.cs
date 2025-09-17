using UnityEngine;
using UnityEngine.InputSystem;

public class SpaceshipTargetSetter : MonoBehaviour
{
    public SpaceshipControllerBase _SpaceshipControllerBase; // Reference to the SpaceshipController script
    private InputAction _clickAction;

    private void OnEnable()
    {
        _clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        _clickAction.canceled += OnClickReleased;
        _clickAction.Enable();
    }

    private void OnDisable()
    {
        _clickAction.canceled -= OnClickReleased;
        _clickAction.Disable();
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        var mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if(GlobalSettings.Instance.ScrollingPlane.Raycast(ray, out float enter))
        {
            Vector3 targetPosition = ray.GetPoint(enter);
            _SpaceshipControllerBase.TargetPosition = targetPosition; // Assuming SetTargetPosition is a method in SpaceshipController
        }
    }
}
