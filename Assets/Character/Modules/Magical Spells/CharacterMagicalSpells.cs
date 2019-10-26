using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMagicalSpells : Character.Module
{
    [SerializeField]
    GameObject prefab;

    public List<MagicalSpell> Elements { get; protected set; }

    public MagicalSpell this[int index] => Elements[index];

    public int Count => Elements.Count;

    public class Module : Character.Module
    {
        public CharacterMagicalSpells MagicalSpells { get { return Character.MagicalSpells; } }
    }

    public WeaponMagicalSpellsRing Ring { get; protected set; }

    public override void Configure(Character character)
    {
        base.Configure(character);

        Elements = new List<MagicalSpell>();

        Ring = Character.FindProperty<WeaponMagicalSpellsRing>();
    }

    private void Update()
    {
        if(UnityEngine.Input.GetKeyDown(KeyCode.Space))
        {
            Add();
            Add();
            Add();
        }

        if(UnityEngine.Input.GetKeyDown(KeyCode.R) && Elements.Count > 0)
        {
            var instance = Elements[0];

            Remove(instance);

            Destroy(instance.gameObject);
        }
    }

    public delegate void AddDelegate(MagicalSpell spell);
    public event AddDelegate OnAdd;
    MagicalSpell Add()
    {
        var instance = Create();

        Elements.Add(instance);

        OnAdd?.Invoke(instance);

        return instance;
    }

    public delegate void RemoveDelegate(MagicalSpell spell);
    public event RemoveDelegate OnRemove;
    void Remove(MagicalSpell spell)
    {
        if(Elements.Contains(spell) == false)
        {
            Debug.LogWarning("trying to remove non registerd magical spell: " + spell.name);
            return;
        }

        Elements.Remove(spell);

        OnRemove?.Invoke(spell);
    }
    public MagicalSpell Take()
    {
        var instance = Elements[Elements.Count - 1];

        Remove(instance);

        return instance;
    }

    MagicalSpell Create()
    {
        var instance = Instantiate(prefab, Ring.transform);

        instance.name = prefab.name;

        var script = instance.GetComponent<MagicalSpell>();

        script.Configure();
        script.Init();

        script.IgnoreCollision(Character.gameObject);

        script.Particles.SetSimulationSpace(transform);

        return script;
    }
}