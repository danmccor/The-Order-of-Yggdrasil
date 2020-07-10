using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float turnSpeed = 2.0f;
    public float moveSpeed = 10.0f;

    public float yaw = 0.0f , pitch = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position = transform.position + Camera.main.transform.forward *moveSpeed* Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position = transform.position - Camera.main.transform.forward * moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position = transform.position - Camera.main.transform.right * moveSpeed* Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position = transform.position + Camera.main.transform.right * moveSpeed * Time.deltaTime;
            }


            yaw += turnSpeed * Input.GetAxis("Mouse X");
            pitch -= turnSpeed * Input.GetAxis("Mouse Y");


           transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
            
        }
    }
}
