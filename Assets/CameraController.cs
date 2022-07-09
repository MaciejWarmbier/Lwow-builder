using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 20f;
    [SerializeField] private float cameraMovementMargin = 20f;
    [SerializeField] private float minPositionX = 0;
    [SerializeField] private float maxPositionX = 80;
    [SerializeField] private float minPositionZ = -40;
    [SerializeField] private float maxPositionZ = 16;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height  - cameraMovementMargin && pos.z < maxPositionZ)
        {
            pos.z += cameraSpeed * Mathf.Min((1 - (Screen.height - Input.mousePosition.y) / cameraMovementMargin), 1.5f) * Time.deltaTime;
        }
        else if (Input.mousePosition.y <= cameraMovementMargin && pos.z > minPositionZ)
        {
            pos.z -= cameraSpeed * Mathf.Min((1 - Input.mousePosition.y / cameraMovementMargin), 1.5f) * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - cameraMovementMargin && pos.x < maxPositionX)
        {
            pos.x += cameraSpeed * Mathf.Min((1 - (Screen.width - Input.mousePosition.x) / cameraMovementMargin), 1.5f) * Time.deltaTime;
        }
        else if (Input.mousePosition.x <= cameraMovementMargin && pos.x > minPositionX)
        {
            pos.x -= cameraSpeed * Mathf.Min((1 - Input.mousePosition.x / cameraMovementMargin), 1.5f) * Time.deltaTime;
        }

        transform.position = pos;
    }
}
