using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Input the speed")]
    public float speed;

    private Rigidbody2D Body;

    private float Player1control_x;
    private float Player1control_y;
    private float Player2control_x;
    private float Player2control_y;

    private Vector2 movement;
    private float rotation;
    private float theta; //the angle of the box
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // input from arrows
        Player1control_x = Input.GetAxisRaw("Horizontal");
        Player1control_y = Input.GetAxisRaw("Vertical");
        //input from AWSD
        Player2control_x = Input.GetAxisRaw("Horizontal2");
        Player2control_y = Input.GetAxisRaw("Vertical2");

        movement = new Vector2(Player1control_x + Player2control_x, Player1control_y + Player2control_y);

        theta = -Body.transform.rotation.z;
        //Debug.Log("theta: " + theta);
        rotation = Player1control_x * Mathf.Sin(theta)-Player2control_x*Mathf.Sin(theta) +Player1control_y*Mathf.Cos(theta) -Player2control_y*Mathf.Cos(theta);
    }

    private void FixedUpdate()
    {
        Body.AddForce(movement * speed * 10f);
        Body.AddTorque(rotation * speed);
    }
}

public class testss
{
    public float name;
}