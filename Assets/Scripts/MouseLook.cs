// ========== HMOVE ==========
//
//        MouseLook.cs
//
// Use mouse to rotate camera.
//
// ===========================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public float xRotation = 0f;
    public bool invert = false;

    public Camera m_camera;

    public bool flippedcamera = true;



    // Start is called before the first frame update
    void Start()
    {
        m_camera = this.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        if (invert == false)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation += mouseY;
        }

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            invert = !invert;
        }
    }
}
