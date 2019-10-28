using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
public class MagicalSpell : MonoBehaviour
{
    [SerializeField]
    protected ParticlesData particles;
    public ParticlesData Particles => particles;
    [Serializable]
    public class ParticlesData
    {
        [SerializeField]
        ParticleSystem component = null;
        public ParticleSystem Component => component;

        [SerializeField]
        ParticleSystem[] simulated = new ParticleSystem[] { };
        public ParticleSystem[] Simulated => simulated;

        public void SetSimulation(ParticleSystemSimulationSpace space)
        {
            for (int i = 0; i < simulated.Length; i++)
            {
                simulated[i].Stop(true);

                var main = simulated[i].main;

                main.simulationSpace = space;

                simulated[i].Play(true);
            }
        }
        public void SetSimulationSpace(Transform target)
        {
            for (int i = 0; i < simulated.Length; i++)
            {
                var main = simulated[i].main;

                main.simulationSpace = ParticleSystemSimulationSpace.Custom;
                main.customSimulationSpace = target;
            }
        }
    }

    [SerializeField]
    GameObject explosion = null;

    public Rigidbody rigidbody { get; protected set; }
    public SphereCollider collider { get; protected set; }

    public bool Armed { get => collider.enabled; set => collider.enabled = value; }

    public void Configure()
    {
        rigidbody = GetComponent<Rigidbody>();

        collider = GetComponent<SphereCollider>();

        Armed = false;
    }

    public void Init()
    {

    }

    public void IgnoreCollision(GameObject gameObject)
    {
        var colliders = gameObject.GetComponentsInChildren<Collider>();

        for (int i = 0; i < colliders.Length; i++)
        {
            Physics.IgnoreCollision(collider, colliders[i], true);
        }
    }

    public Vector3 DirectionTo(Vector3 point)
    {
        return (point - transform.position).normalized;
    }

    public void Launch(Vector3 velocity)
    {
        transform.SetParent(null);

        particles.SetSimulation(ParticleSystemSimulationSpace.World);

        rigidbody.velocity = velocity;

        Armed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Armed == false) return;

        Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword