using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Objects and Components")]
    GameObject playerGameObject;
    Rigidbody rb;
    [Header("Movement and Camera")]
    public float speed;
    public Camera playerView;
    public float viewSpeed;
    public float jumpForce;
    [SerializeField]
    bool isGrounded;
    [Header("Physics and Collisions")]
    public float rampGravity;
    public float rampAcceleration;
    public float airStrife;

    private Vector3 movVector;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerGameObject = rb.gameObject;
    }

    // Update is called once per frame
    
    void Update()
    {
        // Lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        {
            
        };
        // Fall Respawn
        if(playerGameObject.transform.position.y <= -10)
        {
            playerGameObject.transform.position = new Vector3(0,2,-4);
        }

        // Jumping
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up*jumpForce*10);
            isGrounded = false;
        }

        // Look Control
        Vector3 r;
        Vector3 r2;
        r = new Vector3(Input.GetAxis("Mouse Y")*-1*viewSpeed,0,0);
        r2 = new Vector3(this.gameObject.transform.rotation.x,Input.GetAxis("Mouse X")*viewSpeed,this.gameObject.transform.rotation.z);
        playerView.transform.Rotate(r);
        this.gameObject.transform.Rotate(r2);
    }

    void FixedUpdate()
    {           
        Debug.Log(rb.velocity.magnitude);
        // Player Movement
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        if(isGrounded)
        {
            rb.velocity = new Vector3(transform.forward.x*verticalAxis*speed,rb.velocity.y,transform.forward.z*verticalAxis*speed);
            rb.AddForce(transform.right*horizontalAxis*speed,ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // Check if player is grounded
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionStay(Collision col)
    {
        // Apply Ramp Gravity
        if(col.gameObject.tag == "Ramp")
        {
            float verticalAxis = Input.GetAxis("Vertical");
            float horizontalAxis = Input.GetAxis("Horizontal");
            rb.AddForce(new Vector3(transform.forward.x*rampAcceleration,rampGravity,transform.forward.z*rampAcceleration),ForceMode.VelocityChange);
            rb.AddForce(transform.right*horizontalAxis*airStrife,ForceMode.VelocityChange);
        }
    }

    void OnCollisionExit(Collision col)
    {
        // Check if player is grounded
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }

        // // Apply Ramp Gravity
        // if(col.gameObject.tag == "Ramp")
        // {
        //     rb.AddForce(new Vector3(transform.forward.x*100,rampGravity,transform.forward.z*rb.velocity.magnitude));
        // }
    }
}
