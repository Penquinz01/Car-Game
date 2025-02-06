using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private WheelControl[] wheels;
    [SerializeField] private float motorTorque = 2000;
    [SerializeField] private float brakeTorque = 2000;
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private float maxSteeringAngle = 30;
    [SerializeField] private float maxSpeedSteeringAngle = 10;
    [SerializeField] private float centreOfGravityOffset = -1f;

    private Rigidbody _rb;
    private CarInput _carInput;
    private float _vInput;
    private float _hInput;
    private bool _isBraking = false;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _carInput = GetComponent<CarInput>();
        _rb.centerOfMass+= Vector3.up * centreOfGravityOffset;
        _carInput.startedBraking += SetBraking;
        _carInput.stoppedBraking += SetBraking;
    }

    private void OnDestroy()
    {
        _carInput.startedBraking -= SetBraking;
        _carInput.stoppedBraking -= SetBraking;
    }

    private void SetBraking()
    {
        
        _isBraking = !_isBraking;
    }

    // Update is called once per frame
    private void Update()
    {
        _vInput = _carInput.MovementVector.y;
        _hInput = _carInput.MovementVector.x;
        
        var forwardSpeed = Vector3.Dot(transform.forward, _rb.linearVelocity);
        
        var speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        
        var currentTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        
        var currentSteeringRange = Mathf.Lerp(maxSteeringAngle,maxSpeedSteeringAngle,speedFactor);
        
        var isAccelerating = Mathf.Approximately(Mathf.Sign(_vInput), Mathf.Sign(forwardSpeed));

        foreach (var wheel in wheels)
        {
            if (wheel.isSteerable)
            {
                wheel.WheelCollider.steerAngle = currentSteeringRange*_hInput;
            }

            if (_isBraking)
            {
                wheel.WheelCollider.motorTorque = 0;
                wheel.WheelCollider.brakeTorque = brakeTorque;
            }
            else if (isAccelerating)
            {
                if (wheel.isMotorized)
                {
                    wheel.WheelCollider.motorTorque = currentTorque * _vInput;
                }

                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                wheel.WheelCollider.brakeTorque = Mathf.Abs(_vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}
