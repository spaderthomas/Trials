﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetInputTriggers : StateMachineBehaviour
{
    public bool IncludingDodge = false;
    public bool IncludingAttackRecoil = false;
    bool IncludingJump = true;
    public bool IncludingLadderLockout = false;
    public bool OnEntry = true;
    public bool OnExit = false;
    public bool Lockout = false;
    public bool MashPrev = false;
    public int MashPreventionID;
    public bool ResetHeldTime = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnEntry)
        {
            Reset(animator);
        }
       
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Lockout || (MashPrev && animator.GetInteger("Input-AttackID") == MashPreventionID))
        {
            Reset(animator);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (OnExit)
        {
            Reset(animator);
        }
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    private void Reset(Animator animator)
    {
        animator.ResetTrigger("Input-SlashDown");
        animator.ResetTrigger("Input-SlashUp");
        animator.ResetTrigger("Input-ThrustDown");
        animator.ResetTrigger("Input-ThrustUp");
        animator.ResetTrigger("Input-HeavyDown");
        animator.ResetTrigger("Input-HeavyUp");

        animator.ResetTrigger("Input-Attack");
        animator.ResetTrigger("Input-AttackUp");
        animator.ResetTrigger("Input-AttackDown");

        animator.ResetTrigger("Input-Parry");
        animator.SetBool("Parry-Success", false);

        if (IncludingDodge)
        {
            animator.ResetTrigger("Input-DodgeDown");
            animator.ResetTrigger("Input-DodgeUp");
        }
        if (IncludingAttackRecoil)
        {
            animator.ResetTrigger("AttackBlocked");
        }
        if (IncludingJump)
        {
            animator.SetBool("Input-Jump", false);
        }
        if (IncludingLadderLockout)
        {
            animator.SetBool("LadderLockout", false);
        }
        if (ResetHeldTime)
        {
            animator.SetFloat("InputHeldTime", 0f);
        }
        animator.SetBool("Input-Player", false);

        animator.SetBool("Input-AttackBlockOkay", false);
        animator.SetBool("Input-AttackFallingOkay", false);
        animator.SetBool("Input-AttackSprintOkay", false);
        animator.SetBool("Input-AttackParryOkay", false);
    }
}
