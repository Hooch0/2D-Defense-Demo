using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIAnimator
{
    public Action AttackFinished { get; set; }

    public Animator AnimatorSystem;
    [Range(0,0.99f), Tooltip("Attack animation completion percentage before AttackFInished is called so damage can be dealt.")]
    public float AttackCompletion = 0.95f;
   
    private AIController _ai;

    public void Initialize(AIController controller)
    {
        _ai = controller;
        _ai.Attacking += OnAttack;
        _ai.Destroyed += OnDestroyed;
    }

    public void Update(float deltaTime)
    {
        UpdateMovementAnimation();
        UpdateAttackAnimationCheck();
    }

    private void UpdateAttackAnimationCheck()
    {
        //Check if the "Attack" animation is past the set completion time 
        if (AnimatorSystem.GetCurrentAnimatorStateInfo(0).IsName("Attack") && AnimatorSystem.GetCurrentAnimatorStateInfo(0).normalizedTime % 1 > AttackCompletion)
        {
            AttackFinished?.Invoke();
        }
    }

    private void UpdateMovementAnimation()
    {

        //Set the movmement animation based on the AI's velocity.
        if (_ai.Velocity.x != 0)
        {
            AnimatorSystem.SetFloat("Speed",1);
        }
        else
        {
             AnimatorSystem.SetFloat("Speed",0);
        }
    }

    private void OnAttack()
    {
        AnimatorSystem.SetTrigger("IsAttacking");
    }

    private void OnDestroyed()
    {
        //Cleanup
        _ai.Destroyed -= OnDestroyed;
        _ai.Attacking -= OnAttack;
    }
}
