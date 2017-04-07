using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Car_SelfManagement : MonoBehaviour {

    [SerializeField]
    float m_DrivingSpeed;
    [SerializeField]
    int m_HealthPoints;
    [SerializeField]
    float m_DistanceToTarget;
    [SerializeField]
    float m_SpeedRotation;

    bool m_IsShooting = false;
    Quaternion m_Orientation;

    Animator m_An;
    NavMeshAgent m_Agent;


	// Use this for initialization
	void Start () {

        m_Agent = this.GetComponent<NavMeshAgent>();

        m_An = this.GetComponent<Animator>();
        m_An.GetBehaviour<Car_Driving>().SetKeyPointsPos(Manager_Objects.Instance.m_ListOfWaypoints);
        m_An.GetBehaviour<Car_Driving>().SetAgent(m_Agent);
        m_An.GetBehaviour<Car_Shooting>().SetParameters(m_SpeedRotation, this.gameObject);

        DrivingBehavior();

    }

    private void Update()
    {
        if(m_IsShooting)
        {
            m_Orientation = Quaternion.LookRotation(Manager_Input.Instance.m_Target.transform.position - transform.position);
            m_An.GetBehaviour<Car_Shooting>().SetOrientation(m_Orientation);
        }

        if(Input.GetKeyUp("q"))
        {
            DrivingBehavior();
        }

        if(Input.GetKeyUp("d"))
        {
            FiringBehavior();
        }

        if (Input.GetKeyUp("z"))
        {
            DestroyedBehavior();
        }
    }


    public void DrivingBehavior()           
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", false);
        m_An.SetBool("m_Driving", true);

        m_IsShooting = false;
        m_Agent.isStopped = false;
        m_Agent.speed = m_DrivingSpeed;            


    }

    public void FiringBehavior()             
    {
        m_An.SetBool("m_Destroyed", false);
        m_An.SetBool("m_Firing", true);
        m_An.SetBool("m_Driving", false);

        m_IsShooting = true;
        m_Agent.isStopped = true;
    }

    public void DestroyedBehavior()             
    {
        m_An.SetBool("m_Destroyed", true);
        m_An.SetBool("m_Firing", false);
        m_An.SetBool("m_Driving", false);

        m_An.GetBehaviour<Car_Destroyed>().SetVehicle(this.gameObject.transform.root.gameObject);

        m_IsShooting = false;
        m_Agent.isStopped = true;
    }

    public void LooseHealthPoint()
    {
        m_HealthPoints--;
        if (m_HealthPoints <= 0)
            DestroyedBehavior();

    }

    public void PrepareShooting()
    {
        if(Vector3.Distance(this.transform.position, Manager_Input.Instance.m_Target.transform.position) < m_DistanceToTarget)
        {
            if (true)
            {
                FiringBehavior();
            }
        }
    }
}
