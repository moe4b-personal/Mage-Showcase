using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
public class CharacterCamera : Character.Module
{
    [SerializeField]
    Transform pivot = null;

    [SerializeField]
    Camera component = null;
    public Camera Component => component;

    [SerializeField]
    float sensitivity = 5f;

    public Vector3 Forward => component.transform.forward;
    public Vector3 AlignedForward => Vector3.Scale(Forward, Vector3.right + Vector3.forward).normalized;
    public Vector3 Right => component.transform.right;

    public class Module : Character.Module
    {
        public CharacterCamera camera => Character.camera;
    }

    public CharacterCameraRaycast RayCast { get; protected set; }

    public CharacterCameraFOV FOV { get; protected set; }

    public IList<Character.IProperty> GetAllModules()
    {
        return GetComponentsInChildren<Character.IProperty>();
    }

    public override void Configure(Character character)
    {
        base.Configure(character);

        RayCast = GetComponentInChildren<CharacterCameraRaycast>();

        FOV = GetComponentInChildren<CharacterCameraFOV>();
    }

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    private void Process()
    {
        transform.position = Character.Position;

        pivot.eulerAngles += Vector3.up * Input.Look.X.Value * sensitivity * Time.deltaTime;
        pivot.eulerAngles += Vector3.right * -Input.Look.Y.Value * sensitivity * Time.deltaTime;
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword