using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementSprint : CharacterMovement.Module
{
    public float Value { get; protected set; }

    [SerializeField]
    float acceleration = 5f;

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    void Process()
    {
        void Calculate(float target, float delta)
        {
            Value = Mathf.MoveTowards(Value, target, delta * Time.deltaTime);
        }

        if(Input.Sprint.Value)
        {
            Calculate(1f, acceleration);
        }
        else
        {
            Calculate(0f, acceleration);
        }
    }
}