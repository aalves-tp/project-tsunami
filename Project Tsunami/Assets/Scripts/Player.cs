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
    public float rampAcceleration;
    public float airStrafe;
    public float rampStrafe;
    public float inclinationMultiplier;
    public float rampExitImpulse;

    private Vector3 movVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerGameObject = rb.gameObject;
        playerViewRotation = 0;
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
        if(playerGameObject.transform.position.y <= -25)
        {
            rb.velocity = Vector3.zero;
            playerGameObject.transform.position = new Vector3(0,2,-4);
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
        if(!onRamp)
        {
        rb.AddForce(transform.right*horizontalAxis*airStrafe,ForceMode.VelocityChange);
        // rb.velocity = new Vector3(rb.velocity.x*airStrafe,rb.velocity.y,rb.velocity.z);
        // rb.AddForce(transform.forward*rb.velocity.magnitude);
        }
    }

    // Called when this object collide with another one
    void OnCollisionEnter(Collision col)
    {
        // Check if player is grounded
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        // Check if player has hit a wall
        if(col.gameObject.tag == "Wall")
        {
            rb.velocity = Vector3.zero;
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
            if(Input.GetAxis("Mouse Y") == 0)
            {
                rb.AddForce(transform.right*horizontalAxis*rampStrafe,ForceMode.VelocityChange); // Ramp strafe
            }else{
                rb.AddForce(transform.right*horizontalAxis*rampStrafe*(Input.GetAxis("Mouse Y")*inclinationMultiplier*10),ForceMode.VelocityChange); // Ramp strafe + mouse influence
            }
        }
    }

    // Called when this object isn't touching another one anymore
    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "Ramp")
        {
            onRamp = false;
            rampAcceleration = 0;
            rb.AddForce(playerView.transform.forward*rb.velocity.magnitude*rampExitImpulse*10);
        }
        // Check if player is grounded
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }
}
