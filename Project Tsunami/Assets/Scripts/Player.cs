using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Objects and Components")]
    GameObject playerGameObject;
    Rigidbody rb;
    [Header("Movement and Camera")]
    public float speed;
    public Camera playerView;
    public float playerViewRotation;
    public float viewSpeed;
    public float jumpForce;
    [SerializeField]
    bool isGrounded;
    [SerializeField]
    bool onRamp;
    [Header("Physics and Collisions")]
    public float airStrafe;
    public float rampStrafe;
    public float inclinationMultiplier;
    public float rampExitImpulse;
    [HideInInspector]
    public Vector3 startPosition;
    public float directionShiftModifier;

    private Vector3 movVector;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        playerGameObject = rb.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Pause Game
        if(Input.GetButtonDown("Fire2"))
        {
            GameSys.Pause();
        }
        // Lock mouse
        if(!GameSys.isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }else{
            Cursor.lockState = CursorLockMode.None;
        }
        
        // Fall Respawn
        if(playerGameObject.transform.position.y <= -350)
        {
            rb.velocity = Vector3.zero;
            playerGameObject.transform.position = startPosition;
        }
        // Store player view rotation value to avoid getting it reversed and to clamp the view
        playerViewRotation = Mathf.Clamp(playerViewRotation,-5,6); // clamp playerViewRotation
        if(!GameSys.isPaused)
        {
            if(Input.GetAxis("Mouse Y") > 0)
            {
            playerViewRotation -= Input.GetAxis("Mouse Y");
            }else if(Input.GetAxis("Mouse Y")< 0)
            {
            playerViewRotation -= Input.GetAxis("Mouse Y");
            }
        }
        if(!GameSys.isPaused)
        {
            Vector3 r;
            Vector3 r2;
            r = new Vector3(Input.GetAxis("Mouse Y")*-1*viewSpeed*GameSys.mouseSensitivity,0,0);
            r2 = new Vector3(this.gameObject.transform.rotation.x,Input.GetAxis("Mouse X")*viewSpeed*GameSys.mouseSensitivity,this.gameObject.transform.rotation.z);
            if(playerViewRotation > -5 && playerViewRotation < 6) // Clamp vertical view rotation
            {playerView.transform.Rotate(r);}
            this.gameObject.transform.Rotate(r2);
        }
    }

    // FixedUpdate is called based on physics
    void FixedUpdate()
    {           
        // Player Movement
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        if(isGrounded)
        {
            rb.velocity = new Vector3(transform.forward.x*verticalAxis*speed,rb.velocity.y,transform.forward.z*verticalAxis*speed); // Walk
            rb.AddForce(transform.right*horizontalAxis*speed,ForceMode.VelocityChange); // Strafe
        }

        // Jumping
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up*jumpForce*10);
            isGrounded = false;
        }

        // Air Control
        if(!onRamp && !isGrounded)
        {
            if(horizontalAxis != 0)
            {
                rb.AddForce(playerView.transform.right*Input.GetAxisRaw("Mouse X")*airStrafe,ForceMode.VelocityChange);
            }
            // if(Input.GetAxis("Mouse X") != 0)
            // {
            //     Vector3 desiredDirection = transform.forward; // set this to the direction you want.
            //     Vector3 newVelocity = desiredDirection.normalized * rb.velocity.magnitude;
            //     rb.velocity = new Vector3(newVelocity.x,rb.velocity.y,newVelocity.z);
            // }
        }
    }

    // Called when this object collide with another one
    void OnCollisionEnter(Collision col)
    {
        // Check if player is grounded
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            airStrafe = 0;
        }

        // Check wall hits
        if(col.gameObject.tag == "Wall")
        {
            airStrafe = 0;
        }

        // Check if player has hit a wall
        if(col.gameObject.tag == "Wall")
        {
            rb.velocity = Vector3.zero;
        }

        if(col.gameObject.tag == "Ramp")
        {
            rb.AddForce(transform.forward*playerViewRotation,ForceMode.VelocityChange);
            airStrafe = 0;
        }
    }

    // Called while this object is touching another object
    void OnCollisionStay(Collision col)
    {
        // Apply Ramp Physics
        if(col.gameObject.tag == "Ramp")
        {
            onRamp = true;
            float verticalAxis = Input.GetAxis("Vertical");
            float horizontalAxis = Input.GetAxis("Horizontal");
            rb.AddForce(transform.right*horizontalAxis*rampStrafe,ForceMode.VelocityChange); // Ramp strafe
            if(Input.GetAxis("Mouse Y") > -1.5f && Input.GetAxis("Mouse Y") < 1.5f)
            {
                rb.AddForce(transform.up*Input.GetAxis("Mouse Y")*inclinationMultiplier,ForceMode.VelocityChange);
            }
        }
    }

    // Called when this object isn't touching another one anymore
    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "Ramp")
        {
            onRamp = false;
            rb.AddForce(playerView.transform.forward*rb.velocity.magnitude*rampExitImpulse*10);
            airStrafe = 6;
        }
        // Check if player is grounded
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    float MakePositive(float value)
    {
        if(value < 0 )
        {
            value *= -1;
            return value;
        }

        return value;
    }
}
