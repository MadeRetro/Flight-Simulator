using UnityEngine;

public class PlaneController : MonoBehaviour
{
    // Plane object
    private GameObject planeObject;

    // Rotation
    [SerializeField]
    private float rotationSpeed = 50f;

    // Camera
    [SerializeField]
    private TPSCamera tpsCamera = null;


    void Start()
    {
        
        planeObject = transform.GetChild(0).gameObject;

        
        if (tpsCamera != null)
        {
            tpsCamera.GetComponent<Camera>().enabled = true;
            Camera.SetupCurrent(tpsCamera.CameraComponent);
        }
    }

    
    void Update()
    {
        
        float pitchInput = Input.GetAxis("Pitch");
        float rollInput = Input.GetAxis("Roll");

        
        RotatePlane(pitchInput, rollInput);

        
        UpdateCameraRotation();
    }

    
    private void RotatePlane(float pitchInput, float rollInput)
    {
        // Calculate pitch and roll rotations
        Quaternion pitchRotation = Quaternion.AngleAxis(pitchInput * rotationSpeed * Time.deltaTime, Vector3.right);
        Quaternion rollRotation = Quaternion.AngleAxis(rollInput * rotationSpeed * Time.deltaTime, Vector3.back);

        // Apply rotations
        planeObject.transform.rotation *= pitchRotation * rollRotation;
    }

   

    private void UpdateCameraRotation()
    {
        if (tpsCamera != null)
        {
            // Set camera rotation to match the plane's rotation !
            tpsCamera.transform.rotation = planeObject.transform.rotation;
        }
    }





}
