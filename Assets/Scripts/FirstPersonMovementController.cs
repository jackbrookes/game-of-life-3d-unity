
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows movement and rotation of this GameObject with key and mouse inputs.
/// </summary>
public class FirstPersonMovementController : MonoBehaviour
{
    private const int RIGHT_MOUSE_BUTTON = 1; 

    [Header("Looking")]
    [SerializeField] private float _sensitivity = 5.0f;

    [Header("Movement")]
    [SerializeField] private float _speed = 5.0f;

    private Vector3 _lookEulers = Vector3.zero;

    private void Update()
    {
        // lock cursor while left mouse button held
        if (Input.GetMouseButton(RIGHT_MOUSE_BUTTON))
        {
            Cursor.lockState = CursorLockMode.Locked;

            // only allow looking whilst left mouse button is held
            UpdateLook();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }

        UpdateMovement();
    }


    private void UpdateLook()
    {
        Vector3 mouseDelta = new Vector3(-Input.GetAxisRaw("Mouse Y"), Input.GetAxisRaw("Mouse X"), 0);
        mouseDelta *= _sensitivity;

        // incrementally add to the camera look
        _lookEulers += mouseDelta * Time.deltaTime;

        // prevent looking all the way up or down
        _lookEulers.y = Mathf.Clamp(_lookEulers.y, -90, 90);

        transform.rotation = Quaternion.Euler(_lookEulers);
    }

    private void UpdateMovement()
    {
        float forward = Input.GetAxis("Vertical");
        float strafe = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(strafe, 0, forward) * _speed * Time.deltaTime;
        transform.position += transform.TransformVector(movement);
    }
}