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
    float speed = 2f;

    [SerializeField]
    Vector3 offset = Vector3.zero;

    public Transform Bone { get; protected set; }

    float _weight;
    public float Weight
    {
        get
        {
            return _weight;
        }
        set
        {
            _weight = value;

            UpdateState();
        }
    }

    float _target;
    public float WeightTarget
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

    public Quaternion Rotation { get; protected set; }

    protected override void Init()
    {
        base.Init();

        Bone = Character.Animator.GetBoneTransform(HumanBodyBones.Neck);

        Character.OnLateProcess += LateProcess;
    }

    private void LateProcess()
    {
        Weight = Mathf.MoveTowards(Weight, WeightTarget, speed * Time.deltaTime);
    }

    public void Set(Vector3 direction)
    {
        Rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(offset);
    }

    void UpdateState()
    {
        Set(Rotation);
    }

    void Set(Quaternion rotation)
    {
        Bone.rotation = Quaternion.Lerp(Bone.rotation, rotation, Weight);
    }
}