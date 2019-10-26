using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class BoneModification : AnimatorStateMachiner.State
{
    [SerializeField]
    ElementData[] elements;
    [Serializable]
    public class ElementData
    {
        [SerializeField]
        HumanBodyBones target;

        [SerializeField]
        Vector3 angles;

        public void Apply(Animator animator, float weight)
        {
            var bone = animator.GetBoneTransform(target);

            bone.localEulerAngles += angles * weight;
        }
    }

    [SerializeField]
    float speed = 4f;

    int layerIndex;
    public float LayerWeight => Animator.GetLayerWeight(layerIndex);

    float weight = 0;
    float weightTarget = 0f;

    public override void Configure(AnimatorStateMachiner reference)
    {
        base.Configure(reference);

        Machiner.OnLateUpdate += LateUpdate;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        this.layerIndex = layerIndex;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);

        weightTarget = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex, controller);

        weightTarget = 1f;
    }

    private void LateUpdate()
    {
        Apply();
    }

    void Apply()
    {
        weight = Mathf.MoveTowards(weight, weightTarget, 1f * Time.deltaTime);

        for (int i = 0; i < elements.Length; i++)
        {
            elements[i].Apply(Animator, LayerWeight * weight);
        }
    }
}