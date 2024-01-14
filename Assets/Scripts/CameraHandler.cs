using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    float camSens = 0.25f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun = 1.0f;

    void Update()
    {
        // Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();

        // Handle shift key
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // ... (остальной код)
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            p = p * mainSpeed;
        }

        p = p * Time.deltaTime;

        // Applying keyboard movement
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.Space))
        {
            // Handle movement along X and Z axes
            // ...
        }
        else
        {
            // Handle general movement
            transform.Translate(p);
        }

        // Mouse movement for camera panning in 4 directions
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Limiting mouse movement to 4 cardinal directions
        if (Mathf.Abs(mouseX) > Mathf.Abs(mouseY))
        {
            transform.Translate(Vector3.right * mouseX * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.forward * mouseY * Time.deltaTime);
        }
    }


    // Остальная часть кода остается без изменений


    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
}
