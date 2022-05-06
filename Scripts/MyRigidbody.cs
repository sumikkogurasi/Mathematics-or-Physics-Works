using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyRigidbody : MonoBehaviour
{
    public Vector3 acceleration;
    public Vector3 velocity;
    public Vector3 position;

    const float dt = 1f / 60f;

    void Start()
    {
        position = transform.position;
    }

    public void AddForce(Vector3 force)
    {
        acceleration += force;
    }

    void FixedUpdate()
    {
        velocity += acceleration * dt;
        position += velocity * dt;

        if(position.y < 0.4f)
        {
            velocity = -velocity;
        }

        transform.position = position;
        acceleration = Vector3.zero;
    }
}
