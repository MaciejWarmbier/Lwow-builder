using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float cameraSpeed = 20f;
    [SerializeField] private float minPositionX = 0;
    [SerializeField] private float maxPositionX = 80;
    [SerializeField] private float minPositionZ = -40;
    [SerializeField] private float maxPositionZ = 16;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height  - 10f && pos.z < maxPositionZ)
        {
            pos.z += cameraSpeed * Mathf.Min((1 - (Screen.height - Input.mousePosition.y) / 10f), 1.5f) * Time.deltaTime;
        }
        else if (Input.mousePosition.y <= 10f && pos.z > minPositionZ)
        {
            pos.z -= cameraSpeed * Mathf.Min((1 - Input.mousePosition.y / 10f), 1.5f) * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - 10f && pos.x < maxPositionX)
        {
            pos.x += cameraSpeed * Mathf.Min((1 - (Screen.width - Input.mousePosition.x) / 10f), 1.5f) * Time.deltaTime;
        }
        else if (Input.mousePosition.x <= 10f && pos.x > minPositionX)
        {
            pos.x -= cameraSpeed * Mathf.Min((1 - Input.mousePosition.x / 10f), 1.5f) * Time.deltaTime;
        }

        transform.position = pos;
    }
}
