using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeaponMagicalSpellsRing : CharacterMagicalSpells.Module
{
    [SerializeField]
    float radius = 0.5f;

    [SerializeField]
    float speed = 240f;

    public List<ElementData> Elements { get; protected set; }
    [Serializable]
    public class ElementData
    {
        public MagicalSpell Spell { get; protected set; }

        public float Angle { get; protected set; }

        public float Distance { get; protected set; }

        public WeaponMagicalSpellsRing Ring { get; protected set; }

        public int Index => Ring.Elements.IndexOf(this);

        public void Process()
        {
            Distance = Mathf.MoveTowards(Distance, Ring.radius, 1.2f * Time.deltaTime);

            var target = (Index / 1f / Ring.Elements.Count) * 360f;

            Angle = Mathf.MoveTowardsAngle(Angle, target, Ring.speed / 10f * Time.deltaTime);

            var position = Ring.CalculatePosition(target, Distance);

            Spell.transform.localPosition = position;
        }

        public ElementData(WeaponMagicalSpellsRing ring, MagicalSpell spell)
        {
            this.Spell = spell;
            this.Ring = ring;
        }
    }

    public override void Configure(Character character)
    {
        base.Configure(character);

        Elements = new List<ElementData>();
    }

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;

        MagicalSpells.OnAdd += Register;
        MagicalSpells.OnRemove += UnRegister;
    }

    private void Register(MagicalSpell spell)
    {
        var instance = new ElementData(this, spell);

        Elements.Add(instance);
    }
    private void UnRegister(MagicalSpell spell)
    {
        bool Predicate(ElementData target) => target.Spell == spell;

        Elements.RemoveAll(Predicate);
    }

    private void Process()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);

        for (int i = 0; i < Elements.Count; i++)
        {
            var rate = i / 1f / Elements.Count;

            Elements[i].Process();
        }
    }

    public Vector3 CalculatePosition(float angle, float distance)
    {
        var rotation = Quaternion.Euler(0f, 0f, angle);

        var position = rotation * MagicalSpells.transform.up * distance;

        return position;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;

        Handles.DrawWireDisc(transform.position, transform.forward, radius);
#endif
    }
}