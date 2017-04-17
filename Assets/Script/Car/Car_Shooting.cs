using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Shooting : StateMachineBehaviour {

        //Speed of the vehicle's rotation
    float m_SpeedRotation;
        //The gameObject that rotates
    GameObject m_DynamicVisual;
        //The orientation the gameobject will reach
    Quaternion m_Orientation;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            //The gameobject rotates toward the set orientation using a slerp
        m_DynamicVisual.transform.localRotation = Quaternion.Slerp(m_DynamicVisual.transform.localRotation, m_Orientation, m_SpeedRotation * Time.deltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

        //This function is used to set several parameters 
    public void SetParameters(float _speedRotation, GameObject _dynamicVisual)
    {
        m_SpeedRotation = _speedRotation;
        m_DynamicVisual = _dynamicVisual;
    }
        //This function is called to set the new orientation toward the vehicle's target
    public void SetOrientation(Quaternion _orientation)
    {
        m_Orientation = _orientation;
    }
}
