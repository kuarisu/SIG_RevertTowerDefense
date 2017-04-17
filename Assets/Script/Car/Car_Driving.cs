using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car_Driving : StateMachineBehaviour {

        //List of the waypoints the vehicle follows
    List<Transform> m_ListOfWayPoints = new List<Transform>();

        //The current target that vehicle needs to reach
    Transform m_TargetPosition;
        //The nav mesh agent of the vehicle
    NavMeshAgent m_Agent;
        //The index of the next target inside the waypoint list
    int m_indexOfNextTarget;
        //This indew is used to store the index of the next target, even if the vehicle is not in the driving state.
    int m_currentIndexOfNextTarget;
        //the minimal distance between the vehicle and its target 
    float m_DistanceToDestination;
        //A bool to check if the vehicle is at the end of the list
    bool m_AtEndPoint = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            //To make sure the vehicle is going to right target, even if it stopped driving, the index of the next target is the one stored in current index.
        m_indexOfNextTarget = m_currentIndexOfNextTarget;
            //The target of the vehicle is now the tranform of the gameobject inside the list of way point, at the index stored inside current index
        SetTarget(m_ListOfWayPoints[m_currentIndexOfNextTarget].transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

            //The destination of the agent is the target position
        m_Agent.destination = m_TargetPosition.position;
            //It checks the distance between the agent position and the target position
        m_DistanceToDestination = Vector3.Distance(m_TargetPosition.position, m_Agent.transform.position);

            //We check if the current target is the last one of the list
        if(m_indexOfNextTarget == m_ListOfWayPoints.Count -1)
        {
            m_AtEndPoint = true;
        }
            //If the vehicle is too close to the target and if it's the last one in the waypoints list, the index of the next target is increased by one, it's stored in the current index and a new target is set
        if (m_DistanceToDestination < 5 && !m_AtEndPoint)
        {
            m_indexOfNextTarget++;
            m_currentIndexOfNextTarget = m_indexOfNextTarget;
            SetTarget(m_ListOfWayPoints[m_indexOfNextTarget].transform);
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

        //This function is use to communicate the list of waypoints from the game manager
    public void SetKeyPointsPos(List<Transform> _ListOfKeyPosition)
    {
        m_ListOfWayPoints = _ListOfKeyPosition;
    }
        //This function is used to set the new agent's target
    public void SetTarget(Transform _newPos)
    {

        m_TargetPosition = _newPos;
    }
        //This function is used to set the agent
    public void SetAgent(NavMeshAgent _currentAgent)
    {
        m_Agent = _currentAgent;
    }
}
