using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed;
    public float minDirection = 0.5f;

    public float rotationSpeed = 100f;

    public GameObject sparksVFX;

    private Vector3 direction;
    private Rigidbody rb;

    private bool stopped = true;

    private Vector3 currentTorque;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        //transform.position += direction * speed * Time.deltaTime;

    }

    void FixedUpdate()
    {
        if (stopped)
            return;

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
        rb.AddTorque(currentTorque * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        bool hit = false;

        if (other.CompareTag("Wall"))
        {
            direction.z = -direction.z;
            direction += rb.angularVelocity * 0.5f;
            hit = true;

        }

        if (other.CompareTag("Racket"))
        {
            Vector3 newDirection = (transform.position - other.transform.position).normalized;
            newDirection.x = Mathf.Sign(newDirection.x) * Mathf.Max(Mathf.Abs(newDirection.x), this.minDirection);
            newDirection.z = Mathf.Sign(newDirection.z) * Mathf.Max(Mathf.Abs(newDirection.z), this.minDirection);

            newDirection += rb.angularVelocity * 0.5f;
            direction = newDirection;
            hit = true;

            // Update torque for continuous rotation
            Vector3 collisionPoint = other.ClosestPoint(transform.position);
            Vector3 collisionNormal = (transform.position - collisionPoint).normalized;
            currentTorque = Vector3.Cross(direction, collisionNormal) * rotationSpeed;

            Debug.Log($"Updated Torque: {currentTorque}");
        }

        if (hit)
        {
            GameObject sparks = Instantiate(this.sparksVFX, transform.position, transform.rotation);
            Destroy(sparks, 4f);
        }
    }

    private void ChooseDirection()
    {
        float signX = Mathf.Sign(Random.Range(-1f, 1f));
        float signZ = Mathf.Sign(Random.Range(-1f, 1f));
        this.direction = new Vector3(0.5f * signX, 0, 0.5f * signZ);
        Debug.Log("chooseDirection is called");
    }


    public void Stop()
    {
        stopped = true;
    }
    public void Go()
    {
        stopped = false;
        ChooseDirection();
        Debug.Log("GO is called");
    }
}