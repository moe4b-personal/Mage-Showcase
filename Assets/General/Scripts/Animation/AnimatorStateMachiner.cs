using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorStateMachiner : MonoBehaviour
{
    public Animator Animator { get; protected set; }

    private void Start()
    {
        Animator = GetComponent<Animator>();

        var states = Animator.GetBehaviours<State>();

        for (int i = 0; i < states.Length; i++)
            states[i].Configure(this);
    }

    public event Action OnUpdate;
    private void Update()
    {
        OnUpdate?.Invoke();
    }

    public event Action OnLateUpdate;
    private void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }

    public class State : StateMachineBehaviour
    {
        public AnimatorStateMachiner Machiner { get; protected set; }
        public Animator Animator => Machiner.Animator;

        public virtual void Configure(AnimatorStateMachiner reference)
        {
            this.Machiner = reference;
        }
    }
}