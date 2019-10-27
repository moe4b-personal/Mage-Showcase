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

public class CharacterCameraFOVZoom : CharacterCameraFOV.Module
{
    [SerializeField]
    float speed = 5f;

    float _value = 1f;
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

            FOV.Multiplier = _value;
        }
    }

    float _target = 1f;
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

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    private void Process()
    {
        Value = Mathf.MoveTowards(Value, Target, speed * Time.deltaTime);
    }
}