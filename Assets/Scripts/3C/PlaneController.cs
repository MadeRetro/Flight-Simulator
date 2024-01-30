using UnityEngine;

public class PlaneController : MonoBehaviour
{
    // Plane object
    private GameObject planeObject;

    // Rotation speed
    [SerializeField]
    private float rotationSpeed = 50f;

    // Camera
    [SerializeField]
    private TPSCamera tpsCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        // Assuming your plane is the child of the controller object
        planeObject = transform.GetChild(0).gameObject;

        // Set up the camera
        if (tpsCamera != null)
        {
            tpsCamera.GetComponent<Camera>().enabled = true;
            Camera.SetupCurrent(tpsCamera.CameraComponent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check plane rotation inputs
        float pitchInput = Input.GetAxis("Pitch");
        float rollInput = Input.GetAxis("Roll");

        // Rotate the plane based on user input
        RotatePlane(pitchInput, rollInput);

        // Update camera based on plane rotation
        UpdateCameraRotation();
    }

    // Rotate the plane using quaternions
    private void RotatePlane(float pitchInput, float rollInput)
    {
        // Calculate pitch and roll rotations
        Quaternion pitchRotation = Quaternion.AngleAxis(pitchInput * rotationSpeed * Time.deltaTime, Vector3.right);
        Quaternion rollRotation = Quaternion.AngleAxis(rollInput * rotationSpeed * Time.deltaTime, Vector3.back);

        // Apply rotations
        planeObject.transform.rotation *= pitchRotation * rollRotation;
    }

    // Update camera rotation based on the plane's rotation

    private void UpdateCameraRotation()
    {
        if (tpsCamera != null)
        {
            // Set camera rotation to match the plane's rotation
            tpsCamera.transform.rotation = planeObject.transform.rotation;
        }
    }





}
