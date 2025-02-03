using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] WheelControl[] wheels;
    [SerializeField]float motorTorque = 2000;
    [SerializeField]float brakeTorque = 2000;
    [SerializeField]float maxSpeed = 30;
    [SerializeField]float maxSteeringAngle = 30;
    [SerializeField]float maxSpeedSteeringAngle = 10;
    [SerializeField] private float centreOfGravityOffset = -1f;

    private Rigidbody _rb;
    private CarInput _carInput;
    private float _vInput;
    private float _hInput;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _carInput = GetComponent<CarInput>();
        _rb.centerOfMass+= Vector3.up * centreOfGravityOffset;
    }

    // Update is called once per frame
    void Update()
    {
        _vInput = _carInput.MovementVector.y;
        _hInput = _carInput.MovementVector.x;
        
        float forwardSpeed = Vector3.Dot(transform.forward, _rb.linearVelocity);
        
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);
        
        float currentTorque = Mathf.Lerp(motorTorque, 0, speedFactor);
        
        float currentSteeringRange = Mathf.Lerp(maxSteeringAngle,maxSpeedSteeringAngle,speedFactor);
        
        bool isAccelerating = Mathf.Sign(_vInput) == Mathf.Sign(forwardSpeed);

        foreach (WheelControl wheel in wheels)
        {
            if (wheel.isSteerable)
            {
                wheel.WheelCollider.steerAngle = currentSteeringRange*_hInput;
            }

            if (isAccelerating)
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
