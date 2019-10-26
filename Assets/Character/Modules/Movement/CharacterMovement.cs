using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
public class CharacterMovement : Character.Module
{
    public CharacterMovementSprint Sprint { get; protected set; }
    public CharacterMovementRotation Rotation { get; protected set; }

    public class Module : Character.Module
    {
        public CharacterMovement Movement { get { return Character.Movement; } }
    }

    public class Controller : Module
    {
        public CharacterMovementSprint Sprint => Movement.Sprint;
        public CharacterMovementRotation Rotation => Movement.Rotation;

        public Rigidbody rigidbody => Character.rigidbody;
        public CapsuleCollider collider => Character.collider;
        public Animator Animator => Character.Animator;

        public CharacterCamera camera => Character.camera;

        protected override void Init()
        {
            base.Init();

            Character.OnProcess += Process;
        }

        protected virtual void Process()
        {
            
        }

        protected virtual void ApplyVelocity(Vector3 velocity)
        {
            velocity.y = rigidbody.velocity.y;

            rigidbody.velocity = velocity;
        }
    }

    public Rigidbody rigidbody => Character.rigidbody;
    public CapsuleCollider collider => Character.collider;
    public Animator Animator => Character.Animator;

    public CharacterCamera camera => Character.camera;

    public override void Configure(Character character)
    {
        base.Configure(character);

        Sprint = Character.FindProperty<CharacterMovementSprint>();
        Rotation = Character.FindProperty<CharacterMovementRotation>();
    }

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    private void Process()
    {
        if(Character.Attack.IsProcessing)
        {

        }
        else
        {
            Animator.SetBool("Aim", Input.Aim.Value);
        }
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword