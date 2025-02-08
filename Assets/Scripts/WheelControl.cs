using UnityEngine;

public class WheelControl : MonoBehaviour
{
    public bool isSteerable;
    public bool isMotorized;
    public bool isDriftable;
    public WheelCollider WheelCollider { get;private set; }

    private Vector3 _position;
    private Quaternion _rotation;
    void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        WheelCollider.GetWorldPose(out _position, out _rotation);
        WheelCollider.transform.position = _position;
        WheelCollider.transform.rotation = _rotation;
    }
}
