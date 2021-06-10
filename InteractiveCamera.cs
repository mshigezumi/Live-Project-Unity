using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveCamera : MonoBehaviour
{
    public Camera camera;
    public float mouseSensitivity = 100f;
    float yRotation;
    float xRotation;

    public CharacterController characterController;
    public float MovementSpeed = 10f;

    public Transform targetObject;
    public float height = 3f; //change to an offset?
    public float xOffset = -2f;
    public float zOffset = -2f;
    Vector3 cameraOffset;
    string playerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        //Automatically sets the targetObject to the playerBall if there is no targetObject manually set
        if (targetObject == null)
        {
            targetObject = GameObject.FindGameObjectWithTag(playerTag).transform;
        }

        //Sets the camera start postion releative to the target object and at a specified height
        cameraOffset.Set(xOffset, height, zOffset);
        Vector3 startPosition = targetObject.transform.position + cameraOffset;
        startPosition.y = height;
        transform.position = startPosition;

        //Makes the camera look at the target object
        transform.LookAt(targetObject);

        //This ensures the camera starts with the correct roations for the x and y axis and sets the z axis to 0, makes it so the camera doesn't snap to 0,0,0 rotation when it is activated
        yRotation = camera.transform.localEulerAngles.y;
        xRotation = camera.transform.localEulerAngles.x;
        camera.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            //Camera rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            yRotation += mouseX;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            camera.transform.eulerAngles = new Vector3(xRotation, yRotation, 0.0f);

            //Camera movement
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * vertical;
            move.y = 0f;
            characterController.Move(move * MovementSpeed * Time.deltaTime);
        }
    }
}
