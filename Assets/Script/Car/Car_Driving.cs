using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car_Driving : StateMachineBehaviour {

    List<Transform> m_ListOfKeyPoints = new List<Transform>();

    Transform m_TargetPosition;
    NavMeshAgent m_Agent;
    int m_indexOfNextTarget;
    int m_currentIndexOfNextTarget;
    bool m_HasArrivedDestination;
    float m_DistanceToDestination;
    bool m_IsClose = false;
    bool m_AtEndPoint = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_indexOfNextTarget = m_currentIndexOfNextTarget;
        SetTarget(m_ListOfKeyPoints[m_currentIndexOfNextTarget].transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        m_Agent.destination = m_TargetPosition.position;
        m_DistanceToDestination = Vector3.Distance(m_TargetPosition.position, m_Agent.transform.position);

        if(m_indexOfNextTarget == m_ListOfKeyPoints.Count -1)
        {
            m_AtEndPoint = true;
        }

        if (m_DistanceToDestination < 5 && !m_AtEndPoint)
        {
            m_IsClose = true;
            m_indexOfNextTarget++;
            m_currentIndexOfNextTarget = m_indexOfNextTarget;
            SetTarget(m_ListOfKeyPoints[m_indexOfNextTarget].transform);
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}


    public void SetKeyPointsPos(List<Transform> _ListOfKeyPosition)
    {
        m_ListOfKeyPoints = _ListOfKeyPosition;
    }
    public void SetTarget(Transform _newPos)
    {

        m_TargetPosition = _newPos;
    }

    public void SetAgent(NavMeshAgent _currentAgent)
    {
        m_Agent = _currentAgent;
    }
}
