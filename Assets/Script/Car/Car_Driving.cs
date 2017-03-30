using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car_Driving : StateMachineBehaviour {

    List<Transform> m_ListOfKeyPoints = new List<Transform>();

    Transform m_TargetPosition;
    NavMeshAgent m_Agent;
    int m_indexOfNextTarget = 0;
    int m_currentIndexOfNextTarget = 0;
    bool m_HasArrivedDestination;
    float m_DistanceToDestination;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_currentIndexOfNextTarget = m_indexOfNextTarget;
        SetTarget(m_ListOfKeyPoints[m_currentIndexOfNextTarget].transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Agent.destination = m_TargetPosition.position;
        m_DistanceToDestination = Vector3.Distance(m_TargetPosition.position, m_Agent.transform.position);

        if (m_DistanceToDestination < 3)
        {

            if (m_indexOfNextTarget < (m_ListOfKeyPoints.Count - 1))
            {
                SetTarget(m_ListOfKeyPoints[m_indexOfNextTarget].transform);
                m_indexOfNextTarget++;
            }
            else if (m_indexOfNextTarget == m_ListOfKeyPoints.Count - 1)
            {
                SetTarget(m_ListOfKeyPoints[m_indexOfNextTarget].transform);
                m_indexOfNextTarget = 0;
            }
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
