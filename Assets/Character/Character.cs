using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
public class Character : MonoBehaviour
{
    public Rigidbody rigidbody { get; protected set; }
    public CapsuleCollider collider { get; protected set; }

    public Animator Animator => Body.Animator;

    #region Properties
    public interface IProperty
    {
        Character Character { get; }

        void Configure(Character character);
    }

    public class Property : IProperty
    {
        public Character Character { get; protected set; }

        public Animator Animator => Character.Animator;

        public CharacterInput Input { get { return Character.Input; } }

        public virtual void Configure(Character character)
        {
            this.Character = character;

            Character.OnInit += Init;
        }

        protected virtual void Init()
        {

        }
    }

    public abstract class Module : MonoBehaviour, IProperty
    {
        public Character Character { get; protected set; }

        public CharacterInput Input { get { return Character.Input; } }

        public virtual void Configure(Character character)
        {
            this.Character = character;

            Character.OnInit += Init;
        }

        protected virtual void Init()
        {

        }
    }

    public List<IProperty> Properties { get; protected set; }
    public void ForAllModules(Action<IProperty> action)
    {
        for (int i = 0; i < Properties.Count; i++)
        {
            action(Properties[i]);
        }
    }
    public TType FindProperty<TType>()
        where TType : class, IProperty
    {
        var type = typeof(TType);

        for (int i = 0; i < Properties.Count; i++)
        {
            if (Properties[i].GetType() == type)
                return Properties[i] as TType;
        }

        return null;
    }
    #endregion

    public CharacterInput Input { get; protected set; }

    public CharacterCamera camera { get; protected set; }

    public CharacterBody Body { get; protected set; }

    public CharacterMovement Movement { get; protected set; }

    public CharacterMagicalSpells MagicalSpells { get; protected set; }

    public CharacterAttack Attack { get; protected set; }

    #region Transform
    public Vector3 Position { get { return transform.position; } set { transform.position = value; } }
    public Quaternion Rotation { get { return transform.rotation; } set { transform.rotation = value; } }
    #endregion

    private void Start()
    {
        Configure();

        Init();
    }

    private void Configure()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();

        Properties = new List<IProperty>();

        var modules = GetComponentsInChildren<Module>();

        for (int i = 0; i < modules.Length; i++)
            Properties.Add(modules[i]);

        camera = FindObjectOfType<CharacterCamera>();
        Properties.Add(camera);

        Input = FindProperty<CharacterInput>();
        Body = FindProperty<CharacterBody>();
        Movement = FindProperty<CharacterMovement>();
        MagicalSpells = FindProperty<CharacterMagicalSpells>();
        Attack = FindProperty<CharacterAttack>();

        void Process(IProperty module) => module.Configure(this);

        ForAllModules(Process);
    }

    public event Action OnInit;
    private void Init()
    {
        OnInit?.Invoke();
    }

    private void Update()
    {
        Process();
    }
    public event Action OnProcess;
    void Process()
    {
        if (OnProcess != null) OnProcess();
    }

    private void LateUpdate()
    {
        LateProcess();
    }
    public event Action OnLateProcess;
    void LateProcess()
    {
        OnLateProcess?.Invoke();
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword