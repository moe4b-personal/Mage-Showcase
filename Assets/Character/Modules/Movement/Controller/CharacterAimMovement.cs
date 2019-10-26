using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimMovement : CharacterMovement.Controller
{
    [SerializeField]
    float acceleration = 5f;

    public Vector3 Velocity { get; protected set; }

    public CharacterHeadAim HeadAim { get; protected set; }

    protected override void Init()
    {
        base.Init();

        HeadAim = Character.FindProperty<CharacterHeadAim>();

        Character.OnLateProcess += LateProcess;
    }

    protected override void Process()
    {
        if (Input.Aim.Value == false) return;

        base.Process();

        var target = camera.AlignedForward * Input.Movement.Y.RawValue + camera.Right * Input.Movement.X.RawValue;
        target = Vector3.ClampMagnitude(target, 1f);

        Velocity = Vector3.MoveTowards(Velocity, target, acceleration * Time.deltaTime);

        var localVelocity = Character.transform.InverseTransformDirection(Velocity);

        Animator.SetFloat("Horizontal", localVelocity.x);
        Animator.SetFloat("Vertical", localVelocity.z);

        Rotation.Process(camera.AlignedForward, Rotation.Speed * 2f);

        ApplyVelocity(Animator.velocity);
    }

    private void LateProcess()
    {
        HeadAim.WeightTarget = Input.Aim.Value ? 1f : 0f;

        HeadAim.Set(camera.Forward);
    }
}