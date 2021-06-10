using UnityEngine;
using UnityEngine.UI;

public class playerBall : MonoBehaviour
{
    //PUBLIC FIELDS
    public float puttDirectionChangeSpeed;
    public float puttMaxPower;
    public Transform directionPointerBall;
    public Slider powerSlider;
    public Rigidbody ball;

    //PRIVATE FIELDS
    private LineRenderer puttDirectionLine;
    private float puttDirection;
    private bool didPutt = false;
    private int puttCount;
    private float powerUpTime;
    private float puttPower;
    private Vector3 startLocation;

    void Start()
    {
        startLocation = ball.transform.position;
    }

    // Awake is called before Update
    void Awake()
    {
        //Default is 7. Higher values result in better rolling, but chances of physics oddities goes up
        ball.maxAngularVelocity = 10000f;
        ball.sleepThreshold = 0.01f;
        puttDirectionLine = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //If Mouse2 is pressed it means camera controls are active
        if (!Input.GetKey(KeyCode.Mouse2))
        {
            puttDirection += Input.GetAxis("Horizontal") * Time.deltaTime * puttDirectionChangeSpeed;

            if (Input.GetKeyUp(KeyCode.Space) && ball.IsSleeping())
            {
                // Play Swing audio
                AudioManager.instance.Play("Swing");
                didPutt = true;
            }
            if (Input.GetKey(KeyCode.Space) && ball.IsSleeping())
            {
                PowerSliderControl();
            }
            BallMoving();
            RotatePuttDirection();
        }
    }

    // FixedUpdate is called once per frame
    // Better for physics
    void FixedUpdate()
    {
        if (didPutt)
        {
            Putt();
        }
    }

    private void Putt()
    {
        //puttMaxPower defined in inspector
        ball.AddForce(Quaternion.Euler(0, puttDirection, 0) * Vector3.forward * puttMaxPower * puttPower, ForceMode.Impulse);
        didPutt = false;
        puttCount++;
        resetPuttUI();
    }

    private void resetPuttUI()
    {
        powerUpTime = 0f;
        powerSlider.value = 0f;
    }

    private void RotatePuttDirection()
    {
        puttDirectionLine.SetPosition(0, transform.position);
        puttDirectionLine.SetPosition(1, transform.position + Quaternion.Euler(0, puttDirection, 0) * Vector3.forward * 0.15f);
        directionPointerBall.transform.position = puttDirectionLine.GetPosition(1);
    }

    private void PowerSliderControl()
    {
        powerUpTime += Time.deltaTime;
        puttPower = Mathf.PingPong(powerUpTime, 1f);
        powerSlider.value = puttPower;
    }

    //Checks the movement state of the ball
    //Disables UI when moving
    //Resets ball if it goes off course
    private void BallMoving()
    {
        // the ball is awake
        // disable direction indicator
        if (!ball.IsSleeping())
        {
            directionPointerBall.GetComponent<MeshRenderer>().enabled = false;
        }
        // the ball is sleeping & outside the hole
        // enable direction indicator
        if (ball.IsSleeping() && !playerBallTriggers.ballInHole)
        {
            directionPointerBall.GetComponent<MeshRenderer>().enabled = true;
        }
        // the ball is off the course
        if (ball.transform.position.y < -5f)
        {
            ball.velocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
            ball.transform.position = startLocation;
        }
    }
}
