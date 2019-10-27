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

public class CharacterCameraFOV : CharacterCamera.Module
{
    [SerializeField]
    float speed = 10f;

    public float Anchor { get; protected set; }

    public float Value { get => camera.Component.fieldOfView; set => camera.Component.fieldOfView = value; }

    public float Multiplier
    {
        set
        {
            Value = Anchor * value;
        }
    }

    public class Module : CharacterCamera.Module
    {
        public CharacterCameraFOV FOV => camera.FOV;
    }

    public CharacterCameraFOVZoom Zoom { get; protected set; }

    public override void Configure(Character character)
    {
        base.Configure(character);

        Zoom = Character.FindProperty<CharacterCameraFOVZoom>();
    }

    protected override void Init()
    {
        base.Init();

        Anchor = Value;
    }
}