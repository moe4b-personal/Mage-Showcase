using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
public class CharacterAttack : Character.Module
{
    public Animator Animator => Character.Animator;

    public CharacterMagicalSpells MagicalSpells => Character.MagicalSpells;

    public CharacterCamera camera => Character.camera;

    public float Progress => Animator.GetFloat("Attack-Progress");

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;

        Character.Body.OnEvent += EventCallback;
    }

    private void EventCallback(string ID)
    {
        if (ID == "Attack Connected") Connected();
    }

    public bool CanPerform
    {
        get
        {
            if (Bool) return false;

            if (MagicalSpells.Count == 0) return false;

            return true;
        }
    }

    public bool Bool
    {
        get
        {
            return Animator.GetBool("Attack");
        }
        set
        {
            Animator.SetBool("Attack", value);
        }
    }

    private void Process()
    {
        Debug.Log(Progress);

        if(Input.Attack.Value)
        {
            if(CanPerform)
            {
                Bool = true;
            }
        }
    }

    public event Action OnConnected;
    void Connected()
    {
        Launch();

        IEnumerator Procedure()
        {
            while (MagicalSpells.Count > 0)
            {
                var timer = 0.75f;

                while(timer > 0f)
                {
                    timer = Mathf.MoveTowards(timer, 0f, Time.deltaTime);

                    if (Input.Attack.Value == false) break;

                    yield return new WaitForEndOfFrame();
                }

                if (Input.Attack.Value)
                    Launch();
                else
                    break;
            }

            yield return new WaitForSeconds(0.5f);

            End();
        }

        StartCoroutine(Procedure());

        OnConnected?.Invoke();
    }

    public event Action OnEnd;
    void End()
    {
        Bool = false;

        OnEnd?.Invoke();
    }

    void Launch()
    {
        var spell = MagicalSpells.Take();

        var direction = camera.Forward;

        if (camera.RayCast.HasHit)
            direction = (camera.RayCast.Hit.point - spell.transform.position).normalized;

        spell.Launch(direction * 20);
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword