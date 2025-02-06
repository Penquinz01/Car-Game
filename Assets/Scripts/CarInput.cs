using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class CarInput : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _movementAction;
    public Vector2 MovementVector { get; private set; }
    private Car _car;
    public event Action startedBraking;
    public event Action stoppedBraking;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _car = new Car();
        _car.CarControls.Enable();
        _car.CarControls.Move.performed += MoveCar;
        _car.CarControls.Move.canceled += StopMovement;
        _car.CarControls.Brake.started += BrakeInitialize;
        _car.CarControls.Brake.canceled += BrakeCanceled;
    }

    private void BrakeCanceled(InputAction.CallbackContext obj)
    {
        startedBraking?.Invoke();
    }

    private void BrakeInitialize(InputAction.CallbackContext obj)
    {
        stoppedBraking?.Invoke();
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
        _car.CarControls.Brake.started -= BrakeInitialize;
        _car.CarControls.Brake.canceled -= BrakeCanceled;
    }

    private void MoveCar(InputAction.CallbackContext obj)
    {
        MovementVector = obj.ReadValue<Vector2>();
    }
}
