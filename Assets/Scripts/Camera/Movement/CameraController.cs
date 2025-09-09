using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 moveLimits = new Vector2(50f, 50f); // Limieten: (x = links/rechts, z = voren/achter)
    public float mouseSensitivity = 100f;
    public float maxVerticalAngle = 80f; // Limiteert hoe ver je naar boven/beneden kunt kijken

    private Vector3 startPosition;
    private float verticalRotation = 0f; // Houdt de verticale rotatie bij

    void Start()
    {
        // Sla de startpositie van de camera op
        startPosition = transform.position;

        // Verberg en vergrendel de cursor
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Lees de invoer van de toetsen
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W)) verticalInput += 1f;
        if (Input.GetKey(KeyCode.S)) verticalInput -= 1f;
        if (Input.GetKey(KeyCode.A)) horizontalInput -= 1f;
        if (Input.GetKey(KeyCode.D)) horizontalInput += 1f;

        // Bereken de voorwaartse en zijwaartse richting (beweging blijft horizontaal)
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        // Verwijder de y-component om verticale beweging te voorkomen
        forward.y = 0;
        right.y = 0;

        // Normaliseer de richtingen
        forward.Normalize();
        right.Normalize();

        // Bereken de nieuwe beweging
        Vector3 movementInput = (forward * verticalInput + right * horizontalInput) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movementInput;

        // Beperk de beweging binnen de limieten
        newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - moveLimits.x, startPosition.x + moveLimits.x);
        newPosition.z = Mathf.Clamp(newPosition.z, startPosition.z - moveLimits.y, startPosition.z + moveLimits.y);

        // Pas de nieuwe positie toe
        transform.position = newPosition;
    }

    private void HandleRotation()
    {
        // Lees muisbewegingen
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotatie rondom de Y-as (horizontaal draaien)
        transform.Rotate(Vector3.up, mouseX);

        // Rotatie rondom de X-as (verticaal kijken)
        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -maxVerticalAngle, maxVerticalAngle);

        // Pas de rotatie toe en fixeer de Z-as op 0
        transform.localEulerAngles = new Vector3(verticalRotation, transform.localEulerAngles.y, 0f);
    }
}
