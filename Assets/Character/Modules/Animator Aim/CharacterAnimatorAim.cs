using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class CharacterAnimatorAim : Character.Module
{
    public CharacterBody Body => Character.Body;
    public Animator Animator => Body.Animator;

    public CharacterAnimatorAimWeight Weight { get; protected set; }

    public Transform Head { get; protected set; }

    public override void Configure(Character character)
    {
        base.Configure(character);

        Weight = Character.FindProperty<CharacterAnimatorAimWeight>();
    }

    protected override void Init()
    {
        base.Init();

        Head = Animator.GetBoneTransform(HumanBodyBones.Head);
    }

    public void Set(Vector3 direction)
    {
        Animator.SetLookAtPosition(Head.position + direction);
        Animator.SetLookAtWeight(Weight.Value, Weight.Value / 2f);
    }
}