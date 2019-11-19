using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementSprint : CharacterMovement.Module
{
    public float Value { get; protected set; }

    [SerializeField]
    float acceleration = 5f;

    public bool CanAccelerate
    {
        get
        {
            if (Input.Sprint.Value == 0f) return false;

            return true;
        }
    }

    public void Process()
    {
        if(CanAccelerate)
        {
            Acccelerate();
        }
        else
        {
            Halt();
        }
    }

    void Calculate(float target, float delta)
    {
        Value = Mathf.MoveTowards(Value, target, delta * Time.deltaTime);
    }

    public void Acccelerate()
    {
        Calculate(Input.Sprint.Value, acceleration);
    }

    public void Halt()
    {
        Calculate(0f, acceleration);
    }
}