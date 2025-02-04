using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _movementAction;
    public Vector2 MovementVector { get; private set; }
    private Car _car;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _car = new Car();
        _car.CarControls.Enable();
        _car.CarControls.Move.performed += MoveCar;
        _car.CarControls.Move.canceled += StopMovement;
    }

    private void StopMovement(InputAction.CallbackContext obj)
    {
        MovementVector = Vector2.zero;
    }

    private void OnDisable()
    {
        _car.CarControls.Disable();
        _car.CarControls.Move.performed -= MoveCar;
        _car.CarControls.Move.canceled -= StopMovement;
    }

    private void MoveCar(InputAction.CallbackContext obj)
    {
        Debug.Log("Working Movement :"+obj.ReadValue<Vector2>());
        MovementVector = obj.ReadValue<Vector2>();
    }
}
