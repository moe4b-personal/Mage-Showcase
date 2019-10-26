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
        if (ID == "Attack Connected")
            AttackConnected();
    }

    public event Action OnAttackConnected;
    void AttackConnected()
    {
        OnAttackConnected?.Invoke();
    }

    public bool CanPerform
    {
        get
        {
            if (IsProcessing) return false;

            if (MagicalSpells.Count == 0) return false;

            return true;
        }
    }

    private void Process()
    {
        if(Input.Attack.Value)
        {
            if(CanPerform)
            {
                coroutine = StartCoroutine(Procedure());
            }
        }
    }

    Coroutine coroutine;
    public bool IsProcessing => coroutine != null;
    IEnumerator Procedure()
    {
        Animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.5f);

        var instance = MagicalSpells.Take();

        Launch(instance);

        coroutine = null;
    }

    void Launch(MagicalSpell spell)
    {
        var direction = camera.Forward;

        if (camera.RayCast.HasHit)
            direction = (camera.RayCast.Hit.point - spell.transform.position).normalized;

        if (camera.RayCast.HasHit)
            Debug.DrawRay(spell.transform.position, direction * 40f, Color.green, 10f);

        spell.Launch(direction * 20);
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword