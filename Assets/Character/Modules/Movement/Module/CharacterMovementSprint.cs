using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementSprint : CharacterMovement.Module
{
    public float Value { get; protected set; }

    [SerializeField]
    float acceleration = 5f;

    public void Process()
    {
        if(Input.Sprint.Value > 0f)
        {
            Calculate(Input.Sprint.Value, acceleration);
        }
        else
        {
            Stop();
        }
    }

    void Calculate(float target, float delta)
    {
        Value = Mathf.MoveTowards(Value, target, delta * Time.deltaTime);
    }

    public void Stop()
    {
        Calculate(0f, acceleration);
    }
}