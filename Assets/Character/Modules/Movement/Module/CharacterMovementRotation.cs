using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementRotation : CharacterMovement.Module
{
    [SerializeField]
    float speed = 240f;
    public float Speed => speed;

    public virtual void Process(Vector3 direction)
    {
        Process(direction, speed);
    }
    public void Process(Vector3 direction, float speed)
    {
        if (direction.magnitude > 0f)
        {
            var rotation = Quaternion.LookRotation(direction, Character.transform.up);

            Character.Rotation = Quaternion.RotateTowards(Character.Rotation, rotation, speed * Time.deltaTime);
        }
    }
}