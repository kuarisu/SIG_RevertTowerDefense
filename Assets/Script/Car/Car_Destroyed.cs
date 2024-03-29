﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Destroyed : StateMachineBehaviour {
        //The gameobject to be destroyed
    GameObject m_Vehicle;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            //The function used to remove the vehicle from the game manager list is called before destroying the vehicle
        Manager_Objects.Instance.RemoveVehicle(m_Vehicle);
        Destroy(m_Vehicle);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

            //this function is used to destroy the right game object
    public void SetVehicle(GameObject _vehicle)
    {
        m_Vehicle = _vehicle;
    }

}
