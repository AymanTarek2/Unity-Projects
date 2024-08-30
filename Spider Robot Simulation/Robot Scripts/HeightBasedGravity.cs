using UnityEngine;

public class HeightBasedGravity : MonoBehaviour
{
    private Rigidbody rb;
    public float gravityThresholdY = 1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (transform.position.y <= gravityThresholdY)
        {
            rb.useGravity = true;
        }
        else
        {
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
        }
    }
}
