using UnityEngine;

public class Player : MonoBehaviour
{
    // Controller Attributes
    float verticalAxis;
    float horizontalAxis;
    float mouseX;
    float mouseY;

    [Header("Player Attributes")]
    public float speed;
    public GameObject playerView;
    [HideInInspector]
    public Rigidbody playerPhysics;
    [HideInInspector]
    public Vector3 startPosition;
    public bool isGrounded;
    public float airStrafe;
    public float airAcceleration;
    [HideInInspector]
    public float playerViewOrientation;
    bool onRamp;
    [SerializeField]
    Transform currentRamp;
    public float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        playerPhysics = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Controller Attributes
        verticalAxis = Input.GetAxisRaw("Vertical");
        horizontalAxis = Input.GetAxisRaw("Horizontal");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        if(currentRamp != null)
        {
            playerViewOrientation = ((playerView.transform.localRotation.x - currentRamp.transform.localRotation.x)*100)*-1;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            playerPhysics.AddForce(transform.up*jumpForce,ForceMode.Impulse);
        }

        if(transform.position.y < -60)
        {
            transform.position = startPosition;
            playerPhysics.velocity = Vector3.zero;
        }
    }

    // FixedUpdate is called based on Physics
    void FixedUpdate()
    {
        // Player Movement
        // Ground Control
        if(isGrounded)
        {
            if(playerPhysics.velocity.magnitude < 10)
            {
                playerPhysics.AddForce(transform.forward*speed*verticalAxis*10); // Walk
                playerPhysics.AddForce(transform.right*speed*horizontalAxis*10); // Strafe
            }
        }else{
            playerPhysics.AddForce(transform.forward*airAcceleration,ForceMode.Acceleration);
            // Ramp Control
            if(onRamp)
            {
                Debug.Log(PositiveFloat(currentRamp.transform.rotation.x));
                // airAcceleration = Mathf.Lerp(airAcceleration,0.5f,Time.deltaTime/2);
                // playerPhysics.AddForce(currentRamp.transform.right*speed*horizontalAxis*airStrafe); // Strafe + Mouse Influence
                if(playerViewOrientation < 5 && playerViewOrientation > -5)
                {
                    if(PositiveFloat(currentRamp.transform.rotation.x) < 0.1f)
                    {
                        playerPhysics.AddForce(transform.right*speed*horizontalAxis*(playerPhysics.velocity.y*-1)); // Strafe + Mouse Influence
                    }else
                    {
                        // playerPhysics.AddForce(transform.right*(horizontalAxis/2)); // Strafe + Mouse Influence
                    }
                }else
                {
                    if(playerViewOrientation > 5)
                    {
                        GameSys.getRampPhysicsMaterial.dynamicFriction += 0.005f;
                        playerPhysics.AddForce(transform.right*speed*horizontalAxis*airStrafe); // Strafe + Mouse Influence
                    }else{
                        if(GameSys.getRampPhysicsMaterial.dynamicFriction > 0.01)
                        {
                            GameSys.getRampPhysicsMaterial.dynamicFriction -= 0.01f;
                        }
                        if(airAcceleration < 5)
                        {
                            airAcceleration+=0.1f;
                        }
                    }
                }
            }
        }
        // Player Rotation
        Quaternion r = Quaternion.Euler(transform.rotation.x,mouseX*GameSys.mouseSensitivity,transform.rotation.z);
        transform.rotation *= r;
        // Player View Rotation
        Quaternion r2 = Quaternion.Euler((mouseY*-1)*GameSys.mouseSensitivity,playerView.transform.localRotation.y,playerView.transform.localRotation.z);
        playerView.transform.localRotation *= r2;
    }

    void OnCollisionEnter(Collision col)
    {

    }
    
    // Called while this object is touching another object
    void OnCollisionStay(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            airAcceleration = 1;
        }

        if(col.gameObject.tag == "Ramp")
        {
            onRamp = true;
            currentRamp = col.transform;
        }
    }

    // Called when this object isn't touching another one anymore
    void OnCollisionExit(Collision col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }

        if(col.gameObject.tag == "Ramp")
        {
            onRamp = false;
            GameSys.getRampPhysicsMaterial.dynamicFriction = 0;
        }

        if(col.gameObject.tag == "Wall")
        {
            playerPhysics.velocity = Vector3.zero;
        }
    }

    float PositiveFloat(float value)
    {
        if(value < 0)
        {
            value *= -1;
        }
        return value;
    }
}
