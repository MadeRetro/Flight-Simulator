using UnityEngine;

public class PlaneVelocity : MonoBehaviour
{
    // Plane rigidbody
    private Rigidbody planeRigidbody;

    // Forward speed
    [SerializeField]
    private float forwardSpeed = 10f;

    // Roll speed
    [SerializeField]
    private float rollSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody component attached to the plane
        planeRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for spacebar input to accelerate forward
        if (Input.GetKey(KeyCode.Space))
        {
            AccelerateForward();
        }

        // Check for roll input
        float rollInput = Input.GetAxis("Roll");
        RollPlane(rollInput);
    }

    // Accelerate the plane forward
    private void AccelerateForward()
    {
        // Calculate forward force
        Vector3 forwardForce = transform.forward * forwardSpeed;

        // Apply force to the rigidbody
        planeRigidbody.AddForce(forwardForce);
    }

    // Roll the plane based on user input
    private void RollPlane(float rollInput)
    {
        // Calculate roll rotation
        Quaternion rollRotation = Quaternion.AngleAxis(rollInput * rollSpeed * Time.deltaTime, Vector3.back);

        // Apply roll rotation to the plane
        transform.rotation *= rollRotation;
    }
}
