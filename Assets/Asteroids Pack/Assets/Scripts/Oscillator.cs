using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startPos;

    [SerializeField] float speed = 10;

    [SerializeField] float distance = 20f;

    [SerializeField] Vector3 movementVector = Vector3.up;

    void Start()
    {
        startPos = transform.position;
    }


    void Update()
    {
        float newPosition = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPos + movementVector.normalized * newPosition;
    }
}
