# Live-Project-Unity

## Introduction:


For the last two weeks with the Tech Academy, I worked on a Live Project in Unity and C# which simulated a real-world work environment in which I worked with my peers and staff on a 3D miniature golf game. I worked on a variety of stories including making my own level for the game and working on multiple systems which will be used on all levels. Below are some code snippets of the stories I worked on, full code files are also available in this repository.

## Stories:

### Add New Hole:
Five stories in which I created my own level for the game including the basic level design, scenery, terrain, lighting, particle effects, and animations. My level has a miniture golf course with a few obstacles including multiple height changes like a hill and a depression, and a few objects on the course like a working windmill and a couple of blocks. There are also a few scenery peices like a house, foutain, lamp post, and a campfire. The house has a chimeny billowing smoke and a the campfire is lit with a blazing fire complete with smoke, embers, and animated light which slightly flickers and moves. All of these effects were created with the base unity tools for animation and particle effects.

### Add Interactive Camera System and Automatic Camera Placement:
Two stories dealing with creating an interactive camera system that is locked on the Y-axis but allows free movement (on the X-axis and Z-axis) and looking when pressing the middle mouse button. I also added the ability for the camera to automatically place itself at a specified offset and height from a target and to automatically start looking at said target, which in this case is usually the ball. Additionally, the camera automatically works "out of the box" when added to a level from the prefab without any parameter changes for ease of use for future developers.

~~~
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
~~~

### Reveal Player Ball When Hidden:
This story focused on making the player ball remain visable when hidden by other objects as to not lose the ball when playing. The way I implemented this change was by using a shader to allow the ball to be visable whenever it was obstructed by another object with a highlight. I did this for both the ball and the aiming ridicule (which when I did this story was another smaller yellow sphere used for aiming).

### Create Fireworks Particle Effect and Trigger
This story revolved around creating a particle effect for fireworks that shoot out of the hole when the player completes the course. Like with the other particles effects I worked on I put a lot of time into making sure that these particles looked good with the base unity tools for particle systems. Additionally I made sure to try to make these fireworks as scalable as possible since these are to be use on all future maps just in case someone needed to scale them upwards or downwards. The fireworks include the firework itself, the smoketrail, and the final explosion using sub emitters. I also had to implement a coroutine for the trigger to delay the next scene loading till after the fireworks shoot off and explode.

~~~
    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "Hole")
        {
            ballInHole = true;
            timeInHole += Time.deltaTime;
        }
        if (timeInHole > 1.5f)
        {
            StartCoroutine(WaitForSceneLoad());
        }
    }
    
    private IEnumerator WaitForSceneLoad()
    {
        if (fireworks != null)
        {
            fireworks.SetActive(true);
            yield return new WaitForSeconds(5);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
~~~

## Images:
![Hole Image](https://user-images.githubusercontent.com/71560004/134138998-3214f5b8-65f9-4b61-ab81-c2861b72963b.png)
Here is an image of the whole level

![Firework Particles](https://user-images.githubusercontent.com/71560004/134144710-d1b69acc-8a5a-4eaf-90b0-0346c0254f4f.gif)
Here is a look at the firework particle effects, all created with the default particle systems.
