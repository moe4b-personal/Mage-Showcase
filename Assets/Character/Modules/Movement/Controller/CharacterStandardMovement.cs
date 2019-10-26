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

        var target = camera.AlignedForward * Input.Movement.Y.RawValue + camera.Right * Input.Movement.X.RawValue;
        target = Vector3.ClampMagnitude(target, 1f);

        Velocity = Vector3.MoveTowards(Velocity, target, acceleration * Time.deltaTime);

        Animator.SetFloat("Movement", Velocity.magnitude + Sprint.Value);

        Rotation.Process(Velocity);

        ApplyVelocity(Animator.velocity);
    }
}