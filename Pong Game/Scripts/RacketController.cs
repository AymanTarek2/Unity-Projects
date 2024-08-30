using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacketController : MonoBehaviour
{
    public float speed;
    public KeyCode up;
    public KeyCode down;

    public bool isPlayer = true;

    public float offset = 0.2f;

    private Rigidbody rb;
    private Transform ball;

    
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        ball = GameObject.FindGameObjectWithTag("Ball").transform;
    }

    
    void Update()
    {
        if (this.isPlayer)
            MoveByPlayer();

        else
            MoveByComputer();
    }
    private void MoveByPlayer()
    {
        bool pressedUp = Input.GetKey(this.up);
        bool pressedDown = Input.GetKey(this.down);

        if (pressedUp && !pressedDown)
            rb.velocity = Vector3.forward * speed;
        else if (pressedDown && !pressedUp)
            rb.velocity = Vector3.back * speed;
        else
            rb.velocity = Vector3.zero;
    }

    private void MoveByComputer()
    {
        if (ball.position.z > transform.position.z + offset)
            rb.velocity = Vector3.forward * speed;
        else if (ball.position.z < transform.position.z - offset)
            rb.velocity = Vector3.back * speed;
        else
            rb.velocity = Vector3.zero;
    }
}
