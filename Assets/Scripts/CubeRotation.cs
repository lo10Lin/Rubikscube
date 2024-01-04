using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotation : MonoBehaviour
{
    public float rotationSensitivity = 5;
    [SerializeField]
    float zoomMin, zoomMax, scrollSensitivity;
    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Confined;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Cursor.lockState = CursorLockMode.None;
           
        }

        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSensitivity;

            //transform.rotation *= new Quaternion(Mathf.Cos(mouseX/2), 0, Mathf.Sin(mouseX/2), 0).normalized;
            //transform.rotation *= new Quaternion(Mathf.Cos(mouseY/2), -Mathf.Sin(mouseY/2), 0, 0).normalized;

            // CUBE ROTATION

            //transform.rotation = Quaternion.AngleAxis(mouseX, Vector3.down) * transform.rotation; 
            //transform.rotation = Quaternion.AngleAxis(mouseY, Vector3.right) * transform.rotation;

            // CAMERA ROTATION

            transform.rotation *= Quaternion.AngleAxis(mouseX, Vector3.up); 
            transform.rotation *= Quaternion.AngleAxis(mouseY, Vector3.left);

        }

        Camera.main.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(Camera.main.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel") * scrollSensitivity, zoomMin, zoomMax));

    }
}
