using UnityEngine;

public class ShipController : MonoBehaviour
{
    // Ship object
    private GameObject shipObject;



    // Rotation speed
    [SerializeField]
    private float rotationSpeed = 50f;


    void Start()
    {
        shipObject = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        // Rotation inputs
        float pitchInput = Input.GetAxis("Pitch");
        float rollInput = Input.GetAxis("Roll");

        // Rotate the ship based on user input
        RotateShip(pitchInput, rollInput);
    }


    // Quaternions to rotate the ship
    private void RotateShip(float pitchInput, float rollInput)
    {
        // Pitch and Roll rotatins
        Quaternion pitchRotation = Quaternion.AngleAxis(pitchInput * rotationSpeed * Time.deltaTime, Vector3.right);
        Quaternion rollRotation = Quaternion.AngleAxis(rollInput * rotationSpeed * Time.deltaTime, Vector3.back);

        // Multiplication for final rotation
        shipObject.transform.rotation *= pitchRotation * rollRotation;
    }
}
