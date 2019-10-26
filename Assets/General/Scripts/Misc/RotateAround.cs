using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Vector3 axis = Vector3.up;

    public float speed = 90f;

    void Update()
    {
        transform.Rotate(axis * speed * Time.deltaTime, Space.Self);
    }
}