using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : Character.Module
{
    [SerializeField]
    DirectionalAxisInput movement = new DirectionalAxisInput("Move X", "Move Y");
    public DirectionalAxisInput Movement => movement;

    [SerializeField]
    DirectionalAxisInput look = new DirectionalAxisInput("Look X" , "Look Y");
    public DirectionalAxisInput Look => look;
    
    [SerializeField]
    ButtonInput jump = new ButtonInput(nameof(Jump));
    public ButtonInput Jump => jump;

    [SerializeField]
    AxisInput sprint = new AxisInput(nameof(Sprint));
    public AxisInput Sprint => sprint;

    [SerializeField]
    ButtonInput aim = new ButtonInput(nameof(Aim));
    public ButtonInput Aim => aim;

    [SerializeField]
    ButtonInput attack = new ButtonInput(nameof(Attack));
    public ButtonInput Attack => attack;

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    private void Process()
    {
        movement.Process();

        look.Process();

        jump.Process();

        sprint.Process();

        aim.Process();

        attack.Process();
    }
}

[Serializable]
public class ButtonInput
{
    [SerializeField]
    string name;

    public bool Value { get; protected set; }

    public void Process()
    {
        Value = Input.GetButton(name);
    }

    public ButtonInput(string name)
    {
        this.name = name;
    }
}

[Serializable]
public class AxisInput
{
    [SerializeField]
    string name;

    public float Value { get; protected set; }

    public float RawValue { get; protected set; }

    public void Process()
    {
        Value = Input.GetAxis(name);

        RawValue = Input.GetAxisRaw(name);
    }

    public AxisInput(string name)
    {
        this.name = name;
    }
}

[Serializable]
public class DirectionalAxisInput
{
    [SerializeField]
    protected AxisInput x;
    public AxisInput X { get { return x; } }

    [SerializeField]
    protected AxisInput y;
    public AxisInput Y { get { return y; } }

    public void Process()
    {
        x.Process();
        y.Process();
    }

    public DirectionalAxisInput(string x, string y)
    {
        this.x = new AxisInput(x);
        this.y = new AxisInput(y);
    }
}