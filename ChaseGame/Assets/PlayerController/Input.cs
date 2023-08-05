using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    //component references
    Movement movement;

    void Awake()
    {
        //get the movement component
        movement = GetComponent<Movement>();
    }
    // Update is called once per frame
    void Update()
    {
        //get the input vector
        Vector2 inputvector = Vector2.zero;
        //set the input vector based on the input
        inputvector.x = UnityEngine.Input.GetAxis("Horizontal");
        inputvector.y = UnityEngine.Input.GetAxis("Vertical");
        //set the input vector in the movement component
        movement.SetInputVector(inputvector);
    }
}
