using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStandardMovement : CharacterMovement.Controller
{
    [SerializeField]
    float acceleration = 5f;

    public Vector3 Velocity { get; protected set; }

    protected override void Process()
    {
        if (Input.Aim.Value) return;

        base.Process();

        var target = CalculateVelocityTarget();

        Velocity = Vector3.MoveTowards(Velocity, target, acceleration * Time.deltaTime);

        SetAnimatorVelocity(Velocity);

        Rotation.Process(Velocity);

        Sprint.Process();

        ApplyVelocity(Animator.velocity);
    }
}