using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCameraRaycast : CharacterCamera.Module
{
    [SerializeField]
    LayerMask mask = Physics.DefaultRaycastLayers;

    RaycastHit hit;
    public RaycastHit Hit => hit;

    public bool HasHit => hit.collider != null;

    protected override void Init()
    {
        base.Init();

        Character.OnProcess += Process;
    }

    private void Process()
    {
        Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity, mask, QueryTriggerInteraction.Ignore);
    }
}