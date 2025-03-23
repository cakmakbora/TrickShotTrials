using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float runSpeed = 5f;
    public float jumpForce = 5f;
    private float currentSpeed = 0f;
    private float speedSmoothTime = 0.1f;
    private float speedSmoothVelocity;

    [Header("Mouse Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    private float xRotation = 0f;
    private float yRotation = 0f;

    [Header("Throw Settings")]
    public float throwForce = 10f;
    public float strongthrowForce = 15f;
    private float currentthrowForce;

    [Header("Others")]
    public GameObject ballPosition;
    public GameObject Player;
    public GameObject currentBall;

    private Rigidbody rb;
    private bool closed = true;
    private bool hasball = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // The player’s rotation starts at y = -90
        transform.rotation = Quaternion.Euler(0f, -90f, 0f);

        
        Vector3 initialCamRotation = playerCamera.localEulerAngles;
        xRotation = initialCamRotation.x;

        
        yRotation = -90f;
        Invoke(nameof(enableLook), 0.5f);
    }

    private void enableLook()
    {
        closed = false;
    }
    void Update()
    {
        if (!closed)
        {
            LookAround();
        }
        if (!hasball)
        {
            Move();
        }
        
        Jump();
        if (currentBall != null)
            ThrowBall();
        if (currentBall != null && hasball)
        {
            currentBall.transform.position = ballPosition.transform.position;
            currentBall.transform.rotation = ballPosition.transform.rotation;
        }
    }

    private void LookAround()
    {
        
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += mouseX;

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, speedSmoothTime);

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 newVelocity = new Vector3(move.x * currentSpeed, rb.velocity.y, move.z * currentSpeed);
        rb.velocity = newVelocity;
    }
    
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // Raycast down just below the player
        return Physics.Raycast(transform.position, Vector3.down, 1.5f);
    }

    private void GrabBall(GameObject Ball)
    {
        Ball.transform.position = ballPosition.transform.position;
        Rigidbody Ballrb = Ball.GetComponent<Rigidbody>();
        Ballrb.isKinematic = true;
        Ball.GetComponent<Collider>().enabled = false;
        Ballrb.detectCollisions = false;
        currentBall = Ball;
        hasball = true;
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
    }

    private void ThrowBall()
    {
        if (currentBall != null && Input.GetMouseButtonDown(0))
        {
            Rigidbody Ballrb = currentBall.GetComponent<Rigidbody>();
            Ballrb.isKinematic = false;
            Ballrb.detectCollisions = true;
            currentBall.GetComponent<Collider>().enabled = true;
            Ballrb.WakeUp();

            Vector3 throwDirection = playerCamera.forward;
            if (IsGrounded())
            {
                 currentthrowForce = throwForce;
            }
            else
                 currentthrowForce = strongthrowForce;
            Ballrb.AddForce(throwDirection * currentthrowForce, ForceMode.Impulse);
            Ballrb.AddTorque(Random.insideUnitSphere * 5f, ForceMode.Impulse);
            currentBall.tag = "ThrownBall";

            currentBall = null; // Done throwing
            hasball = false;
            
        }
    }

    
        
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag ("BallGrab"))
        {
            GameObject parentObject = other.transform.parent.gameObject;
            GrabBall(parentObject);
            other.gameObject.SetActive(false);
            
        }
        
    }
}
