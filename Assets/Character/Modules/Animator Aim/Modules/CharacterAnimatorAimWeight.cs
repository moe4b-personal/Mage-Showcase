using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorAimWeight : Character.Module
{
    [SerializeField]
    float speed = 5f;

    [SerializeField]
    protected float value = 0f;
    public float Value
    {
        get
        {
            return value;
        }
        set
        {
            value = Mathf.Clamp01(value);

            this.value = value;
        }
    }

    [SerializeField]
    protected float target = 0f;
    public float Target
    {
        get
        {
            return target;
        }
        set
        {
            value = Mathf.Clamp01(value);

            this.target = value;
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