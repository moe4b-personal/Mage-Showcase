using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterBody : Character.Module
{
    public Animator Animator { get; protected set; }

    public override void Configure(Character character)
    {
        base.Configure(character);

        Animator = GetComponent<Animator>();
    }

    public delegate void EventDelegate(string ID);
    public event EventDelegate OnEvent;
    void Event(string ID)
    {
        OnEvent?.Invoke(ID);
    }

    public event Action AnimatorMoveEvent;
    private void OnAnimatorMove()
    {
        AnimatorMoveEvent?.Invoke();
    }

    public event Action AnimatorIKEvent;
    private void OnAnimatorIK(int layerIndex)
    {
        AnimatorIKEvent?.Invoke();
    }
}