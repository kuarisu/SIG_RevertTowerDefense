using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car_SelfManagement : MonoBehaviour {

    [SerializeField] float m_DrivingSpeed;

    Animator m_An;
    NavMeshAgent m_Agent;


	// Use this for initialization
	void Start () {

        m_Agent = this.GetComponent<NavMeshAgent>();

        m_An = this.GetComponent<Animator>();
        m_An.GetBehaviour<Car_Driving>().SetKeyPointsPos(Manager_PatrolList.Instance.m_ListOfWaypoints);
        m_An.GetBehaviour<Car_Driving>().SetAgent(m_Agent);

    }

    private void Update()
    {
        if(Input.GetKeyUp("q"))
        {
            DrivingBehavior();
        }

        if(Input.GetKeyUp("d"))
        {
            FiringBehavior();
        }
    }


    public void DrivingBehavior()              //Met l'objet en état Walking
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", false);
        m_An.SetBool("m_Driving", true);

        m_Agent.speed = m_DrivingSpeed;             //Set la vitesse de déplacement pendant l'état de Walk

    }

    public void FiringBehavior()              //Met l'objet en état Walking
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", true);
        m_An.SetBool("m_Driving", false);

        m_Agent.speed = 0;
    }

    public void DestroyedBehavior()              //Met l'objet en état Walking
    {
        m_An.SetBool("m_Destroyed", true);
        m_An.SetBool("m_Firing", false);
        m_An.SetBool("m_Driving", false);

        m_Agent.speed = 0;
    }
}
