using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerWeight : StateMachineBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    float target = 1f;

    [SerializeField]
    float speed = 2f;

    float value;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        var weight = animator.GetLayerWeight(layerIndex);

        value = Mathf.MoveTowards(weight, target, speed * Time.deltaTime);

        animator.SetLayerWeight(layerIndex, value);
    }
}