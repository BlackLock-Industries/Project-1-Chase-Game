using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Movement : MonoBehaviour
{
    //component references
    Rigidbody2D rb;
    Transform tr;

    //public variables:
    //acceleration is the force applied when the player is pressing the forward button
    public float acceleration = 5f;
    //turnSpeed is the speed at which the player turns
    public float turnSpeed = 5f;
    //driftPower is the amount of drift applied when the player is turning, the lower the value the less drift
    public float driftPower = 0.95f;
    //maxSpeed is the maximum speed the player can go
    public float maxSpeed = 20;

    //local variables:
    //accelerationInput is the input from the player, it is a value between -1 and 1
    float accelerationInput = 0;
    //steeringinpuit is the input from the player, it is a value between -1 and 1
    float steeringinput = 0;
    //rotationAngle is the angle the player is facing
    float rotationAngle = 0;
    //velocityVsUp is the velocity of the player in the direction the player is facing
    float velocityVsUp = 0;

    // Start is called before the first frame update
    void Start()
    {
        //get the rigidbody and transform components
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //apply the force to the player
        ApplyForce();
        //apply the steering to the player
        ApplySteering();
        //kill the ortho velocity
        KillOrthoVelocity();

    }
    public void SetInputVector(Vector2 inputVector)
    {
        //set the input vector
        accelerationInput = inputVector.y;
        //set the steering input
        steeringinput = inputVector.x;
    }
    void ApplySteering()
    {
        //calculate the min turn speed
        float minTurnSpeed = (rb.velocity.magnitude / 8);
        //clamp the min turn speed
        minTurnSpeed = Mathf.Clamp01(minTurnSpeed);
        //calculate the turn speed
        rotationAngle -= steeringinput * turnSpeed;
        //rotate the player
        rb.MoveRotation(rotationAngle);
    }
    void ApplyForce()
    {
        //calculate the velocity of the player in the direction the player is facing
        velocityVsUp = Vector2.Dot(rb.velocity, transform.up);
        //if the player is going faster than the max speed and the player is trying to accelerate, return
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
        {
            return;
        }
        //if the player is going faster than half the max speed and the player is trying to decelerate, return
        if (velocityVsUp < -maxSpeed / 2 && accelerationInput < 0)
        {
            return;
        }
        
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
        {
            return;
        }
        //if the player is not trying to accelerate, lower velocity
        if (accelerationInput == 0)
        {
            rb.drag =  Mathf.Lerp(rb.drag, 3, Time.fixedDeltaTime * 3);
        }
        else rb.drag = 0;
        //calculate the force vector
        Vector2 engineForceVector = transform.up * accelerationInput * acceleration;
        //apply the force to the player
        rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

    void KillOrthoVelocity()
    {
        //calculate the forward velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        //calculate the right velocity
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        //apply the forward velocity
        rb.velocity = forwardVelocity + rightVelocity * driftPower;
    }
}
