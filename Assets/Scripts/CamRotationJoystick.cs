using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotationJoystick : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystickRotation;

    public float moveSpeed;
    public Transform mainCamera;

    public static Vector3 currentRotation;

    float horizontalInput, verticalInput;
    // Update is called once per frame
    void Update()
    {
        RotationLogic();
    }


    private void Start()
    {
        currentRotation = new Vector3(0,0,0);
    }
    private void RotationLogic()
    {
        horizontalInput = joystickRotation.Horizontal;
        verticalInput = joystickRotation.Vertical;

        Vector3 rotationInput = new Vector3(-verticalInput, horizontalInput, 0);

        Vector3 tempRotation = currentRotation + (rotationInput * moveSpeed * Time.deltaTime);

        tempRotation.x = Mathf.Clamp(tempRotation.x, -80f, 80f);

        mainCamera.localEulerAngles = tempRotation;

        currentRotation = tempRotation;
    }
}
