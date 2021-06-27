using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWASD : MonoBehaviour
{

    private bool mouseFocus = true;

    public float speed;
    public float mouseSensitivity;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // toggle mouse focus.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mouseFocus)
            {
                mouseFocus = false;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                mouseFocus = true;
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        Vector3 eulerAngles = transform.rotation.eulerAngles;
        // X is pitch, which is mouse Y
        // Y is yaw, which is mouse X.
        if (mouseFocus)
        {
            eulerAngles.x += Math.Max(Math.Min(-Input.GetAxis("Mouse Y") * mouseSensitivity, 90), -90);
            eulerAngles.y += Input.GetAxis("Mouse X") * mouseSensitivity;
            transform.rotation = Quaternion.Euler(eulerAngles);
        }

        Vector3 position = transform.position;

        if (Input.GetKey(KeyCode.W))
        {
            position += speed * transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            position -= speed * transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position -= speed * transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position += speed * transform.right;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            position += speed * Vector3.up;
        }
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            position -= speed * Vector3.up;
        }
        
        transform.SetPositionAndRotation(position, Quaternion.Euler(eulerAngles));
        
    }
}
