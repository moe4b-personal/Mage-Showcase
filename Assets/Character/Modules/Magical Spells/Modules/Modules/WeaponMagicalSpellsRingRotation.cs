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

public class WeaponMagicalSpellsRingRotation : WeaponMagicalSpellsRing.Module
{
    [SerializeField]
    float speed = 240f;
    public float Speed => speed;

    [SerializeField]
    Vector3 axis = Vector3.forward;

    public float Angle
    {
        set
        {
            Ring.transform.localEulerAngles = axis * value;
        }
    }

    public bool active = true;

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    private void Process()
    {
        if(active)
        {
            Ring.transform.Rotate(axis * speed * Time.deltaTime, Space.Self);
        }
    }
}