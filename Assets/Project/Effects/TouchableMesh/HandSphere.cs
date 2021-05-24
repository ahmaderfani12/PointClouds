using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSphere : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }
    public float Radious { get; private set; }

    private Vector3 previous;

    //Run every frame
    private void Update()
    {
        Velocity = (transform.position - previous) / Time.deltaTime;
        previous = transform.position;
        Radious = transform.localScale.x/2;
    }
}
