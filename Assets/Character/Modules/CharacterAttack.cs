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

        if (ID == "Attack End") End();
    }

    public bool CanPerform
    {
        get
        {
            if (AnimatorBoolParameter) return false;

            if (coroutine != null) return false;

            if (MagicalSpells.Count == 0) return false;

            return true;
        }
    }

    public bool AnimatorBoolParameter
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

    Coroutine coroutine;

    private void Process()
    {
        if(Input.Attack.Value)
        {
            if(CanPerform)
            {
                AnimatorBoolParameter = true;
            }
        }

        if(AnimatorBoolParameter)
        {

        }
    }

    public event Action OnConnected;
    void Connected()
    {
        Launch();

        IEnumerator Procedure()
        {
            while (Input.Attack.Value && MagicalSpells.Count > 0)
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
            }

            Stop();

            bool HasEnded() => coroutine == null;

            yield return new WaitUntil(HasEnded);
        }

        coroutine = StartCoroutine(Procedure());

        OnConnected?.Invoke();
    }

    void Launch()
    {
        var spell = MagicalSpells.First;

        var element = MagicalSpells.Ring.Elements.Find(x => x.Spell == spell);

        MagicalSpells.Remove(spell);

        var direction = camera.Forward;

        if (camera.RayCast.HasHit)
            direction = spell.DirectionTo(camera.RayCast.Hit.point);

        spell.Launch(direction * 20);
    }

    public event Action OnStop;
    void Stop()
    {
        AnimatorBoolParameter = false;

        OnStop?.Invoke();
    }

    public event Action OnEnd;
    void End()
    {
        if(coroutine != null)
            StopCoroutine(coroutine);

        coroutine = null;

        OnEnd?.Invoke();
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword