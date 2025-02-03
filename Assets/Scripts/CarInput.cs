using UnityEngine;
using UnityEngine.InputSystem;

public class CarInput : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _movementAction;
    public Vector2 MovementVector { get; private set; }
    private Car car;

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        car = new Car();
        car.CarControls.Enable();
        car.CarControls.Move.performed += MoveCar;
    }

    void OnDisable()
    {
        car.CarControls.Disable();
        car.CarControls.Move.performed -= MoveCar;
    }

    private void MoveCar(InputAction.CallbackContext obj)
    {
        Debug.Log("Working Movement :"+obj.ReadValue<Vector2>());
        MovementVector = obj.ReadValue<Vector2>();
    }
}
