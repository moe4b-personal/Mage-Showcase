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

public class CharacterHeadAim : Character.Module
{
    [SerializeField]
    AnimatorWeighedController[] bones = new AnimatorWeighedController[] { };

    [SerializeField]
    float speed = 2f;

    [SerializeField]
    Vector3 offset = Vector3.zero;

    public float Weight
    {
        set
        {
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].Target = value;
            }
        }
    }

    public Quaternion Rotation { get; protected set; }

    protected override void Init()
    {
        base.Init();

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].Init(Character.Animator);
        }

        Character.OnLateProcess += LateProcess;
    }

    public void Set(Vector3 direction)
    {
        Rotation = Quaternion.LookRotation(Character.transform.InverseTransformDirection(direction));
    }

    private void LateProcess()
    {
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].LocalRotation = Quaternion.Lerp(Quaternion.identity, Rotation, 0.333f);

            bones[i].Process();
        }
    }
}

[Serializable]
public class AnimatorWeighedController
{
    [SerializeField]
    AnimatorBoneController bone;
    public AnimatorBoneController Bone => bone;

    float _value;
    public float Value
    {
        get
        {
            return _value;
        }
        set
        {
            value = Mathf.Clamp01(value);

            _value = value;
        }
    }

    float _target;
    public float Target
    {
        get
        {
            return _target;
        }
        set
        {
            value = Mathf.Clamp01(value);

            _target = value;
        }
    }

    [SerializeField]
    float speed;

    public Quaternion LocalRotation { get; set; }

    public virtual void Init(Animator animator)
    {
        bone.Init(animator);
    }
    
    public virtual void Process()
    {
        Value = Mathf.MoveTowards(Value, Target, speed * Time.deltaTime);

        bone.transform.localRotation = Quaternion.Lerp(bone.transform.localRotation, LocalRotation, this.Value);
    }
}

[Serializable]
public class AnimatorBoneController
{
    [SerializeField]
    HumanBodyBones target;

    public Transform transform { get; protected set; }

    public virtual void Init(Animator animator)
    {
        transform = animator.GetBoneTransform(target);
    }
}