using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovementJoystick : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystickMovement;

    public float moveSpeed = 5f;
    public Vector2 moveLimits = new Vector2(50f, 50f); // Limieten: (x = links/rechts, z = voren/achter)

    private Vector3 startPosition;

    void Start()
    {
        // Sla de startpositie van de camera op
        startPosition = transform.position;
    }

    void Update()
    {
        MovementLogic();
    }

    private void MovementLogic()
    {
        float horizontalInput = joystickMovement.Horizontal;
        float verticalInput = joystickMovement.Vertical;

        // Bereken de richting van beweging op basis van waar de camera naar kijkt
        Vector3 forward = transform.forward;  // Voorwaartse richting van de camera
        Vector3 right = transform.right;      // Zijwaartse richting van de camera

        // Verwijder de y-component om te voorkomen dat de camera omhoog/omlaag beweegt
        forward.y = 0;
        right.y = 0;

        // Normaliseer de richtingen zodat de snelheid consistent blijft
        forward.Normalize();
        right.Normalize();

        // Bereken de nieuwe beweging op basis van joystick-invoer en richtingen
        Vector3 movementInput = (forward * verticalInput + right * horizontalInput) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movementInput;

        // Beperk de beweging binnen de limieten
        newPosition.x = Mathf.Clamp(newPosition.x, startPosition.x - moveLimits.x, startPosition.x + moveLimits.x);
        newPosition.z = Mathf.Clamp(newPosition.z, startPosition.z - moveLimits.y, startPosition.z + moveLimits.y);

        // Pas de nieuwe positie toe
        transform.position = newPosition;
    }
}
