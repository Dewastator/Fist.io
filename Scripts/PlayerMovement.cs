using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystick;
    float joystickMagnitude;
    public float speed, turnSpeed;
    float angle;

    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundLayer;
    CharacterController characterController;
    Vector3 velocity;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        angle = transform.localEulerAngles.y;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        
        var joystickDir = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;
        joystickMagnitude = joystickDir.magnitude;
        var targetAngle = Mathf.Atan2(joystickDir.x, joystickDir.z) * Mathf.Rad2Deg;
        angle = Mathf.LerpAngle(angle, targetAngle, Time.deltaTime * turnSpeed * joystickMagnitude);

        velocity = transform.forward * speed * joystickMagnitude;
        
    }


    private void FixedUpdate()
    {
        rb.MoveRotation(Quaternion.Euler(Vector3.up * angle));
        rb.MovePosition(rb.position + velocity * Time.deltaTime);
    }


}
