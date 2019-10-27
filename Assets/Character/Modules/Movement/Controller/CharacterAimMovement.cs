using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAimMovement : CharacterMovement.Controller
{
    [SerializeField]
    float acceleration = 5f;

    [SerializeField]
    [Range(0f, 1f)]
    float zoom = 0.7f;

    public Vector3 Velocity { get; protected set; }

    public CharacterAnimatorAim AnimatorAim => Character.AnimatorAim;

    protected override void Init()
    {
        base.Init();

        Character.Body.AnimatorIKEvent += AnimatorIKCallback;
    }

    protected override void Process()
    {
        camera.FOV.Zoom.Target = Input.Aim.Value ? zoom : 1f;

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

    private void AnimatorIKCallback()
    {
        AnimatorAim.Weight.Target = Input.Aim.Value ? 1f : 0f;

        AnimatorAim.Set(camera.Forward);
    }
}